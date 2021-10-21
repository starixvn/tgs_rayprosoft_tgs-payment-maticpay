using System;
using Tgs.Payment.Clients;

namespace Tgs.Payment.MaticPay.Services
{
    public record SaveWithdrawReturnCommand(
        Guid FormId,
        PaymentFlowState State,
        MaticPayWithdrawReturn WithdrawReturn,
        decimal? ActualAmount);
}