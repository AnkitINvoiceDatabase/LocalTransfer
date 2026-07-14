namespace LocalTransfer.Model
{
    public class ConnectionStatusResponse
    {
        // Connection Status
        public bool Connected { get; set; }

        // Session Id
        public string? SessionId { get; set; }

        // Device Id
        public string? DeviceId { get; set; }

        // Device Name
        public string? DeviceName { get; set; }

        // Device Type
        public string? DeviceType { get; set; }

        // Device IP Address
        public string? IpAddress { get; set; }

        // WiFi / Hotspot / Bluetooth / LAN
        public string? ConnectionType { get; set; }

        // Same Network Status
        public bool SameNetwork { get; set; }

        // Connected Time
        public DateTime ConnectedTime { get; set; }

        // Last Heartbeat Time
        public DateTime LastSeen { get; set; }

        // Session Expiry
        public DateTime ExpireAt { get; set; }

        // Transfer Running
        public bool IsTransferring { get; set; }

        // Current Transfer Id
        public string? TransferId { get; set; }

        // Response Message
        public string Message { get; set; } = string.Empty;
    }
}
