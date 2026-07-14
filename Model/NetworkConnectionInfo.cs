namespace LocalTransfer.Model
{
    public class NetworkConnectionInfo
    {
        public string IpAddress { get; set; } = "";
        public int Port { get; set; }
        public string HostName { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string NetworkType { get; set; } = "";
        public bool IsConnected { get; set; }
        public string Status { get; set; } = "";
        public string Error { get; set; } = "";
    }
}
