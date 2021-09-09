using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class GetWithdrawFormQueryService : IGetService<GetWithdrawFormQuery, WithdrawFormInfo>
    {
        private readonly ITgsRepository _tgsRepository;

        public GetWithdrawFormQueryService(ITgsRepository tgsRepository)
        {
            _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
        }

        public ValueTask<WithdrawFormInfo> GetAsync(GetWithdrawFormQuery query, CancellationToken cancellationToken = default)
        {
            var withdrawFormId = new Guid(query.OrderNo);

            return _tgsRepository.GetWithdrawFormByIdAsync(withdrawFormId);
        }
    }
}