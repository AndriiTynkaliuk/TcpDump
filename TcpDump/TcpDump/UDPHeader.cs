using System;
using System.IO;
using System.Net;

namespace TcpDump
{
    public class UDPHeader : IHeader
    {
        //UDP header fields
        private ushort usSourcePort;            //Sixteen bits for the source port number        
        private ushort usDestinationPort;       //Sixteen bits for the destination port number
        private ushort usLength;                //Length of the UDP header
        private short sChecksum;                //Sixteen bits for the checksum
                                                //(checksum can be negative so taken as short)              
        //End UDP header fields

        private byte[] byUDPData = new byte[4096];  //Data carried by the UDP packet

        public UDPHeader(byte [] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //The first sixteen bits contain the source port
            usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen bits contain the destination port
            usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen bits contain the length of the UDP packet
            usLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen bits contain the checksum
            sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());            

            //Copy the data carried by the UDP packet into the data buffer
            Array.Copy(byBuffer, 
                       8,               //The UDP header is of 8 bytes so we start copying after it
                       byUDPData, 
                       0, 
                       nReceived - 8);
        }

        public string SourcePort
        {
            get
            {
                return usSourcePort.ToString();
            }
        }

        public string DestPort
        {
            get
            {
                return usDestinationPort.ToString();
            }
        }

        public string Length
        {
            get
            {
                return usLength.ToString ();
            }
        }

        public string Checksum
        {
            get
            {
                //Return the checksum in hexadecimal format
                return string.Format("0x{0:x2}", sChecksum);
            }
        }

        public byte[] Data
        {
            get
            {
                return byUDPData;
            }
        }

        public void Print(IHeader header)
        {
            UDPHeader newHeader = header as UDPHeader;
            Console.WriteLine(string.Format("Soucre Port\tDestination Port\tLength"));
            Console.WriteLine(string.Format("    {0}\t{1}\t{2}",
                newHeader.SourcePort, newHeader.DestPort, newHeader.Length));
        }
    }
}