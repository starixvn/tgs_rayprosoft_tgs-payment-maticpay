using System;
using Tgs.Payment.Clients;

namespace Tgs.Payment.MaticPay
{
    public record DepositFormInfo(
       Guid Id,
       string PlayerCode,
       Guid BrandId,
       Guid BrandPaymentId,
       decimal Amount,
       PaymentFlowState Status,
       MaticPayDepositApplyInfo? ApplyInfo,
       string? PaymentReturn = null);
}