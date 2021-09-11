using System;

namespace Tgs.Payment.MaticPay
{
    public record MaticPayWithdrawSettings(
        string MerchantNo,
        string ApiKey,
        Guid OrderNo,
        string AccountName,
        string BankCode,
        string BankBranchName,
        string CardNo);
}