using System;

namespace Tgs.Payment.MaticPay
{
    public record DepositFormInfo(
       Guid Id,
       string PlayerCode,
       Guid BrandId,
       Guid BrandPaymentId,
       decimal Amount,
       int Status,
       MaticPayDepositApplyInfo? ApplyInfo,
       string? PaymentReturn = null);
}