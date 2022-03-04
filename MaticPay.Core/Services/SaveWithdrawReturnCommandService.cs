using Tgs.Payment.Clients;
using Tgs.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class SaveWithdrawReturnCommandService : ICommandService<SaveWithdrawReturnCommand>
    {
        private readonly IPaymentServiceClient _paymentServiceClient;
        private readonly MaticPaySerializer _maticPaySerializer;

        public SaveWithdrawReturnCommandService(IPaymentServiceClient paymentServiceClient, MaticPaySerializer maticPaySerializer)
        {
            _paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
        }

        public async ValueTask ExecuteAsync(SaveWithdrawReturnCommand command, CancellationToken cancellationToken = default)
        {
            var formData = await _paymentServiceClient
                .GetWithdrawFormAsync(command.FormId)
                .ConfigureAwait(false);

            if (formData == null)
                return;

            await _paymentServiceClient
                .SubmitWithdrawFormStatusAsync(
                    formData.Id,
                    command.State,
                    _maticPaySerializer.Serialize(command.WithdrawReturn),
                    command.ActualAmount)
                .ConfigureAwait(false);
        }
    }
}