using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tgs.Payment.Clients;
using Tgs.Payment.MaticPay;

namespace MaticPayTest
{
    internal class Demo : BackgroundService
    {
        private readonly IPaymentClient _paymentClient;
        private readonly MaticPaySerializer _maticPaySerializer;
        private readonly ILogger<Demo> _logger;

        public Demo(IPaymentClient paymentClient, MaticPaySerializer maticPaySerializer, ILogger<Demo> logger)
        {
            _paymentClient = paymentClient ?? throw new ArgumentNullException(nameof(paymentClient));
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DepoistAsync();
            //await WithdrawAsync();
        }

        private async Task DepoistAsync()
        {
            var merchantNo = "API7313181217337047";
            var apiKey = "68199f7df5e142c69ae2516b75c176d5";
            var orderNo = Guid.NewGuid();
            var amount = 600;
            var customName = "實名制";
            var depositSettings = new MaticPayDepositSettings(
                merchantNo,
                apiKey,
                orderNo,
                amount,
                customName);
            try
            {
                var depositReturn = await _paymentClient.DepositAsync(
                    "tgs.payment.maticpay.deposit",
                    amount,
                    _maticPaySerializer.Serialize(depositSettings));
                _logger.LogInformation($"Deposit Return: {depositReturn}");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
            }
        }

        private async Task WithdrawAsync()
        {
            var merchantNo = "API7313181217337047";
            var apiKey = "68199f7df5e142c69ae2516b75c176d5";
            var orderNo = Guid.NewGuid();
            var accountName = "accountName";
            var bankCode = "ABC";
            var bankBranchName = "农业银行";
            var cardNo = "112233445566";
            var amount = 500;

            var withdrawSettings = new MaticPayWithdrawSettings(
                merchantNo,
                apiKey,
                orderNo,
                amount,
                accountName,
                bankCode,
                bankBranchName,
                cardNo);
            try
            {
                var withdrawReturn = await _paymentClient.WithdrawAsync(
                    "tgs.payment.maticpay.withdraw",
                    amount,
                    _maticPaySerializer.Serialize(withdrawSettings));
                _logger.LogInformation($"Withdraw Return: {withdrawReturn}");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
            }
        }
    }
}