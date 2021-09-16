using System.Collections.Generic;
using NUnit.Framework;

namespace Tgs.Payment.MaticPay.Tests
{
    public class HasherTests
    {
        [Test]
        public void Test_Check_Hash()
        {
            var date = "20191127172151";
            var orderNo = "1574846511910";
            var channleType = "0";
            var signType = "MD5";
            var noticeUrl = "https://www.baidu.com/";
            var version = "V2";
            var bizAmt = "10";
            var merchantNo = "API2442810283706600";
            var dict = new Dictionary<string, object>
            {
                { "date", date },
                { "orderNo", orderNo},
                { "channleType", channleType},
                { "signType", signType},
                { "noticeUrl", noticeUrl},
                { "version",version},
                { "bizAmt",bizAmt},
                { "merchantNo", merchantNo}
            };

            var hasher = new MaticPayHasher();
            var apiKey = "2cf6782df6d7478f872954ef8ff45a16";
            var actual = hasher.ComputeHash(apiKey, dict);
            var expected = "c9937998ef046127f762a002a6e8be8a";
            Assert.AreEqual(expected, actual);
        }
    }
}