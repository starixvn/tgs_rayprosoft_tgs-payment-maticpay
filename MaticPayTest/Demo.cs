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
        }

        private async Task DepoistAsync()
        {
            var merchantNo = "API7313181217337047";
            var apiKey = "68199f7df5e142c69ae2516b75c176d5";
            var orderNo = Guid.NewGuid();
            var depositSettings = new MaticPayDepositSettings(
                merchantNo,
                apiKey,
                orderNo,
                100m);
            try
            {
                var depositReturn = await _paymentClient.DepositAsync(
                    "tgs.payment.maticpay.deposit",
                    100,
                    _maticPaySerializer.Serialize(depositSettings));
                _logger.LogInformation($"Deposit Return:", depositReturn);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
            }
        }
    }
}