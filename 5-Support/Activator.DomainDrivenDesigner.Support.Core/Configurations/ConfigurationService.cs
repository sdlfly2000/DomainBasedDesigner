using Microsoft.Extensions.Configuration;

namespace Activator.DomainDrivenDesigner.Support.Core.Configurations;

public static class ConfigurationService
{
    private static IConfiguration? _configuration;
    private static object _lock = new();
    public static IConfiguration GetConfiguration()
    {
        if(_configuration is null)
        {
            lock(_lock)
            {
                if(_configuration is null)
                {
                    _configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();
                }
            }
        }

        return _configuration;
    }
}
