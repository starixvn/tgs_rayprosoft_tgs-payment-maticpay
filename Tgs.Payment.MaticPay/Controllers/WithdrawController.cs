using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.Payment.Clients;
using Tgs.Payment.MaticPay.Services;
using Tgs.Payment.MaticPay.ViewModels;
using TGS.ServiceUtility;

namespace Tgs.Payment.MaticPay.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        private readonly ILogger<WithdrawController> _logger;

        public WithdrawController(
            ILogger<WithdrawController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("callback")]
        [Produces("text/plain")]
        public async Task<string> WithdrawCallback(
            [FromServices] IOptions<JsonOptions> jsonOptionsAccessor,
            [FromServices] IGetService<GetWithdrawFormQuery, WithdrawFormInfo> getWithdrawFormService,
            [FromServices] ICommandService<SaveWithdrawReturnCommand> saveReturnService,
            [FromBody] CallbackWithdrawViewModel request)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    $"Callback execute, request: " +
                    $"{JsonSerializer.Serialize(request)}");
            }
            var jsonSerializerOptions = jsonOptionsAccessor.Value.JsonSerializerOptions;

            var withdrawForm = await getWithdrawFormService
                .GetAsync(new GetWithdrawFormQuery(request.OrderNo))
                .ConfigureAwait(false);

            if (withdrawForm == null)
                return "Withdraw form not found.";

            var callbackMessage = new MaticPayWithdrawReturn()
            {
                OrderNo = request.OrderNo,
                OrderAmount = decimal.Parse(request.OrderAmt),
                BizAmount = request.BizAmt,
                Status = request.Status,
                Remark = request.Remark,
                Version = request.Version,
                Date = request.Date,
                Notes = request.Notes,
                Sign = request.Sign
            };

            var formStatus = withdrawForm.Status;
            if (withdrawForm.Status == PaymentFlowState.Approved)
                formStatus = PaymentFlowState.Paid;

            await saveReturnService
                .ExecuteAsync(new SaveWithdrawReturnCommand(
                    withdrawForm.Id,
                    formStatus,
                    callbackMessage,
                    callbackMessage.OrderAmount))
                .ConfigureAwait(false);

            _logger.LogInformation(JsonSerializer.Serialize(callbackMessage, jsonSerializerOptions));

            _logger.LogInformation("callback executed");

            return "SUCCESS";
        }
    }
}
