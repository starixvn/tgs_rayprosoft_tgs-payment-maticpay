using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayTransactionQueryResponseDetail
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        [JsonPropertyName("orderNo")]
        public string? OrderNo { get; init; }

        /// <summary>
        /// 交易金额
        /// </summary>
        [JsonPropertyName("bizAmt")]
        public string? BizAmt { get; init; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sysorderNo")]
        public string? SysOrderNo { get; init; }

        /// <summary>
        /// 交易状态
        /// 1 成功 2失败 0处理中
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; init; }

        /// <summary>
        /// 订单信息
        /// </summary>
        [JsonPropertyName("remark")]
        public string? Remark { get; init; }
    }
}