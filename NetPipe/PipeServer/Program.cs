using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Run(new MainFormServer());
        }
    }
}
