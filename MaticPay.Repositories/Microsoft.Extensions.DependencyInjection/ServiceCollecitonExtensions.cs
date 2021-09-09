using System;
using Microsoft.EntityFrameworkCore;
using Tgs.Payment.MaticPay;
using Tgs.Payment.MaticPay.Repositories;
using Tgs.Payment.MaticPay.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollecitonExtensions
    {
        public static IServiceCollection AddMaticPayRepositories(
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            int poolSize = 128)
        {
            return services
            .AddDbContext<TgsContext>(configure)
            .AddTransient<ITgsRepository, TgsRepository>();
        }
    }
}