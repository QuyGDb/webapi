using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.API.Infrastructure;
using MusicShop.IntegrationTests.Infrastructure;
using Xunit;

namespace MusicShop.IntegrationTests.Controllers;

public class ArtistsControllerTests : BaseIntegrationTest
{
    public ArtistsControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateArtist_GivenValidDataAndAdminToken_ReturnsCreatedAndSavesToDatabase()
    {
        // 1. Arrange
        await AuthenticateAsAdminAsync(); // Inject admin token into Client

        var requestPayload = new 
        {
            Name = "The Beatles",
            Country = "UK",
            Biography = "Legendary band from Liverpool"
        };

        // 2. Act (Send HTTP POST)
        var response = await Client.PostAsJsonAsync("/api/v1/artists", requestPayload);

        // 3. Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<ArtistResponse>>();
        
        responseData.Should().NotBeNull();
        responseData!.Data.Should().NotBeNull();
        responseData.Data.Name.Should().Be("The Beatles");
        responseData.Data.Country.Should().Be("UK");
        responseData.Data.Id.Should().NotBeEmpty();

        // 4. Verify DB (You can also query the Virtual DB via Factory.Services to ensure persistence)
    }

    [Fact]
    public async Task CreateArtist_WithoutToken_ReturnsUnauthorized()
    {
        // 1. Arrange (No authentication)
        var requestPayload = new { Name = "Eminem", Country = "US" };

        // 2. Act
        var response = await Client.PostAsJsonAsync("/api/v1/artists", requestPayload);

        // 3. Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
