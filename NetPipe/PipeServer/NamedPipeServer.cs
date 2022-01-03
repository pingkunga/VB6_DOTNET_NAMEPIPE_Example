using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeServer
{
    public class NamedPipeServer
    {
        private readonly string pipeName;
        private readonly int maxNumberOfServerInstances;
        private readonly int initialNumberOfServerInstances;
        private List<NamedPipeServerInstance> servers = new List<NamedPipeServerInstance>();

        public event EventHandler<PipeMsgEventArgs> newRequestEvent = delegate { };
        public event EventHandler<string> newInstanctEvent = delegate { };
        public event EventHandler<string> disposeEvent = delegate { };

        public NamedPipeServer(string pipeName) : this(pipeName, 20, 1) { }

        public NamedPipeServer(string pipeName, int maxNumberOfServerInstances, int initialNumberOfServerInstances)
        {
            this.pipeName = pipeName;
            this.maxNumberOfServerInstances = maxNumberOfServerInstances;
            this.initialNumberOfServerInstances = initialNumberOfServerInstances;
        }
        public void Start()
        {
            for (int i = 0; i < initialNumberOfServerInstances; i++)
            {
                NewServerInstance();
            }
        }
        public void Dispose()
        {
            CleanServers(true);
        }

        private void NewServerInstance()
        {
            // Start a new server instance only when the number of server instances
            // is smaller than maxNumberOfServerInstances
            if (servers.Count < maxNumberOfServerInstances)
            {
                NamedPipeServerInstance server = new NamedPipeServerInstance(pipeName, maxNumberOfServerInstances);

                server.newServerInstanceEvent += (s, e) => { NewServerInstance(); };

                server.newRequestEvent += (s, e) => { newRequestEvent.Invoke(s, e); };

                newInstanctEvent.Invoke(this, "New Instance");
                servers.Add(server);
            }

            // Run clean servers anyway
            CleanServers(false);
        }

        /// <summary>
        /// A routine to clean NamedPipeServerInstances. When disposeAll is true,
        /// it will dispose all server instances. Otherwise, it will only dispose
        /// the instances that are completed, canceled, or faulted.
        /// PS: disposeAll is true only for this.Dispose()
        /// </summary>
        /// <param name="disposeAll"></param>
        private void CleanServers(bool disposeAll)
        {
            if (disposeAll)
            {
                foreach (NamedPipeServerInstance server in servers)
                {
                    server.Dispose();
                    disposeEvent.Invoke(this, "Dispose");
                }
                servers.Clear();
            }
            else
            {
                for (int i = servers.Count - 1; i >= 0; i--)
                {
                    if (servers[i] == null)
                    {
                        servers.RemoveAt(i);
                    }
                    else if (servers[i].TaskCommunication != null &&
                        (servers[i].TaskCommunication.Status == TaskStatus.RanToCompletion ||
                        servers[i].TaskCommunication.Status == TaskStatus.Canceled ||
                        servers[i].TaskCommunication.Status == TaskStatus.Faulted))
                    {
                        servers[i].Dispose();
                        servers.RemoveAt(i);
                        disposeEvent.Invoke(this, "Dispose at " + i);
                    }
                }
            }
        }
    }
}
