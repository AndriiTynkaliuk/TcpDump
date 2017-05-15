using System;
using System.Net;
using System.Net.Sockets;

namespace TcpDump
{
    class Program
    {
        private static string manual = "  empty\t\tDisplays all connections and listening ports.\n  -p\t\tShows connections for the protocol specified"
            + " after '-p'. It   \t\t\tcould be TCP or UDP.\n  -i\t\tShows connections for the secified Source IP.\n  -id\t\tShows connections for the secified Destination IP.\n"
            + "  -os\t\tShows connections for the secified Source Port.\n  -osp\t\tShows connections for the secified Source IP and Port.";

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

                    case "-i": // filter by source IP

                        if(args.Length > 1)
                        {
                            Wale ipFiltering = new Wale();
                            ipFiltering.ReceivePacket(FilterType.All, SpecFilter.SourceIP, args[1]);
                        }
                        else
                            Console.WriteLine("IP Address in needed");

                        break;

                    case "-id":

                        if (args.Length > 1)
                        {
                            Wale ipFiltering = new Wale();
                            ipFiltering.ReceivePacket(FilterType.All, SpecFilter.DestIP, args[1]);
                        }
                        else
                            Console.WriteLine("IP Address in needed");

                        break;

                    case "-os": // filter by source Port (-os Port#)

                        if (args.Length > 1)
                        {
                            Wale ipFiltering = new Wale();
                            ipFiltering.ReceivePacket(FilterType.All, SpecFilter.SourcePort, args[1]);
                        }
                        else
                            Console.WriteLine("Port number is needed");

                        break;
                    case "-osp": // Filter by source port and protocol

                        if (args.Length > 1)
                        {
                            Wale ipFiltering = new Wale();
                            if (args.Length > 2)
                            {
                                if(args[1] == "tcp" || args[1] == "TCP")
                                    ipFiltering.ReceivePacket(FilterType.TCP, SpecFilter.SourcePort, args[2]);
                                else if (args[1] == "udp" || args[1] == "UDP")
                                    ipFiltering.ReceivePacket(FilterType.UDP, SpecFilter.SourcePort, args[2]);
                                else if (args[1] == "-a" || args[1] == "-A")
                                    ipFiltering.ReceivePacket(FilterType.All, SpecFilter.SourcePort, args[2]);
                            }
                            else
                                Console.WriteLine("Enter a Port number");
                            break;
                        }
                        else
                            Console.WriteLine("Enter type of protocol");

                        break;

                    case "man":

                        Console.WriteLine(manual);

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
