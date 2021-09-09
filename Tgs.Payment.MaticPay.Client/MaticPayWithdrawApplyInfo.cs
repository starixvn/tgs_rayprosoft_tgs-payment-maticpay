namespace Tgs.Payment.MaticPay
{
    public record MaticPayWithdrawApplyInfo(
      MaticPayWithdrawSettings Settings,
        MaticPayWithdrawResponse Response);
}