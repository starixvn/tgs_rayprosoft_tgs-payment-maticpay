using NUnit.Framework;

namespace Tgs.Payment.MaticPay.Tests
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public async Task TestTransactionQuery()
        {
            var version = "V2";
            var signType = "MD5";
            var merchantNo = "API7313181217337047";
            var date = DateTime.Now.ToString("yyyyMMddhhmmss");
            var orderNo = "68a51a08-ce7f-4666-a2a8-b1dbbe9d8c70";

            var dict = new Dictionary<string, object>
            {
                { "version",version},
                { "signType", signType},
                { "merchantNo", merchantNo},
                { "date", date },
                { "orderNo", orderNo},
                { "sign", null! }
            };
            var hasher = new MaticPayHasher();
            var apiKey = "68199f7df5e142c69ae2516b75c176d5";
            var sign = MaticPayHasher.ComputeHash(apiKey, dict);

            dict["sign"] = sign;

            var paramDict = dict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);

            var response = await new HttpClient()
                .PostAsync("https://mgp-pay.com:8443/api/defray/queryV2", content)
                .ConfigureAwait(false);

            Console.WriteLine($"Order StatusCode: {response.StatusCode}");

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Console.WriteLine($"Order Body: {bodyJson}");

            _ = response.EnsureSuccessStatusCode();
        }
    }
}