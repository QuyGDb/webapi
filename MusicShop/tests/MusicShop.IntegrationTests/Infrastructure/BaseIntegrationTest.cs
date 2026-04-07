using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Domain.Entities.System;
using MusicShop.Domain.Enums;
using MusicShop.Domain.Interfaces;
using MusicShop.Infrastructure.Persistence;
using MusicShop.API.Infrastructure;
using Xunit;

namespace MusicShop.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IntegrationTestWebAppFactory Factory;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected async Task AuthenticateAsAdminAsync()
    {
        var email = "admin@musicshop.com";
        var password = "SecurePassword123!";

        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (!db.Users.Any(u => u.Email == email))
        {
            var adminUser = new User
            {
                Email = email,
                FullName = "System Admin",
                Role = UserRole.Admin,
                PasswordHash = hasher.Hash(password)
            };
            db.Users.Add(adminUser);
            await db.SaveChangesAsync();
        }

        var loginPayload = new { Email = email, Password = password };
        var response = await Client.PostAsJsonAsync("/api/v1/auth/login", loginPayload);

        response.EnsureSuccessStatusCode();
        var authResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

        if (authResponse?.Data == null) throw new Exception("Could not retrieve Auth Response!");

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Data.AccessToken);
    }
}
