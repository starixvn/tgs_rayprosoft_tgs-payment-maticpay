namespace Tgs.Payment.MaticPay.Services
{
    public record SaveWithdrawReturnCommand(
        Guid FormId,
        MaticPayWithdrawReturn WithdrawReturn,
        decimal? ActualAmount);
}
