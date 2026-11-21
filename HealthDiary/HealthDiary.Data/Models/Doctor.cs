namespace HealthDiary.Data.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string ContactEmail { get; set; } = string.Empty;

        // Навігаційна властивість (1:N до GaitAnalysis)
        public ICollection<GaitAnalysis> GaitAnalyses { get; set; } = new List<GaitAnalysis>();
    }
}