using System;
using System.Net;
using System.Net.Sockets;

namespace TcpDump
{
    abstract class Packet
    {
        protected Socket mainSocket;
        protected byte[] byteData = new byte[4096];
        protected static bool ContinueCapturing = false;

        protected void DisplayPackets()
        {
            try
            {
                if (!ContinueCapturing)
                {
                    ContinueCapturing = true;

                    mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                    IPEndPoint endPoint = new IPEndPoint(Program.GetLocalIPAddress(), 0);
                    mainSocket.Bind(endPoint);
                    mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

                    byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                    byte[] byOut = new byte[4] { 1, 0, 0, 0 };

                    mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
                else
                {
                    ContinueCapturing = false;
                    mainSocket.Shutdown(SocketShutdown.Both);
                    mainSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace + " | " + e.Message);
            }
        }

        protected virtual void ParsePacket(IPHeader protocol)
        {
            try
            {
                switch(protocol.Protocol)
                {
                    case ProtocolType.Tcp:
                        Print();
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.StackTrace + " | " + ex.Message);
            }
        }

        protected abstract void OnReceive(IAsyncResult res);

        protected abstract void Print();
    }
}
