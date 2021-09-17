using System;

namespace Tgs.Payment.MaticPay
{
    public record MaticPayWithdrawSettings(
        string MerchantNo,
        string ApiKey,
        Guid OrderNo,
        decimal Amount,
        string AccountName,
        string BankCode,
        string BankBranchName,
        string CardNo);
}