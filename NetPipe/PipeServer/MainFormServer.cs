using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeServer
{
    public partial class MainFormServer : Form
    {
        private string _pipeName = "invest-pipe";
        NamedPipeServer _server;
        private int _serverStatus = 0;
        public MainFormServer()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                CreateServer();
            });
        }
        
        private void CreateServer()
        {
            _server = new NamedPipeServer(_pipeName);
            EventHandler<string> appentEvent = new EventHandler<string>((sender,message) => { AppendText(message); });
            _server.newInstanctEvent += appentEvent;
            _server.disposeEvent += appentEvent;
            _server.newRequestEvent += (s, e) => {
                e.Response = "Echo. " + e.Request;
                AppendText(e.Response);
            };
            _startBtn_Click(this, EventArgs.Empty);
        }

        private void AppendText(string message)
        {
            if (_consoleRichTextBox.InvokeRequired)
            {
                _consoleRichTextBox.Invoke(new Action(() =>
                {
                    _consoleRichTextBox.AppendText(message + Environment.NewLine);
                }));
            }
            else
            {
                _consoleRichTextBox.AppendText(message + Environment.NewLine);
            }
        }

        private void _startBtn_Click(object sender, EventArgs e)
        {
            if (_serverStatus == 0)
            {
                _server.Start();
                _serverStatus = 1;

                if (_startBtn.InvokeRequired)
                {
                    _startBtn.Invoke(new Action(() => {
                        _startBtn.Enabled = false;
                        _stopBtn.Enabled = true;
                    }));
                }
                else
                {
                    _startBtn.Enabled = false;
                    _stopBtn.Enabled = true;
                }
                AppendText("Server is started");
            }
        }

        private void _stopBtn_Click(object sender, EventArgs e)
        {
            if (_serverStatus == 1)
            {
                _server.Dispose();
                _serverStatus = 0;
                if (_stopBtn.InvokeRequired)
                {
                    _stopBtn.Invoke(new Action(() => {
                        _stopBtn.Enabled = false;
                        _startBtn.Enabled = true;
                    }));
                }
                else
                {
                    _stopBtn.Enabled = false;
                    _startBtn.Enabled = true;
                }
                AppendText("Server is stoped");
            }
        }
    }
}
