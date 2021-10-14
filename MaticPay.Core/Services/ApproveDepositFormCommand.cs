using System;

namespace Tgs.Payment.MaticPay.Services
{
    public record ApproveDepositFormCommand(Guid FormId, Guid BrandId, decimal? ActualAmount);
}