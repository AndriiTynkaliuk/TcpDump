using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpDump
{
    /// <summary>
    /// Represents the TCP header fields
    /// </summary>
    public class TCPHeader
    {
        private ushort tSourcePort;
        private ushort tDestPort;

        private uint tSequenceNum;
        private uint tAcknowlegmentNum;

        private ushort tHeaderLenReservFlag;
        private ushort tWindowSize;

        private short tChecksum;
        private ushort tUrgentPointer;

        private uint tOptions;
        private byte[] tData;

        public TCPHeader(byte[] data, int nBytes)
        {
            MemoryStream reader = new MemoryStream(data, 0, nBytes);

            BinaryReader binaryReader = new BinaryReader(reader);

            tSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        }
    }
}
