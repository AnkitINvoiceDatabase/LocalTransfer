namespace LocalTransfer.Model
{
    public class TransferHistory
    {
        public string TransferId { get; set; } = Guid.NewGuid().ToString();

        

        public string SenderDevice { get; set; } = "";

        public string ReceiverDevice { get; set; } = "";

        public string FileName { get; set; } = "";

        public long FileSize { get; set; }

        public string FileType { get; set; } = "";

        public string SavePath { get; set; } = "";

        public string Status { get; set; } = "Pending";

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
