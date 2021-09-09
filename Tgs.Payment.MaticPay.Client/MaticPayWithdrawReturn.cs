namespace Tgs.Payment.MaticPay
{
    public class MaticPayWithdrawReturn
    {
        /// <summary>
        /// 商戶訂單號
        /// </summary>
        public string OrderNo { get; init; } = default!;

        /// <summary>
        /// 訂單金額
        /// </summary>
        public string OrderAmt { get; init; } = default!;

        /// <summary>
        /// 確認金額
        /// </summary>
        public string BizAmt { get; init; } = default!;

        /// <summary>
        /// 狀態
        /// 1 成功 2失敗 0處理中
        /// </summary>
        public string Status { get; init; } = default!;

        /// <summary>
        /// 狀態信息
        /// </summary>
        public string Remark { get; init; } = default!;

        /// <summary>
        /// 常量V2
        /// </summary>
        public string Version { get; init; } = default!;

        /// <summary>
        /// 時間戳yyyyMMddHHmmss
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