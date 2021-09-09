using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayWithdrawResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = default!;

        [JsonPropertyName("msg")]
        public string Msg { get; init; } = default!;
    }
}