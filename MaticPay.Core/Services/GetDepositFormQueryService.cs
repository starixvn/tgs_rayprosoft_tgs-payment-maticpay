using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.Payment.Clients;
using TGS.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class GetDepositFormQueryService : IGetService<GetDepositFormQuery, DepositFormInfo?>
    {
        private readonly IPaymentServiceClient _paymentServiceClient;
        private readonly MaticPaySerializer _maticPaySerializer;

        public GetDepositFormQueryService(
            IPaymentServiceClient paymentServiceClient,
            MaticPaySerializer maticPaySerializer)
        {
            _paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
        }

        public async ValueTask<DepositFormInfo?> GetAsync(GetDepositFormQuery query, CancellationToken cancellationToken = default)
        {
            var formId = new Guid(query.OrderNo);

            var formData = await _paymentServiceClient
                .GetDepositFormAsync(formId)
                .ConfigureAwait(false);

            return formData == null
                ? null
                : new DepositFormInfo(
                    formData.Id,
                    formData.PlayerCode,
                    formData.BrandId,
                    formData.BrandPaymentId,
                    formData.Amount,
                    formData.Status,
                    string.IsNullOrEmpty(formData.ApplyInfo)
                        ? null
                        : _maticPaySerializer.Deserialize<MaticPayDepositApplyInfo>(formData.ApplyInfo));
        }
    }
}
