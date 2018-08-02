namespace MHLab.Plugins
{
    public class Plugin : IPlugin
    {
        private IPluginHost _host;
        public IPluginHost Host
        {
            get { return _host; }

            set
            {
                _host = value;
                if (_host != null)
                    _host.OnPluginRegister(this);
            }
        }

        public string Name { get; set; }

        public virtual void OnLoad()
        {
            
        }

        public virtual void OnUnload()
        {

        }
    }
}
