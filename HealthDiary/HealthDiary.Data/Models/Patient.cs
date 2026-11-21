namespace HealthDiary.Data.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public double WeightKg { get; set; }
        public double ShoeSize { get; set; }
        public string DiagnosisNotes { get; set; } = string.Empty;

        // Навігаційна властивість (1:N до GaitAnalysis)
        public ICollection<GaitAnalysis> GaitAnalyses { get; set; } = new List<GaitAnalysis>();
    }
}