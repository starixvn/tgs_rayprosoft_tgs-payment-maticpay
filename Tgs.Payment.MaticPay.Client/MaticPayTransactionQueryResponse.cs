using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayTransactionQueryResponse
    {
        /// <summary>
        /// 响应码
        /// 0成功 -1失败
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; init; } = default!;

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; init; } = default!;

        [JsonPropertyName("detail")]
        public MaticPayTransactionQueryResponseDetail? Detail { get; init; }
    }
}