using Tgs.Payment.Clients;
using Tgs.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class SaveDepositReturnCommandService : ICommandService<SaveDepositReturnCommand>
    {
        private readonly IPaymentServiceClient _paymentServiceClient;
        private readonly MaticPaySerializer _maticPaySerializer;

        public SaveDepositReturnCommandService(IPaymentServiceClient paymentServiceClient, MaticPaySerializer maticPaySerializer)
        {
            _paymentServiceClient = paymentServiceClient ?? throw new ArgumentNullException(nameof(paymentServiceClient));
            _maticPaySerializer = maticPaySerializer ?? throw new ArgumentNullException(nameof(maticPaySerializer));
        }

        public async ValueTask ExecuteAsync(SaveDepositReturnCommand command, CancellationToken cancellationToken = default)
        {
            var formData = await _paymentServiceClient
                .GetDepositFormAsync(command.FormId)
                .ConfigureAwait(false);

            if (formData == null)
                return;

            await _paymentServiceClient
                .SubmitDepositFormStatusAsync(
                    command.FormId,
                    formData.Status,
                    _maticPaySerializer.Serialize(command.DepositReturn))
                .ConfigureAwait(false);
        }
    }
}