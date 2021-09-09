using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.MaticPay.MessageQueue
{
    internal class WithdrawProcessor : IMessageProcessor
    {
        private readonly MaticPaySerializer _maticPaySerializer;
        private readonly MaticPayClient _maticPayClient;
        private readonly MaticPayOptions _options;

        public WithdrawProcessor(
            MaticPaySerializer maticPaySerializer,
            IOptions<MaticPayOptions> optionsAccessor,
            MaticPayClient maticPayClient)
        {
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
            _maticPayClient = maticPayClient ?? throw new ArgumentNullException(nameof(maticPayClient));
            _options = optionsAccessor.Value;
        }

        public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            var request = WithdrawRequest.Parser.ParseFrom(data);
            var settings = _maticPaySerializer.Deserialize<MaticPayWithdrawSettings>(request.ApplyInfo)!;

            var order = new WithdrawOrder
            {
                Version = "V2",
                SignType = "MD5",
                MerchantNo = settings.MerchantNo,
                Date = DateTime.Now.ToString("YYYYMMDDhhmmss"),
                ChannleType = "0",
                OrderNo = settings.OrderNo,
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