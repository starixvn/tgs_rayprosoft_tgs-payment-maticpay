using System.Text.Json.Serialization;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayDepositReturn
    {
        /// <summary>
        /// 商戶訂單號
        /// </summary>
        public string OrderNo { get; init; } = default!;

        /// <summary>
        /// 訂單金額
        /// </summary>
        [JsonPropertyName("orderAmt")]
        public decimal OrderAmount { get; init; }

        /// <summary>
        /// 代付金額
        /// </summary>
        [JsonPropertyName("bizAmt")]
        public decimal BizAmount { get; init; } = default!;

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; init; } = default!;

        /// <summary>
        /// 狀態信息
        /// </summary>
        public string Remark { get; init; } = default!;

        /// <summary>
        /// 常量 V2
        /// </summary>
        public string Version { get; init; } = default!;

        /// <summary>
        /// 時間戳
        /// </summary>
        public string Date { get; init; } = default!;

        /// <summary>
        /// 訂單傳入，原值返回
        /// </summary>
        public string Notes { get; init; } = default!;

        /// <summary>
        /// 簽名
        /// </summary>
        public string Sign { get; init; } = default!;
    }
}