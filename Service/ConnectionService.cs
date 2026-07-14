using LocalTransfer.IService;
using LocalTransfer.Model;
using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


namespace LocalTransfer.Service
{
    public class ConnectionService : IConnectionService
    {
        // Memory me active connections store honge
        private static readonly ConcurrentDictionary<string, ConnectionSession> _sessions
            = new ConcurrentDictionary<string, ConnectionSession>();

        public ConnectionResponse VerifyConnection(VerifyConnectionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                return new ConnectionResponse
                {
                    Success = false,
                    Connected = false,
                    Message = "Session Id is required."
                };
            }

            // Same Session already connected
            if (_sessions.ContainsKey(request.SessionId))
            {
                return new ConnectionResponse
                {
                    Success = true,
                    Connected = true,
                    Message = "Already Connected."
                };
            }

            var session = new ConnectionSession
            {
                SessionId = request.SessionId,
                DeviceId = request.DeviceId,
                DeviceName = request.DeviceName,
                DeviceType = request.DeviceType,
                IpAddress = request.IpAddress,
                ConnectedTime = DateTime.Now,
                LastSeen = DateTime.Now,
                IsConnected = true
            };

            _sessions.TryAdd(session.SessionId, session);

            return new ConnectionResponse
            {
                Success = true,
                Connected = true,
                Message = "Device Connected Successfully."
            };
        }

        public ConnectionStatusResponse GetStatus(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out var session))
            {
                return new ConnectionStatusResponse
                {
                    Connected = false,
                    Message = "Session not found."
                };
            }

            return new ConnectionStatusResponse
            {
                Connected = session.IsConnected,
                DeviceName = session.DeviceName,
                DeviceType = session.DeviceType,
                IpAddress = session.IpAddress,
                ConnectedTime = session.ConnectedTime,
                LastSeen = session.LastSeen,
                Message = "Connected"
            };
        }

        public bool Ping(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out var session))
                return false;

            session.LastSeen = DateTime.Now;

            return true;
        }

        public bool Disconnect(string sessionId)
        {
            return _sessions.TryRemove(sessionId, out _);
        }

        public List<DeviceInfo> GetConnectedDevices()
        {
            return _sessions.Values.Select(x => new DeviceInfo
            {
                DeviceId = x.DeviceId,
                DeviceName = x.DeviceName,
                DeviceType = x.DeviceType,
                IpAddress = x.IpAddress,
                Connected = x.IsConnected
            }).ToList();
        }

        public static string GetIPAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                var props = ni.GetIPProperties();

                foreach (var ua in props.UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                        return ua.Address.ToString();
                }
            }

            return "";
        }


        //public NetworkConnectionInfo GetConnectionInfo()
        //{
        //    var info = new NetworkConnectionInfo();

        //    info.DeviceName = Environment.MachineName;
        //    info.HostName = Dns.GetHostName();

        //    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        if (ni.OperationalStatus != OperationalStatus.Up)
        //            continue;

        //        if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
        //            continue;

        //        var props = ni.GetIPProperties();

        //        foreach (var ua in props.UnicastAddresses)
        //        {
        //            if (ua.Address.AddressFamily != AddressFamily.InterNetwork)
        //                continue;

        //            info.IpAddress = ua.Address.ToString();

        //            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
        //                info.NetworkType = "WiFi";
        //            else if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        //                info.NetworkType = "Ethernet";
        //            else
        //                info.NetworkType = ni.NetworkInterfaceType.ToString();

        //            info.IsConnected = true;
        //            info.Status = "Connected";

        //            return info;
        //        }
        //    }

        //    info.IsConnected = false;
        //    info.Status = "Disconnected";
        //    info.Error = "No active network connection found.";

        //    return info;
        //}


        public NetworkConnectionInfo GetConnectionInfo()
        {
            var info = new NetworkConnectionInfo
            {
                DeviceName = Environment.MachineName,
                HostName = Dns.GetHostName(),
                IsConnected = false,
                Status = "Disconnected",
                Error = "No active network connection found."
            };

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Interface must be active
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                // Ignore Loopback & Tunnel
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    continue;

                // Ignore Virtual Adapters
                string name = (ni.Name + " " + ni.Description).ToLower();

                if (name.Contains("virtual") ||
                    name.Contains("vmware") ||
                    name.Contains("hyper-v") ||
                    name.Contains("hyperv") ||
                    name.Contains("docker") ||
                    name.Contains("bluetooth"))
                    continue;

                var props = ni.GetIPProperties();

                foreach (var ua in props.UnicastAddresses)
                {
                    if (ua.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(ua.Address))
                        continue;

                    info.IpAddress = ua.Address.ToString();

                    switch (ni.NetworkInterfaceType)
                    {
                        case NetworkInterfaceType.Wireless80211:
                            info.NetworkType = "WiFi";
                            break;

                        case NetworkInterfaceType.Ethernet:
                        case NetworkInterfaceType.GigabitEthernet:
                            info.NetworkType = "Ethernet";
                            break;

                        default:
                            info.NetworkType = ni.NetworkInterfaceType.ToString();
                            break;
                    }

                    info.IsConnected = true;
                    info.Status = "Connected";
                    info.Error = "";

                    return info;
                }
            }

            return info;
        }


    }


}
