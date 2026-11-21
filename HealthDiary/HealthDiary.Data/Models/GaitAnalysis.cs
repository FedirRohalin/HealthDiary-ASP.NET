namespace HealthDiary.Data.Models
{
    public class GaitAnalysis
    {
        public int Id { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string VideoFilePath { get; set; } = string.Empty;
        public double PronationAngle { get; set; }
        public bool NeedsCorrection { get; set; }
        public string CVProcessingLog { get; set; } = string.Empty;

        // Зовнішні ключі та навігаційні властивості
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        // Навігаційна властивість (1:N до InsoleOrder)
        public ICollection<InsoleOrder> InsoleOrders { get; set; } = new List<InsoleOrder>();
    }
}       