using System;
using System.Net.Sockets;

namespace TcpDump
{
    class Wale : Packet
    {
        private FilterType filter;
        private string key;
        private SpecFilter whatToFilter;

        public void ReceivePacket(FilterType _filter, SpecFilter _whatToFilter, String _key)
        {
            filter = _filter;
            key = _key;
            whatToFilter = _whatToFilter;
            Console.WriteLine("Protol\tSource IP:Port\t\tDestination IP:Port");
            base.DisplayPackets(filter);
        }

        protected override void OnReceive(IAsyncResult res)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(res);

                IPHeader ipHeader = new IPHeader(byteData, nReceived);

                base.ParsePacket(ipHeader, filter);

                if (ContinueCapturing)
                {
                    byteData = new byte[4096];

                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Displays specified information
        /// </summary>
        /// <param name="header"></param>
        protected override void Print(params IHeader[] header)
        {
            if(header.Length > 0)
            {
                IPHeader ipPacket = header[0] as IPHeader;

                if(filter == FilterType.All)
                {
                    if(whatToFilter == SpecFilter.SourceIP)
                    {
                        if(ipPacket.SourceIP == key)
                        {
                            Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                ipPacket.SourceIP, header[1].SourcePort,
                                ipPacket.DestIP, header[1].DestPort));
                        }
                    }
                    else if (whatToFilter == SpecFilter.DestIP)
                    {
                        if(ipPacket.DestIP == key)
                        {
                            Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                ipPacket.SourceIP, header[1].SourcePort,
                                ipPacket.DestIP, header[1].DestPort));
                        }
                    }
                    else if (whatToFilter == SpecFilter.SourcePort)
                    {
                        if(header[1].SourcePort == key)
                        {
                            Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                ipPacket.SourceIP, header[1].SourcePort,
                                ipPacket.DestIP, header[1].DestPort));
                        }
                    }
                    else if (whatToFilter == SpecFilter.DestPort)
                    {
                        if(header[1].DestPort == key)
                        {
                            Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                ipPacket.SourceIP, header[1].SourcePort,
                                ipPacket.DestIP, header[1].DestPort));
                        }
                    }
                    else
                        Console.WriteLine("Unknown request");
                }
                else if(filter == FilterType.TCP)
                {
                    if(ipPacket.Protocol == ProtocolType.Tcp)
                    {
                        if (whatToFilter == SpecFilter.SourceIP)
                        {
                            if (ipPacket.SourceIP == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else if (whatToFilter == SpecFilter.DestIP)
                        {
                            if (ipPacket.DestIP == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else if (whatToFilter == SpecFilter.SourcePort)
                        {
                            if (header[1].SourcePort == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else if (whatToFilter == SpecFilter.DestPort)
                        {
                            if (header[1].DestPort == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else
                            Console.WriteLine("Unknown request");
                    }
                }
                else if (filter == FilterType.UDP)
                {
                    if(ipPacket.Protocol == ProtocolType.Udp)
                    {
                        if (whatToFilter == SpecFilter.SourceIP)
                        {
                            if (ipPacket.SourceIP == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else if (whatToFilter == SpecFilter.DestIP)
                        {
                            if (ipPacket.DestIP == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else if (whatToFilter == SpecFilter.SourcePort)
                        {
                            if (header[1].SourcePort == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else if (whatToFilter == SpecFilter.DestPort)
                        {
                            if (header[1].DestPort == key)
                            {
                                Console.WriteLine(string.Format("{0}\t{1} : {2}\t\t{3} : {4}", ipPacket.Protocol,
                                    ipPacket.SourceIP, header[1].SourcePort,
                                    ipPacket.DestIP, header[1].DestPort));
                            }
                        }
                        else
                            Console.WriteLine("Unknown request");
                    }
                }
                else
                    Console.WriteLine("Unknown packet was received");
            }
            else
                Console.WriteLine("An error occured, nothing to display");
        }
    }
}
