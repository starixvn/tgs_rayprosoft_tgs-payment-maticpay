using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayHasher
    {
        private readonly static MD5 _Md5;

        static MaticPayHasher()
        {
            _Md5 = MD5.Create();
        }

        public string ComputeHash(string apiKey, IDictionary<string, object> dict)
        {
            var content = string.Join(
                "&",
                dict.Where(x => x.Value != null && x.Value.ToString() != "")
                    .OrderBy(x => x.Key).Select(p =>
                    {
                        return $"{p.Key}={p.Value}";
                    }));

            var plainText = $"{content}{apiKey}";

            var data = Encoding.UTF8.GetBytes(plainText);

            var hash = _Md5.ComputeHash(data);

            return Convert.ToHexString(hash).ToLower();
        }
    }
}