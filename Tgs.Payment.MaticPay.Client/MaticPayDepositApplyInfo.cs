namespace Tgs.Payment.MaticPay
{
    public record MaticPayDepositApplyInfo(
        MaticPayDepositSettings Settings,
        MaticPayDepositResponse Response);
}