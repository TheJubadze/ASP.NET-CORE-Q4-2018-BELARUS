namespace WebApp.Services
{
    public interface IConfigurationService
    {
        int ProductsCount { get; }
        string CachePath { get; }
    }
}
