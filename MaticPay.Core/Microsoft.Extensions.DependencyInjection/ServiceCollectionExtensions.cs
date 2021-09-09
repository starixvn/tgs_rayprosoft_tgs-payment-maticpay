using System;
using System.Reflection;
using Microsoft.Extensions.Options;
using Tgs.Payment.MaticPay;
using Tgs.Payment.MaticPay.MessageQueue;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaticPay(this IServiceCollection services, Action<MaticPayOptions, IServiceProvider> configure)
            => services
                .AddOptions<MaticPayOptions>()
                .Configure(configure)
                .Services
                .AddHttpClient<MaticPayClient>()
                .ConfigureHttpClient((sp, httpClient) =>
                {
                    var options = sp.GetRequiredService<IOptions<MaticPayOptions>>().Value;

                    httpClient.BaseAddress = options.PaymentApiUrl;
                })
                .Services
                .RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
                .AddTransient<MaticPayHasher>()
                .AddSingleton<MaticPaySerializer>()
                .AddNatsMessageQueue(config =>
                {
                    config
                        .ConfigQueueOptions((options, sp) =>
                        {
                            var umPayOptions = sp.GetRequiredService<IOptions<MaticPayOptions>>().Value;

                            options.Url = umPayOptions.NatsUrl;
                        })
                        .AddProcessor<DepositProcessor>("tgs.payment.maticpay.deposit", "tgs.payment.maticpay.deposit")
                        .AddProcessor<WithdrawProcessor>("tgs.payment.maticpay.withdraw", "tgs.payment.maticpay.withdraw");
                })
                .AddPaymentClient();
    }
}