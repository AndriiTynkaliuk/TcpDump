using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpDump
{
    /// <summary>
    /// Represents an IP header fields
    /// </summary>
    public class IPHeader : IHeader
    {
        private byte iVersionAndHeaderLength;     // 8 bits for Version and Header Length
        private byte iTypeOfService;              // 8 bits for Type of Service or DiffServ

        private ushort iTotalLength;              // 16 bits for Total Length of the datagram
        private ushort iIdentifier;               // 16 bits for Identifier

        private ushort iFlagsAndOffset;           // 3 bits for Flags + 13 bits for Fragment offset

        private byte iTTL;                        // 8 bits for Time to Live
        private byte iProtocol;

        private short iChecksum;                  // 16 bits for Header checksum

        private uint iSourceAddress;              // 32 bits for Source IPv-4 Address
        private uint iDestinationAddress;         // 32 bits for Destination IPv-4 Address

        private byte iHeaderLength;
        private byte[] iIPData = new byte[4096];  // Data carried by the datagram

        /// <summary>
        /// Creates a new instance of IP header class and parses the IP packet.
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <param name="nButes"></param>
        public IPHeader(byte[] receivedBytes, int nBytes)
        {
            // Receives 
            MemoryStream reader = new MemoryStream(receivedBytes, 0, nBytes);
            BinaryReader binaryReader = new BinaryReader(reader);

            iVersionAndHeaderLength = binaryReader.ReadByte();
            iTypeOfService = binaryReader.ReadByte();

            iTotalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            iIdentifier = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            iFlagsAndOffset = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            iTTL = binaryReader.ReadByte();
            iProtocol = binaryReader.ReadByte();

            iChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            iSourceAddress = (uint)binaryReader.ReadInt32();
            iDestinationAddress = (uint)binaryReader.ReadInt32();

            iHeaderLength = iVersionAndHeaderLength;

            // calculates IP header length. Min value is 5 (5 * 32 bit = 160 bit or 20 bytes), Max value (15 * 32 = 480 bit or 60 bytes)
            iHeaderLength <<= 4;
            iHeaderLength >>= 4;
            iHeaderLength *= 4;

            // Calculates the version of IP packet (IPv4 if equal to 4 or IPv6 if equal to 6)
            iVersionAndHeaderLength >>= 4;

            // Copy the data carried by datagram into another array. 
            Array.Copy(receivedBytes, iHeaderLength, iIPData, 0, iTotalLength - iHeaderLength);
        }

        /// <summary>
        /// The first header field in an IP packet is the four-bit version field. For IPv4, this is always equal to 4.
        /// </summary>
        public string IPVersion
        {
            get
            {
                if (iVersionAndHeaderLength == 4)
                    return "IPv4";
                else if (iVersionAndHeaderLength == 6)
                    return "IPv6";
                else
                    return "Unknown";
            }
        }

        /// <summary>
        /// Internet Header Length (IHL)
        /// </summary>
        public string HeaderLength
        {
            get
            {
                return iHeaderLength + " bytes";
            }
        }

        /// <summary>
        /// Total length of the data carried by the datagram
        /// </summary>
        public ushort MessageLength
        {
            get
            {
                return (ushort)(iTotalLength - iHeaderLength);
            }
        }

        // 0 : Best Effor
        // 1 : Priority
        // 2 : Immediate
        // 3 : Flash - mainly used for voice signaling
        // 4 : Flash Override
        // 5 : Critical - mainly used for voice RTP
        // 6 : Internetwork Control
        // 7 : Network Control
        /// <summary>
        /// Differentiated Services Code Point (DSCP) + Explicit Congestion Notification (ECN).
        /// ECN is an optional feature that is only used when both endpoints support it and are willing to use it
        /// </summary>
        public string TypeOfService
        {
            get
            {
                return string.Format("0x{0:x2} ({1})", iTypeOfService, 
                    iTypeOfService);
            }
        }

        /// <summary>
        /// Total Length. 
        /// This 16-bit field defines the entire packet size in bytes, including header and data
        /// </summary>
        public ushort TotalLength
        {
            get
            {
                return iTotalLength;
            }
        }

        /// <summary>
        /// Identification.
        /// This field is an identification field and is primarily used for uniquely identifying the group of fragments of a single IP datagram.
        /// </summary>
        public ushort Identifier
        {
            get
            {
                return iIdentifier;
            }
        }

        // bit 0: Reserved; must be zero
        // bit 1: Don't Fragment (DF)
        // bit 2: More Fragments (MF)
        /// <summary>
        /// Flags.
        /// A three-bit field follows and is used to control or identify fragments.
        /// </summary>
        public string Flag
        {
            get
            {
                if ((iFlagsAndOffset >> 13) == 2)
                    return "Don't fragment";
                else if ((iFlagsAndOffset >> 13) == 1)
                    return "Has fragments";
                else if ((iFlagsAndOffset >> 13) == 0)
                    return "No or last fragment";
                else
                    return (iFlagsAndOffset >> 13).ToString();
            }
        }

        /// <summary>
        /// Fragment Offset.
        /// The fragment offset field is measured in units of eight-byte blocks.
        /// </summary>
        public ushort Offset
        {
            get
            {
                ushort result = iFlagsAndOffset;
                result <<= 3;
                result >>= 3;
                return result;
            }
        }

        /// <summary>
        /// Time To Live (TTL).
        /// This field limits a datagram's lifetime.
        /// </summary>
        public string TTL
        {
            get
            {
                return iTTL.ToString();
            }
        }

        /// <summary>
        /// Protocol.
        /// This field defines the protocol used in the data portion of the IP datagram.
        /// </summary>
        public ProtocolType Protocol
        {
            get
            {
                if (iProtocol == 1)
                    return ProtocolType.Icmp;
                else if (iProtocol == 6)
                    return ProtocolType.Tcp;
                else if (iProtocol == 17)
                    return ProtocolType.Udp;
                else
                    return ProtocolType.Unknown;
            }
        }

        /// <summary>
        /// Header Checksum.
        /// The 16-bit checksum field is used for error-checking of the header.
        /// </summary>
        public string Checksum
        {
            get
            {
                return string.Format("0x{0:x2}", iChecksum);
            }
        }

        /// <summary>
        /// ource address
        /// This field is the IPv4 address of the sender of the packet.
        /// </summary>
        public string SourceIP
        {
            get
            {
                return new IPAddress(iSourceAddress).ToString();
            }
        }

        /// <summary>
        /// Destination address
        /// This field is the IPv4 address of the receiver of the packet.
        /// </summary>
        public string DestIP
        {
            get
            {
                return new IPAddress(iDestinationAddress).ToString();
            }
        }

        /// <summary>
        /// The data portion of the packet is not included in the packet checksum.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return iIPData;
            }
        }

        public string SourcePort => throw new NotImplementedException();

        public string DestPort => throw new NotImplementedException();

        public void Print(IHeader header)
        {
            IPHeader newIPHeader = header as IPHeader;
            Console.WriteLine(string.Format("Protocol\tSource IP\tDestination IP\tTotal Length"));
            Console.WriteLine(string.Format("  {0}\t{1}\t{2}\t{3}", newIPHeader.Protocol,
                newIPHeader.SourceIP, newIPHeader.DestIP, newIPHeader.TotalLength));
        }
    }
}
