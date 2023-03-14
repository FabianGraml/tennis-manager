using Microsoft.Extensions.Options;
namespace Tennis.Service.AppSettingsService;
public class AppSettingsService<T> : IAppSettingsService<T> where T : class, IAppSettings
{
    private readonly IOptions<T> _appSettings;
    public AppSettingsService(IOptions<T> appSettings)
    {
        _appSettings = appSettings;
    }
    public T GetSettings()
    {
        return _appSettings.Value;
    }
}