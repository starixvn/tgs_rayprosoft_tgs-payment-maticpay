using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class WithdrawOrder
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
        /// 通道類型
        /// </summary>
        [JsonPropertyName("channleType")]
        public string ChannleType { get; set; } = default!;

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

        /// <summary>
        /// 金額
        /// </summary>
        [JsonPropertyName("bizAmt")]
        public decimal BizAmt { get; init; }

        /// <summary>
        /// 收款人姓名
        /// </summary>
        [JsonPropertyName("accName")]
        public string AccountName { get; init; } = default!;

        /// <summary>
        /// 銀行編碼
        /// </summary>
        [JsonPropertyName("bankCode")]
        public string BankCode { get; init; } = default!;

        /// <summary>
        /// 支行名稱
        /// </summary>
        [JsonPropertyName("bankBranchName")]
        public string BankBranchName { get; init; } = default!;

        /// <summary>
        /// 卡號
        /// </summary>
        [JsonPropertyName("cardNo")]
        public string CardNo { get; init; } = default!;

        /// <summary>
        /// 異步回調地址
        /// </summary>
        [JsonPropertyName("noticeUrl")]
        public string? NoticeUrl { get; init; }

        /// <summary>
        /// 開戶省
        /// </summary>
        [JsonPropertyName("openProvince")]
        public string? OpenProvince { get; init; }

        /// <summary>
        /// 開戶市
        /// </summary>
        [JsonPropertyName("openCity")]
        public string? OpenCity { get; init; }

        /// <summary>
        /// 手機號
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; init; }

        /// <summary>
        /// 出款類型
        /// </summary>
        [JsonPropertyName("payoutType")]
        public string? PayOutType { get; init; }

        /// <summary>
        /// Pix類型
        /// </summary>
        [JsonPropertyName("pixType")]
        public string? PixType { get; init; }

        /// <summary>
        /// 帳戶種類
        /// </summary>
        [JsonPropertyName("accKind")]
        public string? AccountKind { get; init; }

        /// <summary>
        /// 帳號類型
        /// </summary>
        [JsonPropertyName("accountType")]
        public string? AccountType { get; init; }
    }
}