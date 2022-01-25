﻿using Payments.Public;

namespace Payments.Domain;

public interface IPaymentsRepository
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<IEnumerable<Payment>> GetAllPayments(string authId);
}