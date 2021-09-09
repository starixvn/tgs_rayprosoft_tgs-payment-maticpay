using System;

namespace Tgs.Payment.MaticPay
{
    public record MaticPayDepositSettings(
        string MerchantNo,
        string ApiKey,
        Guid OrderNo,
        decimal Amount
        );
}