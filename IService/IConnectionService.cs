using LocalTransfer.Model;

namespace LocalTransfer.IService
{
    public interface IConnectionService
    {
        // Verify a new connection
        ConnectionResponse VerifyConnection(VerifyConnectionRequest request);

        // Get current connection status
        ConnectionStatusResponse GetStatus(string sessionId);

        // Update heartbeat (Ping)
        bool Ping(string sessionId);

        // Disconnect a device
        bool Disconnect(string sessionId);

        // Get all connected devices
        List<DeviceInfo> GetConnectedDevices();

        NetworkConnectionInfo GetConnectionInfo();


    }
}
