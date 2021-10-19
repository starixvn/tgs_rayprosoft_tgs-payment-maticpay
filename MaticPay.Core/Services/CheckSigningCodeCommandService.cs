using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Payment.MaticPay.Services
{
    internal class CheckSigningCodeCommandService : ICommandService<CheckSigningCodeCommand>
    {
        private readonly MaticPayHasher _hasher;

        public CheckSigningCodeCommandService(MaticPayHasher hasher)
        {
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        public ValueTask ExecuteAsync(CheckSigningCodeCommand command, CancellationToken cancellationToken = default)
        {
            var signCode = _hasher.ComputeHash(command.ApiKey, command.Data);

            command.CommandResultSource.SetResult(signCode == command.SigningCode);

            return ValueTask.CompletedTask;
        }
    }
}