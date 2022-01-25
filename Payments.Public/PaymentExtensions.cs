namespace Payments.Public;

public static class PaymentExtensions
{
   public static IEnumerable<DateTime> GenerateDates(this Payment payment, DateTime startDate, DateTime endDate)
   {
      var result = new List<DateTime>();
      var referenceDate = payment.ReferenceDate;

      while (referenceDate < endDate)
      {
         referenceDate = payment.Frequency switch
         {
            PaymentFrequency.Weekly => referenceDate.AddDays(7),
            PaymentFrequency.Fortnightly => referenceDate.AddDays(14),
            PaymentFrequency.Monthly => referenceDate.AddMonths(1),
            PaymentFrequency.Yearly => referenceDate.AddYears(1),
            _ => referenceDate
         };
         
         if (referenceDate >= startDate)
            result.Add(referenceDate);
      }

      return result;
   }
}