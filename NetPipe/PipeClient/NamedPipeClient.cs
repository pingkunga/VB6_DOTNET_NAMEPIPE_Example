using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeClient
{
    public class NamedPipeClient : IDisposable
    {
        private NamedPipeClientStream client;
        private StreamReader reader;
        private StreamWriter writer;

        public NamedPipeClient(string pipeName) : this(pipeName, 100) { }

        public NamedPipeClient(string pipeName, int timeOut)
        {
            client = new NamedPipeClientStream(pipeName);
            client.Connect();
            reader = new StreamReader(client);
            writer = new StreamWriter(client);
        }

        public void Dispose()
        {
            writer.Dispose();
            reader.Dispose();
            client.Dispose();
        }

        public string SendRequest(string request)
        {
            if (request != null)
            {
                try
                {
                    writer.WriteLine(request);
                    writer.Flush();
                    return reader.ReadLine();
                }
                catch (Exception ex)
                {
                    return string.Format("{0}\r\nDetails:\r\n{1}", "Error on server communication.", ex.Message);
                }
            }
            else
            {
                return "Error. Null request.";
            }
        }
    }
}
