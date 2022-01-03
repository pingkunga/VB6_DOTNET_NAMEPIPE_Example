using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeClient
{
    class Program
    {
        static void Main(string[] Args)
        {
            Application.Run(new MainSimpleClient());
        }
    }
}
