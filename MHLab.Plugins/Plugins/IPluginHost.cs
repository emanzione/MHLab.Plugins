namespace MHLab.Plugins
{
    public interface IPluginHost
    {
        string PluginsDirectory { get; }
        string PluginsExtension { get; }

        bool OnPluginRegister(IPlugin p);
    }
}
