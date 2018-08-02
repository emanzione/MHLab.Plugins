namespace MHLab.Plugins
{
    public interface IPlugin
    {
        string Name { get; set; }
        IPluginHost Host { get; set; }

        void OnLoad();
        void OnUnload();
    }
}
