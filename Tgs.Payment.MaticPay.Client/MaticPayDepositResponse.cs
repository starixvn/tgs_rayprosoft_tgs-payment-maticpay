using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public MaticDepositResponseDetail Detail { get; init; } = default!;
        [JsonPropertyName("checkstandInfo")]
        public MaticDepositRepsonseCheckStandInfo CheckStandInfo { get; init; } = default!;
    }
}
