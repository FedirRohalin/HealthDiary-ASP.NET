namespace HealthDiary.Data.Models
{
    public class InsoleOrder
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string PrinterModel { get; set; } = "Creality Ender 3";
        public decimal TotalCost { get; set; }
        public bool IsCompleted { get; set; }
        public string GCodeFilePath { get; set; } = string.Empty;

        // Зовнішній ключ та навігаційна властивість (1:N від GaitAnalysis)
        public int GaitAnalysisId { get; set; }
        public GaitAnalysis? GaitAnalysis { get; set; }

        // Навігаційна властивість (M:N до Material)
        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}