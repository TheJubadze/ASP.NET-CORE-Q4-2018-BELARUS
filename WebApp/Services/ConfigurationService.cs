using Microsoft.Extensions.Configuration;

namespace WebApp.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;
        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int ProductsCount => _configuration.GetSection("RepositorySettings").GetValue<int>("ProductsCountMax");
        public string CachePath => _configuration.GetSection("Cache").GetValue<string>("Path");
        public int CacheCapacity => _configuration.GetSection("Cache").GetValue<int>("Capacity");
        public int LifeTime => _configuration.GetSection("Cache").GetValue<int>("LifeTime");
    }
}
