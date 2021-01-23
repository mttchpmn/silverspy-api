namespace SilverSpy.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Frequency { get; set; }
        public int Day { get; set; }
        public decimal Amount { get; set; }
    }
}