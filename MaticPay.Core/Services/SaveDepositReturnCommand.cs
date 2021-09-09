using System;

namespace Tgs.Payment.MaticPay.Services
{
    public record SaveDepositReturnCommand(Guid FormId, MaticPayDepositReturn DepositReturn);
}