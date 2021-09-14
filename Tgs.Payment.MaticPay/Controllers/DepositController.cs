using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tgs.Payment.MaticPay.Services;
using Tgs.Payment.MaticPay.ViewModels;
using TGS.ServiceUtility;

namespace Tgs.Payment.MaticPay.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly ILogger<DepositController> _logger;

        public DepositController(
            ILogger<DepositController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("callback")]
        [Produces("text/plain")]
        public async Task<string> DepositCallback(
           [FromServices] IGetService<GetDepositFormQuery, DepositFormInfo> getDepositFormService,
           [FromServices] ICommandService<SaveDepositReturnCommand> saveReturnService,
           [FromServices] ICommandService<ApproveDepositFormCommand> approveService,
           [FromBody] CallbackDepositViewModel request)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    $"Callback execute, request: " +
                    $"{JsonSerializer.Serialize(request)}");
            }

            var depositForm = await getDepositFormService
                .GetAsync(new GetDepositFormQuery(request.OrderNo))
                .ConfigureAwait(false);

            if (depositForm.Status < 1000)
            {
                var callbackMessage = new MaticPayDepositReturn()
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
                    .ExecuteAsync(new SaveDepositReturnCommand(depositForm.Id, callbackMessage))
                    .ConfigureAwait(false);

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                _logger.LogInformation(JsonSerializer.Serialize(callbackMessage, jsonSerializerOptions));

                await approveService
                    .ExecuteAsync(new ApproveDepositFormCommand(depositForm.Id, depositForm.BrandId))
                    .ConfigureAwait(false);
            }

            _logger.LogInformation("callback executed");
            return "SUCCESS";
        }
    }
}