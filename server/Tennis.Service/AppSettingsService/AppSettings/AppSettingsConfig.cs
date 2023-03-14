namespace Tennis.Service.AppSettingsService.AppSettings;
public class AppSettingsConfig : IAppSettings
{
    public string? JwtSecretKey { get; set; } = "";
    public string? JwtValid { get; set; } = "";
    public string? RefreshTokenValid { get; set; } = "";
}