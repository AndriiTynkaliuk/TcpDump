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
    public class TCPHeader : IHeader
    {
        private ushort tSourcePort;
        private ushort tDestPort;

        private uint tSequenceNum;
        private uint tAcknowlegmentNum;

        private ushort tHeaderLenReservFlag;
        private ushort tWindowSize;

        private short tChecksum;
        private ushort tUrgentPointer;

        private byte tHeaderLength;
        private ushort tMessageLength;
        private byte[] tTCPData = new byte[4096];

        public TCPHeader(byte[] data, int nBytes)
        {
            try
            {
                MemoryStream reader = new MemoryStream(data, 0, nBytes);

                BinaryReader binaryReader = new BinaryReader(reader);

                tSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                tDestPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                tSequenceNum = (uint)binaryReader.ReadInt32();
                tAcknowlegmentNum = (uint)binaryReader.ReadInt32();

                tHeaderLenReservFlag = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                tWindowSize = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                tChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                tUrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                tHeaderLength = (byte)(tHeaderLenReservFlag >> 12);
                tHeaderLength *= 4;

                tMessageLength = (ushort)(nBytes - tHeaderLength);

                Array.Copy(data, tHeaderLength, tTCPData, 0, nBytes - tHeaderLength);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Source port (16 bits).
        /// Identifies the sending port
        /// </summary>
        public string SourcePort
        {
            get
            {
                return tSourcePort.ToString();
            }
        }

        /// <summary>
        /// Destination port (16 bits).
        /// Identifies the receiving port
        /// </summary>
        public string DestPort
        {
            get
            {
                return tDestPort.ToString();
            }
        }

        /// <summary>
        /// Sequence number (32 bits).
        /// </summary>
        public string SequenceNumber
        {
            get
            {
                return tSequenceNum.ToString();
            }
        }

        /// <summary>
        /// Acknowledgment number (32 bits).
        /// If the ACK flag is set then the value of this field is the next sequence number that the sender is expecting.
        /// </summary>
        public string AcknowledgmentNum
        {
            get
            {
                //If the ACK flag is set then only we have a valid value in
                //the acknowlegement field, so check for it beore returning 
                //anything
                if ((tHeaderLenReservFlag & 0x10) != 0)
                {
                    return tAcknowlegmentNum.ToString();
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// Specifies the size of the TCP header in 32-bit words.
        /// </summary>
        public string HeaderLength
        {
            get
            {
                return tHeaderLength.ToString();
            }
        }

        /// <summary>
        /// Reserved (3 bits).
        /// For future use and should be set to zero
        /// </summary>
        public string ReservedBytes
        {
            get
            {
                var reservedBytes = tHeaderLenReservFlag;
                reservedBytes >>= 0;
                reservedBytes <<= 13;
                reservedBytes >>= 13;
                return reservedBytes.ToString();
            }
        }

        /// <summary>
        /// Flags (9 bits) (aka Control bits)
        /// </summary>
        public string Flags
        {
            get
            {
                //The last six bits of the data offset and flags contain the
                //control bits

                //First we extract the flags
                int nFlags = tHeaderLenReservFlag & 0x3F;

                string strFlags = string.Format("0x{0:x2} (", nFlags);

                //Now we start looking whether individual bits are set or not
                if ((nFlags & 0x01) != 0)
                {
                    strFlags += "FIN, ";
                }
                if ((nFlags & 0x02) != 0)
                {
                    strFlags += "SYN, ";
                }
                if ((nFlags & 0x04) != 0)
                {
                    strFlags += "RST, ";
                }
                if ((nFlags & 0x08) != 0)
                {
                    strFlags += "PSH, ";
                }
                if ((nFlags & 0x10) != 0)
                {
                    strFlags += "ACK, ";
                }
                if ((nFlags & 0x20) != 0)
                {
                    strFlags += "URG";
                }
                strFlags += ")";

                if (strFlags.Contains("()"))
                {
                    strFlags = strFlags.Remove(strFlags.Length - 3);
                }
                else if (strFlags.Contains(", )"))
                {
                    strFlags = strFlags.Remove(strFlags.Length - 3, 2);
                }

                return strFlags;
            }
        }

        /// <summary>
        /// Window size (16 bits).
        /// The size of the receive window, which specifies the number of window size units 
        /// that the sender of this segment is currently willing to receive
        /// </summary>
        public string WindowSize
        {
            get
            {
                return tWindowSize.ToString();
            }
        }

        /// <summary>
        /// Urgent pointer (16 bits).
        /// If the URG flag is set, then this 16-bit field is an offset from the sequence number indicating the last urgent data byte
        /// </summary>
        public string UrgentPointer
        {
            get
            {
                //If the URG flag is set then only we have a valid value in
                //the urgent pointer field, so check for it beore returning 
                //anything
                if ((tHeaderLenReservFlag & 0x20) != 0)
                {
                    return tUrgentPointer.ToString();
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// Checksum (16 bits).
        /// The 16-bit checksum field is used for error-checking of the header and data
        /// </summary>
        public string Checksum
        {
            get
            {
                //Return the checksum in hexadecimal format
                return string.Format("0x{0:x2}", tChecksum);
            }
        }

        /// <summary>
        /// Data carried by the TCP packet
        /// </summary>
        public byte[] Data
        {
            get
            {
                return tTCPData;
            }
        }

        

        /// <summary>
        /// Prints basic header fields
        /// </summary>
        /// <param name="header"></param>
        public void Print(IHeader header)
        {
            TCPHeader newHeader = header as TCPHeader;
            Console.WriteLine(string.Format("Window Size\tSource Port\tDestination Port\tUrgent Pointer"));
            Console.WriteLine(string.Format("    {0}\t{1}\t{2}\t{3}",
                newHeader.WindowSize, newHeader.SourcePort, newHeader.DestPort, newHeader.UrgentPointer));
        }
    }
}
