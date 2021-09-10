using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayDepositResponse
    {
        /// <summary>
        /// 響應碼
        /// 0成功 -1失敗
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; init; } = default!;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; init; } = default!;

        [JsonPropertyName("detail")]
        public MaticDepositResponseDetail? Detail { get; init; }

        [JsonPropertyName("checkstandInfo")]
        public MaticDepositRepsonseCheckStandInfo? CheckStandInfo { get; init; }
    }
}