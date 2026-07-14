using LocalTransfer.Model;

namespace LocalTransfer.IService
{
    public interface ITransferService
    {
        TransferHistory StartTransfer( string sessionId, string sender, string receiver, string fileName,   long fileSize,string fileType);
        Task<bool> SaveFileAsync( string transferId, IFormFile file);
        bool FinishTransfer(string transferId);
        List<TransferHistory> GetHistory();
    }
}
}
