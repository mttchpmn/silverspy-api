namespace SilverSpy.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string DateProcessed { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}