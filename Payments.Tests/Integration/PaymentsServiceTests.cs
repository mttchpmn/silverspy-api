using System;
using System.Threading.Tasks;
using Payments.Domain;
using Payments.Public;
using Test.Utilities;
using Xunit;

namespace Payments.Tests.Integration;

[Collection("Test database")]
public class PaymentsServiceTests
{
    private readonly PaymentsService _paymentsService;
    private readonly string _authId;

    public PaymentsServiceTests()
    {
        var paymentsRepository = new PaymentsRepository(TestDatabaseConnectionFactory.GetFactory());
        _paymentsService = new PaymentsService(paymentsRepository);
        _authId = "test|abc123456789";
    }

    public class AddPayment : PaymentsServiceTests
    {
        [Fact]
        public async Task Can_add_and_retrieve_a_payment()
        {
            var input = GetAddPaymentInput();

            var newPayment = await _paymentsService.AddPayment(_authId, input);

            Assert.Equal(input.Category, newPayment.Category);
            Assert.Equal(input.ReferenceDate, newPayment.ReferenceDate);
            Assert.Equal(input.Type, newPayment.Type);
            Assert.Equal(input.Frequency, newPayment.Frequency);
            Assert.Equal(input.Name, newPayment.Name);
            Assert.Equal(input.Category, newPayment.Category);
            Assert.Equal(input.Details, newPayment.Details);
            Assert.Equal(input.Value, newPayment.Value);
        }

        private static AddPaymentInput GetAddPaymentInput()
        {
            return new AddPaymentInput(
                DateTime.MaxValue,
                PaymentType.Outgoing,
                PaymentFrequency.Fortnightly,
                "Rent",
                "Bills",
                "2 weeks rent",
                1190M);
        }

        [Fact]
        public async Task Can_update_a_payment()
        {
            var payment = await _paymentsService.AddPayment(_authId, GetAddPaymentInput());

            var input = new UpdatePaymentInput(payment.Id, DateTime.UnixEpoch, PaymentType.Incoming,
                PaymentFrequency.Monthly, "New payment", "Costs", "For things", 999M);

            var newPayment = await _paymentsService.UpdatePayment(_authId, input);

            Assert.Equal(input.Category, newPayment.Category);
            Assert.Equal(input.ReferenceDate, newPayment.ReferenceDate);
            Assert.Equal(input.Type, newPayment.Type);
            Assert.Equal(input.Frequency, newPayment.Frequency);
            Assert.Equal(input.Name, newPayment.Name);
            Assert.Equal(input.Category, newPayment.Category);
            Assert.Equal(input.Details, newPayment.Details);
            Assert.Equal(input.Value, newPayment.Value);
        }

        [Fact]
        public async Task Can_delete_a_payment()
        {
            var payment = await _paymentsService.AddPayment(_authId, GetAddPaymentInput());
            await _paymentsService.DeletePayment(_authId, payment.Id);

            var result = await _paymentsService.GetPaymentById(_authId, payment.Id);
            
            Assert.Null(result);
        }
    }
}