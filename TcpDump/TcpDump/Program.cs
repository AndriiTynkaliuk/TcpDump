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
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments.");
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
        }

        static void DisplayAllPackats(ProtocolType protocol)
        {
            byte[] byteData = new byte[1000];

            Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, protocol);
            mainSocket.Bind(new IPEndPoint(IPAddress.Parse(GetLocalIPAddress()), 0));
            mainSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.HeaderIncluded, true);

            byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
            byte[] byOut = new byte[4];

            mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);
        }

        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP Address Not Found!";
        }
    }
}
