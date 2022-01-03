using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeClient
{
    public partial class MainSimpleClient : Form
    {
        private string pipeName = "invest-pipe";
        public MainSimpleClient()
        {
            InitializeComponent();
        }

        private void _sendBtn_Click(object sender, EventArgs e)
        {
            using (var client = new NamedPipeClient(pipeName))
            {
                var request = _requestText.Text;
                var response = client.SendRequest(request);

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => { _responseText.Text = response; }));
                }
                else
                {
                    _responseText.Text = response;
                }
            }
        }
    }
}
