using System;

namespace Tgs.Payment.MaticPay
{
    public record WithdrawFormInfo(
        Guid Id,
        string PlayerCode,
        Guid BrandId,
        Guid? BrandPaymentId,
        decimal Amount,
        int Status,
        Guid BankInfoId,
        string? BankInfo,
        MaticPayWithdrawApplyInfo? ApplyInfo,
        string? PaymentReturn = null);
}