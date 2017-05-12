using System;
using System.Net;
using System.Net.Sockets;

namespace TcpDump
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Broadcast receiveAll = new Broadcast();
                receiveAll.ReceiveAllPackets(FilterType.All);
            }    
            else
            {
                switch(args[0])
                {
                    case "-p":

                        if(args.Length > 1)
                        {
                            Broadcast displayPackets = new Broadcast();
                            if (args[1] == "tcp" || args[1] == "TCP")
                            {
                                displayPackets.ReceiveAllPackets(FilterType.TCP);
                            }
                            else if (args[1] == "udp" || args[1] == "UDP")
                            {
                                displayPackets.ReceiveAllPackets(FilterType.UDP);
                            }
                            else
                                Console.WriteLine("Unknown protocol");
                            break;
                        }
                        else
                            Console.WriteLine("Specify the protocol, please");

                        break;

                    case "-i":

                        if(args.Length > 1)
                        {

                        }
                        else
                            Console.WriteLine("IP Address in needed");

                        break;

                    case "-os":

                        if (args.Length > 1)
                        {
                            Wale ipFiltering = new Wale();
                            if (args.Length > 2)
                            {
                                ipFiltering.ReceivePacket(FilterType.All, SpecFilter.SourcePort, args[1]);
                            }
                            else
                                Console.WriteLine("Enter an IP address");
                            break;
                        }
                        else
                            Console.WriteLine("IP Address in needed");

                        break;
                    case "-osp":

                        if (args.Length > 1)
                        {
                            Wale ipFiltering = new Wale();
                            if (args.Length > 2)
                            {
                                if(args[1] == "tcp" || args[1] == "TCP")
                                    ipFiltering.ReceivePacket(FilterType.TCP, SpecFilter.SourcePort, args[2]);
                                else if (args[1] == "udp" || args[1] == "UDP")
                                    ipFiltering.ReceivePacket(FilterType.UDP, SpecFilter.SourcePort, args[2]);
                            }
                            else
                                Console.WriteLine("Enter a Port number");
                            break;
                        }
                        else
                            Console.WriteLine("Enter type of protocol");

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
