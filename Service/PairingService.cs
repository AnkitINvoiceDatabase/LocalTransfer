using System.Collections.Concurrent;
using LocalTransfer.IService;

namespace LocalTransfer.Service
{
    public class PairingService: IPairingService
    {
        private readonly ConcurrentDictionary<string, DateTime> _tokens = new();

        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString("N");

            _tokens[token] = DateTime.UtcNow.AddMinutes(5);

            return token;
        }

        public bool ValidateToken(string token)
        {
            if (!_tokens.TryGetValue(token, out var expiry))
                return false;

            if (expiry < DateTime.UtcNow)
            {
                _tokens.TryRemove(token, out _);
                return false;
            }

            return true;
        }

        public void RemoveToken(string token)
        {
            _tokens.TryRemove(token, out _);
        }

    }
}
