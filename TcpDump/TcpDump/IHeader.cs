namespace TcpDump
{
    public interface IHeader
    {
        void Print(IHeader header);

        string SourcePort { get; }
        string DestPort { get; }
    }
}
