using LocalTransfer.IService;
using LocalTransfer.Model;
using System.Text.Json;

namespace LocalTransfer.Service
{
    public class TransferService : ITransferService
    {
        private readonly string _historyFile;
        private readonly string _receiveFolder;
        public TransferService()
        {
            _historyFile = Path.Combine(Directory.GetCurrentDirectory(),"TransferHistory.json");
            _receiveFolder = Path.Combine(Directory.GetCurrentDirectory(),"ReceivedFiles");
            Directory.CreateDirectory(_receiveFolder);
            if (!File.Exists(_historyFile))
            {
                File.WriteAllText(_historyFile, "[]");
            }
        }

        public TransferHistory StartTransfer(string sessionId,string sender,string receiver,string fileName,long fileSize,string fileType)
        {
            var history = LoadHistory();
            var transfer = new TransferHistory
            {
                TransferId = Guid.NewGuid().ToString(),
                SessionId = sessionId,
                SenderDevice = sender,
                ReceiverDevice = receiver,
                FileName = fileName,
                FileSize = fileSize,
                FileType = fileType,
                SavePath = Path.Combine(_receiveFolder, fileName),
                Status = "Uploading",
                StartTime = DateTime.Now
            };
            history.Add(transfer);
            SaveHistory(history);
            return transfer;
        }

        public async Task<bool> SaveFileAsync(string transferId,IFormFile file)
        {
            var history = LoadHistory();
            var transfer = history.FirstOrDefault(x => x.TransferId == transferId);
            if (transfer == null)
                return false;
            var path = transfer.SavePath;
            using FileStream stream = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);
            await file.CopyToAsync(stream);
            transfer.Status = "Uploaded";
            SaveHistory(history);
            return true;
        }

        public bool FinishTransfer(string transferId)
        {
            var history = LoadHistory();
            var transfer = history.FirstOrDefault(x => x.TransferId == transferId);
            if (transfer == null)
                return false;
            transfer.Status = "Completed";
            transfer.EndTime = DateTime.Now;
            SaveHistory(history);
            return true;
        }

        public List<TransferHistory> GetHistory()
        {
            return LoadHistory();
        }

        private List<TransferHistory> LoadHistory()
        {
            var json = File.ReadAllText(_historyFile);
            return JsonSerializer.Deserialize<List<TransferHistory>>(json)?? new List<TransferHistory>();
        }

        private void SaveHistory(List<TransferHistory> history)
        {
            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(_historyFile, json);
        }
    }
}
