using Tgs.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class CheckSigningCodeCommandService : ICommandService<CheckSigningCodeCommand>
    {
        public CheckSigningCodeCommandService()
        {
        }

        public ValueTask ExecuteAsync(CheckSigningCodeCommand command, CancellationToken cancellationToken = default)
        {
            var signCode = MaticPayHasher.ComputeHash(command.ApiKey, command.Data);

            command.CommandResultSource.SetResult(signCode == command.SigningCode);

            return ValueTask.CompletedTask;
        }
    }
}