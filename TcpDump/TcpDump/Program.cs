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
                Broadcast receiveAll = new Broadcast();
                receiveAll.ReceiveAllPackets();
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

        /// <summary>
        /// Gets local computer IP address
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLocalIPAddress()
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
