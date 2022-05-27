using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.Payment.Clients;
using Tgs.Payment.MaticPay.Services;
using Tgs.Payment.MaticPay.ViewModels;
using Tgs.ServiceUtility;

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
            [FromServices] IGetService<GetWithdrawFormQuery, WithdrawFormInfo?> getWithdrawFormService,
            [FromServices] ICommandService<CheckSigningCodeCommand> checkSigningCodeService,
            [FromServices] ICommandService<SaveWithdrawReturnCommand> saveReturnService,
            [FromServices] IPaymentClient paymentClient,
            [FromBody] Dictionary<string, object> response)
        {
            var json = JsonSerializer.Serialize(response);

            _logger.LogInformation(json);

            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
            _ = dict.Remove("sign");

            var jsonSerializerOptions = jsonOptionsAccessor.Value.JsonSerializerOptions;

            var withdrawResponse = JsonSerializer.Deserialize<CallbackWithdrawViewModel>(json)!;

            var withdrawForm = await getWithdrawFormService
                .GetAsync(new GetWithdrawFormQuery(withdrawResponse.OrderNo))
                .ConfigureAwait(false);

            if (withdrawForm == null)
                return "Withdraw form not found.";

            var checkSigningCodeCommand = new CheckSigningCodeCommand(
                dict,
                withdrawResponse.Sign,
                withdrawForm!.ApplyInfo!.Settings.ApiKey
                );

            await checkSigningCodeService
                .ExecuteAsync(checkSigningCodeCommand)
                .ConfigureAwait(false);

            if (await checkSigningCodeCommand.CommandResultSource.Task.ConfigureAwait(false))
            {
                var returnData = new MaticPayWithdrawReturn()
                {
                    OrderNo = withdrawResponse.OrderNo,
                    OrderAmount = decimal.Parse(withdrawResponse.OrderAmt),
                    BizAmount = decimal.Parse(withdrawResponse.BizAmt),
                    Status = withdrawResponse.Status,
                    Remark = withdrawResponse.Remark,
                    Version = withdrawResponse.Version,
                    Date = withdrawResponse.Date,
                    Notes = withdrawResponse.Notes,
                    Sign = withdrawResponse.Sign
                };

                if (withdrawResponse.Status == "1" && withdrawForm.Status == PaymentFlowState.Approved)
                {
                    var saveWithdrawCommand = new SaveWithdrawReturnCommand(
                                withdrawForm.Id,
                                returnData,
                                returnData.BizAmount);

                    await saveReturnService
                        .ExecuteAsync(saveWithdrawCommand)
                        .ConfigureAwait(false);

                    await paymentClient
                        .PaidWithdrawAsync(
                            withdrawForm.Id,
                            withdrawForm.BrandId,
                            HttpContext.RequestAborted)
                        .ConfigureAwait(false);
                }
                else
                {
                    var saveWithdrawCommand = new SaveWithdrawReturnCommand(
                        withdrawForm.Id,
                        returnData,
                        null);

                    await saveReturnService
                        .ExecuteAsync(saveWithdrawCommand)
                        .ConfigureAwait(false);
                }

                _logger.LogInformation(JsonSerializer.Serialize(returnData, jsonSerializerOptions));

                _logger.LogInformation("callback executed");

                return "SUCCESS";
            }
            else
                throw new Exception();
        }
    }
}
