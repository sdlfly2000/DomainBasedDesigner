namespace Activator.DomainDrivenDesigner.Support.Core.Version;

public static class VersionManger
{
    private static string _version = string.Empty;
    private static object _lock = new();

    public static string GetVersion()
    {
        if (string.IsNullOrEmpty(_version))
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(_version))
                {
                    try
                    {
                        _version = File.ReadAllText("version");
                    }
                    catch (Exception ex)
                    {
                        _version = ex.Message;
                    }
                }
            }
        }

        return _version;
    }
}
