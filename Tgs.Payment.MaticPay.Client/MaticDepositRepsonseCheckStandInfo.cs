using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class MaticDepositRepsonseCheckStandInfo
    {
        /// <summary>
        /// 二維碼地址
        /// </summary>
        [JsonPropertyName("qrcode_url")]
        public string QRCode_Url { get; init; } = default!;

        /// <summary>
        /// 卡號
        /// </summary>

        [JsonPropertyName("bankAccount")]
        public string BankAccount { get; init; } = default!;

        /// <summary>
        /// 銀行編碼
        /// </summary>
        [JsonPropertyName("bankCode")]
        public string BankCode { get; init; } = default!;

        /// <summary>
        /// 銀行名稱
        /// </summary>
        [JsonPropertyName("bankName")]
        public string BankName { get; init; } = default!;

        /// <summary>
        /// 持卡人
        /// </summary>
        [JsonPropertyName("bankAccountName")]
        public string BankAccountName { get; init; } = default!;

        /// <summary>
        /// 金額
        /// </summary>
        [JsonPropertyName("money")]
        public string Money { get; init; } = default!;
    }
}