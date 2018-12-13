namespace DeveloperTest.Config
{
    public interface IConfigProvider
    {
        string GetAppSettings(string key);
    }
}
