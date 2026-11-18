namespace HealthDiary.Data.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Напр. "TPU 95A"
        public string Color { get; set; } = string.Empty;
        public double ShoreHardness { get; set; }
        public decimal PricePerGram { get; set; }
        public double ExtrusionTemp { get; set; }

        // Навігаційна властивість (M:N до InsoleOrder)
        public ICollection<InsoleOrder> InsoleOrders { get; set; } = new List<InsoleOrder>();
    }
}   