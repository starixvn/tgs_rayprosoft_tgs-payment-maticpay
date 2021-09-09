using System;
using System.Threading.Tasks;

namespace Tgs.Payment.MaticPay
{
    public interface ITgsRepository
    {
        ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id);

        ValueTask<WithdrawFormInfo> GetWithdrawFormByIdAsync(Guid id);

        ValueTask SaveDepositReturnAsync(Guid id, MaticPayDepositReturn depositReturn);

        ValueTask SaveWithdrawReturnAsync(Guid id, MaticPayWithdrawReturn withdrawReturn);
    }
}