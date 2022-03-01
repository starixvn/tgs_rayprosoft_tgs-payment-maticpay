using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.MessageQueue;
using Tgs.Payment.Clients;
using Tgs.Payment.Protos;

namespace Tgs.Payment.MaticPay.MessageQueue
{
    internal class WithdrawProcessor : IMessageProcessor
    {
        private readonly MaticPaySerializer _maticPaySerializer;
        private readonly MaticPayClient _maticPayClient;
        private readonly ILogger<WithdrawProcessor> _logger;
        private readonly IPaymentClient _paymentClient;
        private readonly MaticPayOptions _options;

        public WithdrawProcessor(
            MaticPaySerializer maticPaySerializer,
            IOptions<MaticPayOptions> optionsAccessor,
            MaticPayClient maticPayClient,
            ILogger<WithdrawProcessor> logger,
            IPaymentClient paymentClient)
        {
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
            _maticPayClient = maticPayClient ?? throw new ArgumentNullException(nameof(maticPayClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentClient = paymentClient ?? throw new ArgumentNullException(nameof(paymentClient));
            _options = optionsAccessor.Value;
        }

        public async ValueTask<ReadOnlyMemory<byte>> HandleAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
        {
            var request = WithdrawRequest.Parser.ParseFrom(data.Span);
            var settings = _maticPaySerializer.Deserialize<MaticPayWithdrawSettings>(request.ApplyInfo)!;

            var order = new WithdrawOrder
            {
                Version = "V2",
                SignType = "MD5",
                MerchantNo = settings.MerchantNo,
                Date = DateTime.Now.ToString("yyyyMMddhhmmss"),
                ChannleType = "0",
                OrderNo = settings.OrderNo.ToString(),
                BizAmt = decimal.Parse(request.Amount),
                AccountName = settings.AccountName,
                BankCode = settings.BankCode,
                BankBranchName = settings.BankBranchName,
                CardNo = settings.CardNo,
                NoticeUrl = $"{_options.CallbackBaseUrl}/withdraw/callback"
            };

            var withdrawResponse = await _maticPayClient
                .WithdrawAsync(settings.MerchantNo, settings.ApiKey, order)
                .ConfigureAwait(false);

            if (withdrawResponse != null && withdrawResponse.Code == "0")
            {
                var applyInfo = new MaticPayWithdrawApplyInfo(settings, withdrawResponse);

                var transactionQueryOrder = new TransactionQueryOrder()
                {
                    Version = "V2",
                    SignType = "MD5",
                    MerchantNo = settings.MerchantNo,
                    Date = DateTime.Now.ToString("yyyyMMddhhmmss"),
                    OrderNo = settings.OrderNo.ToString()
                };
                var transactionQueryResponse = await _maticPayClient
                    .TransactionQueryAsync(settings.MerchantNo, settings.ApiKey, transactionQueryOrder)
                    .ConfigureAwait(false);

                var thirdPartyOrderNo = "";
                if (transactionQueryResponse!.Code == "0")
                {
                    thirdPartyOrderNo = transactionQueryResponse!.Detail!.SysOrderNo;
                }
                else
                {
                    _logger.LogInformation($"transactionQueryResponse Failed:{transactionQueryResponse.Msg}");
                }

                await _paymentClient
                    .SaveWithdrawFormThirdPartyOrderNoAsync(settings.OrderNo, thirdPartyOrderNo!)
                    .ConfigureAwait(false);

                var msg = new WithdrawResponse
                {
                    Result = _maticPaySerializer.Serialize(applyInfo)
                };

                return msg.ToByteArray();
            }
            else
            {
                var response = new MaticPayWithdrawFailResponse
                {
                    Code = withdrawResponse!.Code,
                    Msg = withdrawResponse!.Msg
                };

                var applyInfo = new MaticPayWithdrawApplyInfo(settings, response);

                var msg = new WithdrawResponse
                {
                    Result = _maticPaySerializer.Serialize(applyInfo)
                };

                return msg.ToByteArray();
            }
        }
    }
}