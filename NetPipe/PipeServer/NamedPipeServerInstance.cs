using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;

namespace PipeServer
{
    public class NamedPipeServerInstance : IDisposable
    {
        public const bool IsUTF8 = false;                //WHEN CLIENT .NET

        private NamedPipeServerStream _instanceServer;
        private bool disposeFlag = false;

        public Task TaskCommunication { get; private set; }

        public event EventHandler newServerInstanceEvent = delegate { };
        public event EventHandler<PipeMsgEventArgs> newRequestEvent = delegate { };

        public NamedPipeServerInstance(string pipeName, int maxNumberOfServerInstances)
        {
            PipeSecurity ps = new PipeSecurity();
            //ps.AddAccessRule(new PipeAccessRule(@"NT AUTHORITY\NETWORK", PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Deny));
            ps.AddAccessRule(new PipeAccessRule(string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName), PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.ReadWrite, AccessControlType.Allow));
            _instanceServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxNumberOfServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous, maxNumberOfServerInstances, maxNumberOfServerInstances, ps);
            IAsyncResult asyncResult = _instanceServer.BeginWaitForConnection(OnConnected, null);
        }

        public void Dispose()
        {
            disposeFlag = true;
            _instanceServer.Dispose();
        }

        private void OnConnected(IAsyncResult result)
        {
            if (!disposeFlag)
            {
                _instanceServer.EndWaitForConnection(result);

                newServerInstanceEvent.Invoke(this, EventArgs.Empty);

                TaskCommunication = Task.Factory.StartNew(Communication);
            }
        }

        private void Communication()
        {
            StringBuilder messageBuilder = new StringBuilder();
            string messageChunk = string.Empty;
            byte[] messageBuffer = new byte[1024];

            do
            {
                _instanceServer.Read(messageBuffer, 0, messageBuffer.Length);

                messageChunk = CreateRequestString(messageBuffer, IsUTF8);

                messageBuilder.Append(messageChunk);
                messageBuffer = new byte[messageBuffer.Length];
            }
            while (!_instanceServer.IsMessageComplete);
            PipeMsgEventArgs msgEventArgs = new PipeMsgEventArgs(messageBuilder.ToString());
            newRequestEvent.Invoke(this, msgEventArgs);
            string response = msgEventArgs.Response + Environment.NewLine;
            
            byte[] byteSend = CreateResponseString(response, IsUTF8);
            _instanceServer.Write(byteSend, 0, byteSend.Count());
        }

        private String CreateRequestString(byte[] messageBuffer, bool pIsUTF8)
        {
            if (pIsUTF8)
            {
                //NOTE ถ้าคุยกับ Application ที่ใช่ UTF8 ได้ ควรใข้อันนี้
                return Encoding.UTF8.GetString(messageBuffer);
            }
            else
            {
                //NOTE : VB6 Send ASCII Byte
                //Encoding.Default < System.Globalization.CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
                String messageChunk = Encoding.Default.GetString(messageBuffer);
                //Clear \0 Non-String such as \0ทดสอบ\0\0\0\0\0 >> ทดสอบ
                messageChunk = messageChunk.Replace("\0", string.Empty);

                return messageChunk;
            }
        }

        private byte[] CreateResponseString(String response, bool pIsUTF8)
        {
            if (pIsUTF8)
            {
                //NOTE ถ้าคุยกับ Application ที่ใช่ UTF8 ได้ ควรใข้อันนี้
                return Encoding.UTF8.GetBytes(response);
            }
            else
            {
                return Encoding.Default.GetBytes(response);
            }
        }
    }
}
