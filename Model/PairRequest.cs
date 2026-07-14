namespace LocalTransfer.Model
{
    public class PairRequest
    {
        //public string SessionId { get; set; }
        public string Token { get; set; } = string.Empty;

        public string DeviceId { get; set; } = string.Empty;

        public string DeviceName { get; set; } = string.Empty;

        public string DeviceType { get; set; } = string.Empty;

        public string IpAddress { get; set; } = string.Empty;
    }
}
