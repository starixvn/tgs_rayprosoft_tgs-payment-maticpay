using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Tgs.Payment.Clients;
using Tgs.Payment.MaticPay.Services;
using Tgs.Payment.MaticPay.ViewModels;
using Tgs.ServiceUtility;

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
           [FromServices] IGetService<GetDepositFormQuery, DepositFormInfo?> getDepositFormService,
           [FromServices] ICommandService<CheckSigningCodeCommand> checkSigningCodeService,
           [FromServices] ICommandService<SaveDepositReturnCommand> saveReturnService,
           [FromServices] ICommandService<ApproveDepositFormCommand> approveService,
           [FromBody] Dictionary<string, object> response)
        {
            var json = JsonSerializer.Serialize(response);

            _logger.LogInformation(json);

            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
            _ = dict.Remove("sign");

            var depositResponse = JsonSerializer.Deserialize<CallbackDepositViewModel>(json)!;

            var depositForm = await getDepositFormService
                .GetAsync(new GetDepositFormQuery(depositResponse.OrderNo))
                .ConfigureAwait(false);

            var checkSigningCodeCommand = new CheckSigningCodeCommand(
                dict,
                depositResponse.Sign,
                depositForm!.ApplyInfo!.Settings.ApiKey
                );

            await checkSigningCodeService
                .ExecuteAsync(checkSigningCodeCommand)
                .ConfigureAwait(false);

            if (await checkSigningCodeCommand.CommandResultSource.Task.ConfigureAwait(false))
            {
                if (depositForm.Status < PaymentFlowState.WaitApprove)
                {
                    var returnData = new MaticPayDepositReturn()
                    {
                        OrderNo = depositResponse.OrderNo,
                        OrderAmount = decimal.Parse(depositResponse.OrderAmount),
                        BizAmount = decimal.Parse(depositResponse.BizAmount),
                        Status = depositResponse.Status,
                        Remark = depositResponse.Remark,
                        Version = depositResponse.Version,
                        Date = depositResponse.Date,
                        Notes = depositResponse.Notes,
                        Sign = depositResponse.Sign
                    };

                    await saveReturnService
                        .ExecuteAsync(new SaveDepositReturnCommand(depositForm.Id, returnData))
                        .ConfigureAwait(false);

                    var jsonSerializerOptions = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    _logger.LogInformation(JsonSerializer.Serialize(returnData, jsonSerializerOptions));

                    await approveService
                        .ExecuteAsync(new ApproveDepositFormCommand(
                            depositForm.Id,
                            depositForm.BrandId,
                            returnData.BizAmount))
                        .ConfigureAwait(false);
                }

                _logger.LogInformation("callback executed");

                return "SUCCESS";
            }
            else
                throw new Exception();
        }
    }
}