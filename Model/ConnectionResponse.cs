namespace LocalTransfer.Model
{
    public class ConnectionResponse
    {
        // Request Success or Failed
        public bool Success { get; set; }

        // Device Connected
        public bool Connected { get; set; }

        // Session Id
        public string? SessionId { get; set; }

        // Device Id
        public string? DeviceId { get; set; }

        // Device Name
        public string? DeviceName { get; set; }

        // Connection Type (WiFi, Hotspot, Bluetooth)
        public string? ConnectionType { get; set; }

        // Same Network Status
        public bool SameNetwork { get; set; }

        // Server Time
        public DateTime ServerTime { get; set; } = DateTime.Now;

        // Response Message
        public string Message { get; set; } = string.Empty;
    }
}
