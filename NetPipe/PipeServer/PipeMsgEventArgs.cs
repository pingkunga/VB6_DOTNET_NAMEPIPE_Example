using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeServer
{
    public class PipeMsgEventArgs : EventArgs
    {
        public string Request { get; set; }
        public string Response { get; set; }

        public PipeMsgEventArgs()
        {

        }

        public PipeMsgEventArgs(string request)
        {
            this.Request = request;
        }
    }
}
