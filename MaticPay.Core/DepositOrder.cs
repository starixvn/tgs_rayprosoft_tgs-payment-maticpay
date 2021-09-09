using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tgs.Payment.MaticPay
{
    public class DepositOrder
    {
        /// <summary>
        /// 版本號
        /// 默認：V2
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; init; } = default!;
        /// <summary>
        /// 簽名類型
        /// MD5  RSA
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
        public string ChannleType { get; init; } = default!;
        /// <summary>
        /// 加密串
        /// </summary>
        [JsonPropertyName("sign")]
        public string? Sign { get; set; }
        /// <summary>
        /// 異步回調地址
        /// </summary>
        [JsonPropertyName("noticeUrl")]
        public string NoticeUrl { get; init; } = default!;
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
        /// 銀行代碼
        /// 部分通道需要
        /// </summary>
        [JsonPropertyName("bankCode")]
        public string? BankCode { get; init; }
        /// <summary>
        /// 同步回調地址
        /// </summary>
        [JsonPropertyName("returnUrl")]
        public string? ReturnUrl { get; init; }
        /// <summary>
        /// 會員ID
        /// </summary>
        [JsonPropertyName("customNo")]
        public string? CustomNo { get; init; }
        /// <summary>
        /// 會員名稱
        /// </summary>
        [JsonPropertyName("customName")]
        public string? CustomName { get; init; }
        /// <summary>
        /// 卡類型
        /// Alipay : 支付寶支付頁 ( 沒有重新提交按鈕 ) 
        /// Alipay2 : 支付寶支付頁(沒有重新提交按鈕 )
        /// Nine1 : 有支付寶文字
        /// Nine2 : 無支付寶文字
        /// </summary>
        [JsonPropertyName("cardAccountType")]
        public string? CardAccountType { get; init; }
    }
}
