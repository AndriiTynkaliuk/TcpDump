using System;
using System.Net.Sockets;

namespace TcpDump
{
    class Broadcast : Packet
    {
        IPHeader ipHeader;
        FilterType globalFilter;

        /// <summary>
        /// 
        /// </summary>
        public void ReceiveAllPackets(FilterType filter)
        {
            globalFilter = filter;
            Console.WriteLine("Protol\tSource IP:Port\t\tDestination IP:Port");
            base.DisplayPackets(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        protected override void OnReceive(IAsyncResult res)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(res);

                ipHeader = new IPHeader(byteData, nReceived);

                base.ParsePacket(ipHeader, globalFilter);

                if(ContinueCapturing)
                {
                    byteData = new byte[4096];

                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        protected override void Print(params IHeader[] header)
        {
            if(globalFilter == FilterType.All)
            {
                var ipHeader = header[0] as IPHeader;
                TCPHeader tcpHeader;
                UDPHeader udpHeader;
                if (header[1] is TCPHeader)
                {
                    tcpHeader = header[1] as TCPHeader;
                    Console.WriteLine(string.Format("{0}\t{1}:{2}\t\t{3}:{4}",
                    ipHeader.Protocol,
                    ipHeader.SourceIP, tcpHeader.SourcePort,
                    ipHeader.DestIP, tcpHeader.DestPort));
                }
                else if (header[1] is UDPHeader)
                {
                    udpHeader = header[1] as UDPHeader;
                    Console.WriteLine(string.Format("{0}\t{1}:{2}\t\t{3}:{4}",
                    ipHeader.Protocol,
                    ipHeader.SourceIP, udpHeader.SourcePort,
                    ipHeader.DestIP, udpHeader.DestPort));
                }
            }
            else if (globalFilter == FilterType.TCP)
            {
                var ipHeader = header[0] as IPHeader;
                if(ipHeader.Protocol == ProtocolType.Tcp)
                {
                    TCPHeader tcpHeader = header[1] as TCPHeader;
                    Console.WriteLine(string.Format("{0}\t{1}:{2}\t\t{3}:{4}",
                    ipHeader.Protocol,
                    ipHeader.SourceIP, tcpHeader.SourcePort,
                    ipHeader.DestIP, tcpHeader.DestPort));
                }
            }
            else if (globalFilter == FilterType.UDP)
            {
                var ipHeader = header[0] as IPHeader;
                if (ipHeader.Protocol == ProtocolType.Udp)
                {
                    UDPHeader udpHeader = header[1] as UDPHeader;
                    Console.WriteLine(string.Format("{0}\t{1}:{2}\t\t{3}:{4}",
                    ipHeader.Protocol,
                    ipHeader.SourceIP, udpHeader.SourcePort,
                    ipHeader.DestIP, udpHeader.DestPort));
                }
            }
        }
    }
}
