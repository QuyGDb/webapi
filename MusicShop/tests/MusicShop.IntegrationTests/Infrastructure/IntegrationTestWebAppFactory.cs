using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MusicShop.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace MusicShop.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("MusicShopTestDb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove existing DbContext configuration (Dev DB)
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

            // Configure the test DbContext to use the Testcontainers DB
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        // 1. Start the PostgreSQL container
        await _dbContainer.StartAsync();

        // 2. Automatically run migrations on the virtually created DB
        // (The container starts highly clean, with no tables)
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        // Destroy the container immediately after all tests finish
        await _dbContainer.DisposeAsync().AsTask();
    }
}
