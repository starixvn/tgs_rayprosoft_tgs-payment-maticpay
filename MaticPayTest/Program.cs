using MaticPayTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.Payment.MaticPay;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            _ = services
            .AddMessageQueue()
            .AddNatsMessageQueue(_ => { })
            .AddNatsGlobPatternExchange("*");

            _ = services
            .AddPaymentClient()
            .AddSingleton<MaticPaySerializer>()
            .AddHostedService<Demo>();
        });