using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.Payment.MaticPay
{
    public class MaticDepositResponseDetail
    {
        public string PayHtml { get; set; } = default!;
        public string PayURL { get; set; } = default!;
    }
}
