using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.Payment.Clients;
using TGS.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class GetWithdrawFormQueryService : IGetService<GetWithdrawFormQuery, WithdrawFormInfo?>
    {
        private readonly IPaymentServiceClient _paymentServiceClient;
        private readonly MaticPaySerializer _maticPaySerializer;

        public GetWithdrawFormQueryService(IPaymentServiceClient paymentServiceClient, MaticPaySerializer maticPaySerializer)
        {
            _paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
        }

        public async ValueTask<WithdrawFormInfo?> GetAsync(GetWithdrawFormQuery query, CancellationToken cancellationToken = default)
        {
            var withdrawFormId = new Guid(query.OrderNo);

            var formData = await _paymentServiceClient
                .GetWithdrawFormAsync(withdrawFormId)
                .ConfigureAwait(false);

            return formData == null
                ? null
                : new WithdrawFormInfo(
                    formData.Id,
                    formData.UserId,
                    formData.UserType,
                    formData.BrandId,
                    formData.BrandPaymentId,
                    formData.Amount,
                    formData.Status,
                    formData.BankInfoId,
                    formData.BankInfo,
                    string.IsNullOrEmpty(formData.ApplyInfo)
                        ? null
                        : _maticPaySerializer.Deserialize<MaticPayWithdrawApplyInfo>(formData.ApplyInfo),
                    formData.PaymentReturn);
        }
    }
}
