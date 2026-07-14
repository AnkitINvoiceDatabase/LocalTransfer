namespace LocalTransfer.Model
{
    public class DeviceInfo
    {
        // Unique Device Id
        public string DeviceId { get; set; } = string.Empty;

        // Device Name
        public string DeviceName { get; set; } = string.Empty;

        // Android / Windows / iPhone
        public string DeviceType { get; set; } = string.Empty;

        // Device IP Address
        public string IpAddress { get; set; } = string.Empty;

        // WiFi / Hotspot / Bluetooth / LAN
        public string ConnectionType { get; set; } = "WiFi";

        // Connection Status
        public bool Connected { get; set; }

        // Same Network
        public bool SameNetwork { get; set; } = true;

        // Connected Time
        public DateTime ConnectedTime { get; set; }

        // Last Heartbeat
        public DateTime LastSeen { get; set; }

        // Transfer Running
        public bool IsTransferring { get; set; }

        // Current Transfer Id
        public string? TransferId { get; set; }

        // Device Icon (Optional)
        public string Icon { get; set; } = "device";
    }
}
