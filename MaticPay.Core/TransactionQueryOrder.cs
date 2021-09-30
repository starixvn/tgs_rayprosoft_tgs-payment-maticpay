using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class TransactionQueryOrder
    {
        /// <summary>
        /// 版本號
        /// 默認:V2
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; init; } = default!;

        /// <summary>
        /// 簽名類型
        /// MD5 RSA
        /// </summary>
        [JsonPropertyName("signType")]
        public string SignType { get; init; } = default!;

        /// <summary>
        /// 商戶號
        /// </summary>
        [JsonPropertyName("merchantNo")]
        public string MerchantNo { get; init; } = default!;

        /// <summary>
        /// 時間戳
        /// 格式：YYYYMMDDhhmmss
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; init; } = default!;

        /// <summary>
        /// 加密串
        /// </summary>
        [JsonPropertyName("sign")]
        public string? Sign { get; set; }

        /// <summary>
        /// 訂單號
        /// </summary>
        [JsonPropertyName("orderNo")]
        public string OrderNo { get; init; } = default!;
    }
}