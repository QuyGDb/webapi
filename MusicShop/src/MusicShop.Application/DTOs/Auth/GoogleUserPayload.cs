namespace MusicShop.Application.DTOs.Auth;

/// <summary>
/// Represents the user information extracted and verified from a Google ID Token.
/// </summary>
public record GoogleUserPayload(string Email, string Name, string ExternalId);
