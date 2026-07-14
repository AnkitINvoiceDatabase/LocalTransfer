namespace LocalTransfer.IService
{
    public interface IPairingService
    {
        string GenerateToken();

        bool ValidateToken(string token);

        void RemoveToken(string token);
    }
}
