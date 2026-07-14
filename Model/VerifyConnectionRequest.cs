using System.ComponentModel.DataAnnotations;

namespace LocalTransfer.Model
{
    public class VerifyConnectionRequest
    {
        // Pairing Session Id
        [Required]
        public string SessionId { get; set; } = string.Empty;

        // Unique Device Id
        [Required]
        public string DeviceId { get; set; } = string.Empty;

        // Device Name
        [Required]
        public string DeviceName { get; set; } = string.Empty;

        // Android / Windows / iPhone
        [Required]
        public string DeviceType { get; set; } = string.Empty;

        // Device IP Address
        [Required]
        public string IpAddress { get; set; } = string.Empty;

        // WiFi / Hotspot / Bluetooth / LAN
        public string ConnectionType { get; set; } = "WiFi";

        // Optional App Version
        public string? AppVersion { get; set; }

        // Optional Device OS Version
        public string? OsVersion { get; set; }
    }
}
