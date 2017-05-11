using System;
using System.Net.Sockets;

namespace TcpDump
{
    class Broadcast : Packet
    {
        IPHeader ipHeader;

        public void ReceiveAllPackets()
        {
            base.DisplayPackets();
        }

        protected override void OnReceive(IAsyncResult res)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(res);

                ipHeader = new IPHeader(byteData, nReceived);

                base.ParsePacket(ipHeader);

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

        protected override void Print()
        {
            Console.WriteLine(string.Format("{0}\t{1}\t{2}", ipHeader.Protocol, ipHeader.SourceIP, ipHeader.DestIP));
        }
    }
}
