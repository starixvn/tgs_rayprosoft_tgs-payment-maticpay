using Google.Protobuf;
using Microsoft.Extensions.Options;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.MaticPay.MessageQueue
{
    internal class DepositProcessor : IMessageProcessor
    {
        private readonly MaticPaySerializer _maticPaySerializer;
        private readonly MaticPayClient _maticPayClient;
        private readonly MaticPayOptions _options;

        public DepositProcessor(
            MaticPaySerializer maticPaySerializer,
            MaticPayClient maticPayClient,
            IOptions<MaticPayOptions> optionsAccessor)
        {
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
            _maticPayClient = maticPayClient ?? throw new ArgumentNullException(nameof(maticPayClient));
            _options = optionsAccessor.Value;
        }

        public async ValueTask<ReadOnlyMemory<byte>> HandleAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
        {
            var request = DepositRequest.Parser.ParseFrom(data.Span);
            var settings = _maticPaySerializer.Deserialize<MaticPayDepositSettings>(request.ApplyInfo)!;

            var order = new DepositOrder
            {
                Version = "V2",
                SignType = "MD5",
                MerchantNo = settings.MerchantNo,
                Date = DateTime.Now.ToString("yyyyMMddhhmmss"),
                ChannleType = "0",
                NoticeUrl = $"{_options.CallbackBaseUrl}/deposit/callback",
                OrderNo = settings.OrderNo.ToString(),
                BizAmt = decimal.Parse(request.Amount),
                CustomName = settings.CustomName
            };

            var depositResponse = await _maticPayClient
                .DepositAsync(settings.MerchantNo, settings.ApiKey, order)
                .ConfigureAwait(false);

            if (depositResponse != null && depositResponse.Code == "0")
            {
                var applyInfo = new MaticPayDepositApplyInfo(settings, depositResponse);

                var msg = new DepositResponse
                {
                    Result = _maticPaySerializer.Serialize(applyInfo)
                };

                return msg.ToByteArray();
            }
            else
            {
                var errorMsg = string.Empty;
                if (depositResponse != null)
                {
                    errorMsg = depositResponse.Msg ?? "Request Fail";
                }

                var response = new MaticPayDepositFailResponse
                {
                    Code = depositResponse!.Code,
                    Msg = errorMsg
                };

                var applyInfo = new MaticPayDepositApplyInfo(settings, response);

                var msg = new DepositResponse
                {
                    Result = _maticPaySerializer.Serialize(applyInfo)
                };

                return msg.ToByteArray();
            }
        }
    }
}