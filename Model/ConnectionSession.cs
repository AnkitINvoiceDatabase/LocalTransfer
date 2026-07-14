namespace LocalTransfer.Model
{
    public class ConnectionSession
    {
        // Unique Session Id
        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        // Device Unique Id
        public string DeviceId { get; set; } = string.Empty;

        // Device Name
        public string DeviceName { get; set; } = string.Empty;

        // Android / Windows / iPhone
        public string DeviceType { get; set; } = string.Empty;

        // Device IP Address
        public string IpAddress { get; set; } = string.Empty;

        // Connected Time
        public DateTime ConnectedTime { get; set; } = DateTime.Now;

        // Last Heartbeat Time
        public DateTime LastSeen { get; set; } = DateTime.Now;

        // Connection Status
        public bool IsConnected { get; set; } = true;

        // Same WiFi / Hotspot
        public bool IsSameNetwork { get; set; } = true;

        // Session Expiry
        public DateTime ExpireAt { get; set; } = DateTime.Now.AddMinutes(30);

        // Transfer Status
        public bool IsTransferring { get; set; } = false;

        // Current Transfer Id
        public string? TransferId { get; set; }
    }
}
