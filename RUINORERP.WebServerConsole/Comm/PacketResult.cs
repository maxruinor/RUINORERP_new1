namespace RUINORERP.WebServerConsole.Comm
{
    public class PacketResult
    {
        public PacketResult() { Flag = false; }
        public bool Flag { get; set; }
        public string Msg { get; set; } = "";
        public string Data { get; set; } = "";
    }
}