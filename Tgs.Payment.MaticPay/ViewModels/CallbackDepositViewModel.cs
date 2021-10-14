using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay.ViewModels
{
    public class CallbackDepositViewModel
    {
        /// <summary>
        /// 商戶訂單號
        /// </summary>
        [JsonPropertyName("orderNo")]
        public string OrderNo { get; init; } = default!;

        /// <summary>
        /// 訂單金額
        /// </summary>
        [JsonPropertyName("orderAmt")]
        public string OrderAmount { get; init; } = default!;

        /// <summary>
        /// 代付金額
        /// </summary>
        [JsonPropertyName("bizAmt")]
        public string BizAmount { get; init; } = default!;

        /// <summary>
        /// 狀態
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; init; } = default!;

        /// <summary>
        /// 狀態信息
        /// </summary>
        [JsonPropertyName("remark")]
        public string Remark { get; init; } = default!;

        /// <summary>
        /// 常量 V2
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; init; } = default!;

        /// <summary>
        /// 時間戳
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; init; } = default!;

        /// <summary>
        /// 訂單傳入，原值返回
        /// </summary>
        [JsonPropertyName("notes")]
        public string Notes { get; init; } = default!;

        /// <summary>
        /// 簽名
        /// </summary>
        [JsonPropertyName("sign")]
        public string Sign { get; init; } = default!;
    }
}