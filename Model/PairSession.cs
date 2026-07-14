namespace LocalTransfer.Model
{
    public class PairSession
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        public string Token { get; set; } = "";

        public string IpAddress { get; set; } = "";

        public string DeviceName { get; set; } = "";

        public string HostName { get; set; } = "";

        public string NetworkType { get; set; } = "";

        public DateTime CreatedOn { get; set; }

        public DateTime ExpireOn { get; set; }

        public bool Connected { get; set; }
    }
}
