using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpDump
{
    class Program
    {
        private static Socket mainSocket;
        private byte[] byteData = new byte[4096];
        private static bool ContinueCapturing = false;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayAllPackats(ProtocolType.Tcp);
                DisplayAllPackats(ProtocolType.Udp);
            }    
            else
            {
                switch(args[0])
                {
                    case "TCP":
                        break;
                    case "UDP":
                        break;
                    case "-a":
                        break;
                }
            }
            Console.ReadKey(); // prevents from closing window after work is finished
        }

        static void DisplayAllPackats(ProtocolType protocol)
        {
            try
            {
                if(!ContinueCapturing)
                {
                    byte[] byteData = new byte[1000];

                    mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, protocol);
                    IPEndPoint endPoint = new IPEndPoint(GetLocalIPAddress(), 0); 
                    mainSocket.Bind(endPoint);
                    mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

                    byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                    byte[] byOut = new byte[4];

                    mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
                else
                {
                    ContinueCapturing = false;
                    mainSocket.Close();
                }                
            }
            catch(Exception e)
            {
                Console.WriteLine("Display: " + e.StackTrace + " | " + e.Message);
            }
        }

        private static void OnReceive(IAsyncResult res)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(res);
            }
            catch(Exception ex)
            {
                throw new Exception("OnReceive - " + ex.Message);
            }
        }

        /// <summary>
        /// Gets local computer IP address
        /// </summary>
        /// <returns></returns>
        static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return null;
        }
    }
}
