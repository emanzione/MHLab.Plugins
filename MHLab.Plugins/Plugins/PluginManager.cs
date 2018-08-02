using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace MHLab.Plugins
{
    public partial class PluginsManager : IDisposable
    {
        /// <summary>
        /// The list of loaded plugins.
        /// </summary>
        public List<IPlugin> Plugins;

        /// <summary>
        /// The host to all plugins are bound to.
        /// </summary>
        public IPluginHost Host;

        public String PluginsDirectory
        {
            get
            {
                return Host.PluginsDirectory;
            }
        }

        /// <summary>
        /// Initialize the plugins manager and load plugins.
        /// </summary>
        /// <param name="host">The host.</param>
        public PluginsManager(IPluginHost host)
        {
            Plugins = new List<IPlugin>();
            Host = host;

            // Load plugin classes contained in the same assembly of current IPluginHost.
            if (Host != null)
            {
                Assembly ass = Host.GetType().Assembly;
                foreach (Type type in ass.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Plugin))))
                {
                    Plugin plugin = (Plugin)Activator.CreateInstance(type);
                    plugin.Host = Host;
                    Plugins.Add(plugin);
                }
            }
            
            // Check if plugins directory exists.
            if (!Directory.Exists(PluginsDirectory))
            {
                Directory.CreateDirectory(PluginsDirectory);
            }

            // Retrieve all .dll contained in PluginsDirectory.
            IEnumerable<string> files = Directory.GetFiles(PluginsDirectory, "*." + Host.PluginsExtension, SearchOption.AllDirectories);
            files = HooksManager.ExecuteFilter<IEnumerable<string>>("retrieved_plugins", files);
            foreach (string file in files)
            {
                try
                {
                    // Load the retrieved dll.
                    Assembly ass = Assembly.LoadFile(file);

                    // Load plugin classes contained in this dll
                    foreach (Type type in ass.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Plugin))))
                    {
                        Plugin plugin = (Plugin)Activator.CreateInstance(type);
                        plugin.Host = Host;
                        Plugins.Add(plugin);
                    }
                }
                catch (Exception e)
                {
                    HooksManager.ExecuteAction("on_exception", e);
                }
            }

            // Loop over loaded plugins and call OnLoad method
            for (int i = 0; i < Plugins.Count; i++)
            {
                Plugins[i].OnLoad();
            }
        }

        public void Dispose()
        {
            // Loop over loaded plugins and call OnUnload method
            for (int i = 0; i < Plugins.Count; i++)
            {
                Plugins[i].OnUnload();
            }
        }
    }
}
