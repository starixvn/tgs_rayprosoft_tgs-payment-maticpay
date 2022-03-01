using System.Security.Cryptography;
using System.Text;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayHasher
    {
        private static readonly MD5 _Md5;

        static MaticPayHasher()
        {
            _Md5 = MD5.Create();
        }

        public string ComputeHash(string apiKey, IDictionary<string, object> dict)
        {
            dict = dict
                .Select(x => x)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            var content = string.Join(
                "&",
                dict.Select(p =>
                {
                    return p.Key == "sign" ? "" : $"{p.Key}={p.Value}";
                }));

            var plainText = $"{content}";

            var data = Encoding.UTF8.GetBytes(plainText);
            var base64 = Convert.ToBase64String(data);

            var fullText = $"{base64}{apiKey}";
            var hash = _Md5.ComputeHash(Encoding.UTF8.GetBytes(fullText));

            return Convert.ToHexString(hash).ToLower();
        }
    }
}