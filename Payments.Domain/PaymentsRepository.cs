using Dapper;
using Payments.Public;
using Utilities;

namespace Payments.Domain;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly DatabaseConnectionFactory _databaseConnectionFactory;

    public PaymentsRepository(DatabaseConnectionFactory databaseConnectionFactory)
    {
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<Payment> AddPayment(string authId, AddPaymentInput input)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = @"INSERT INTO payment (
                     auth_id,
                     reference_date,
                     type,
                     frequency,
                     value,
                     name,
                     category,
                     details
                     ) 
                 VALUES (
                    @AuthId,
                    @ReferenceDate,
                    @Type,
                    @Frequency,
                    @Value,
                    @Name,
                    @Category,
                    @Details
                    )
                RETURNING id";

        var paymentId = await connection.ExecuteScalarAsync<int>(sql, new
        {
            AuthId = authId,
            input.ReferenceDate,
            input.Type,
            input.Frequency,
            input.Value,
            input.Name,
            input.Category,
            input.Details
        });

        var payment = await GetPaymentById(authId, paymentId);

        return payment;
    }
    
    public async Task<Payment> UpdatePayment(string authId, UpdatePaymentInput input)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = @"UPDATE payment
                    SET reference_date = @ReferenceDate,
                        type = @Type,
                        frequency = @Frequency,
                        value = @Value,
                        name = @Name,
                        category = @Category
                        details = @Details
                    WHERE id = @PaymentId AND auth_id = @AuthId";

        await connection.ExecuteAsync(sql, new
        {
            AuthId = authId,
            input.PaymentId,
            input.ReferenceDate,
            input.Type,
            input.Frequency,
            input.Value,
            input.Name,
            input.Category,
            input.Details
        });

        var payment = await GetPaymentById(authId, input.PaymentId);

        return payment;
    }

    public async Task DeletePayment(string authId, int paymentId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = @"DELETE FROM payment WHERE auth_id = @AuthId AND id = @PaymentId";

        await connection.ExecuteAsync(sql, new
        {
            AuthId = authId,
            PaymentId = paymentId
        });
    }
    
    public async Task<IEnumerable<Payment>> GetAllPayments(string authId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = "SELECT id, auth_id, reference_date, type, frequency, value, name, category, details FROM payment WHERE auth_id = @AuthId";

        var payments =
            (await connection.QueryAsync<PaymentRecord>(sql,
                new { AuthId = authId })).ToList();

        return payments.Select(x => x.ToPayment());
    }

    private async Task<Payment> GetPaymentById(string authId, int paymentId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = "SELECT id, auth_id, reference_date, type, frequency, value, name, category, details FROM payment WHERE id = @PaymentId AND auth_id = @AuthId";

        var payment =
            await connection.QuerySingleOrDefaultAsync<PaymentRecord>(sql,
                new { PaymentId = paymentId, AuthId = authId });

        return payment.ToPayment();
    }
}

internal record PaymentRecord
{
    public PaymentRecord(int id,
        string auth_id,
        DateTime reference_date,
        PaymentType type,
        PaymentFrequency frequency,
        decimal value,
        string name,
        string category,
        string details)
    {
        this.id = id;
        this.auth_id = auth_id;
        this.reference_date = reference_date;
        this.type = type;
        this.frequency = frequency;
        this.value = value;
        this.name = name;
        this.category = category;
        this.details = details;
    }

    public int id { get; init; }
    public string auth_id { get; init; }
    public DateTime reference_date { get; init; }
    public PaymentType type { get; init; }
    public PaymentFrequency frequency { get; init; }
    public decimal value { get; init; }
    public string name { get; init; }
    public string category { get; init; }
    public string details { get; init; }

    public Payment ToPayment() =>
        new Payment(id, auth_id, reference_date, type, frequency, name, category, details, value);
}