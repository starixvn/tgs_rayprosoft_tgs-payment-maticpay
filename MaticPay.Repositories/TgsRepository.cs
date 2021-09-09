using System;
using System.Threading.Tasks;
using Tgs.Payment.MaticPay.Repositories.Entities;

namespace Tgs.Payment.MaticPay.Repositories
{
    internal class TgsRepository : ITgsRepository
    {
        private readonly TgsContext _dbContext;
        private readonly MaticPaySerializer _serializer;

        public TgsRepository(TgsContext dbContext, MaticPaySerializer serializer)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id)
        {
            var data = await _dbContext.DepositForms.FindAsync(id).ConfigureAwait(false);

            return new DepositFormInfo(
               data.Id,
               data.PlayerCode,
               data.BrandId,
               data.BrandPaymentId,
               data.Amount,
               data.Status,
               _serializer.Deserialize<MaticPayDepositApplyInfo>(data.ApplyInfo!),
               data.PaymentReturn);
        }

        public async ValueTask<WithdrawFormInfo> GetWithdrawFormByIdAsync(Guid id)
        {
            var data = await _dbContext.WithdrawForms.FindAsync(id).ConfigureAwait(false);

            return new WithdrawFormInfo(
                data.Id,
                data.UserId!,
                data.BrandId,
                data.BrandPaymentId,
                data.Amount,
                data.Status,
                data.BankInfoId,
                data.BankInfo,
                _serializer.Deserialize<MaticPayWithdrawApplyInfo>(data.ApplyInfo!),
                data.PaymentReturn);
        }

        public async ValueTask SaveDepositReturnAsync(Guid id, MaticPayDepositReturn depositReturn)
        {
            var form = await _dbContext.DepositForms
                .FindAsync(id)
                .ConfigureAwait(false);

            form.Status = 1000;
            form.ActualAmount = decimal.Parse(depositReturn.OrderAmt);
            form.PaymentReturn = _serializer.Serialize(depositReturn);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async ValueTask SaveWithdrawReturnAsync(Guid id, MaticPayWithdrawReturn withdrawReturn)
        {
            var form = await _dbContext.WithdrawForms
                .FindAsync(id)
                .ConfigureAwait(false);

            form.PaymentReturn = _serializer.Serialize(withdrawReturn);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}