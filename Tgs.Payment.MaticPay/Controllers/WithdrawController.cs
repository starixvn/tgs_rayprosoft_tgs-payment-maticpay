using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            var callbackMessage = new MaticPayWithdrawReturn()
            {
                OrderNo = request.OrderNo,
                OrderAmt = request.OrderAmt,
                BizAmt = request.BizAmt,
                Status = request.Status,
                Remark = request.Remark,
                Version = request.Version,
                Date = request.Date,
                Notes = request.Notes,
                Sign = request.Sign
            };

            await saveReturnService
                .ExecuteAsync(new SaveWithdrawReturnCommand(withdrawForm.Id, callbackMessage))
                .ConfigureAwait(false);

            _logger.LogInformation(JsonSerializer.Serialize(callbackMessage, jsonSerializerOptions));

            _logger.LogInformation("callback executed");
            return "SUCCESS";
        }
    }
}