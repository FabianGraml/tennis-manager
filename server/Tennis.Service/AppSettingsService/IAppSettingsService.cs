namespace Tennis.Service.AppSettingsService;
public interface IAppSettingsService<T> where T : class, IAppSettings
{
    T GetSettings();
}