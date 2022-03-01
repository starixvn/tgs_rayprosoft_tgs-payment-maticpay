using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions? _jsonSerializerOptions;

        public MaticPayClient(
            HttpClient httpClient,
            IOptions<MaticPayOptions> optionsAccessor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonSerializerOptions = optionsAccessor.Value.SystemSerializerOptions;
        }

        public async ValueTask<MaticPayDepositResponse?> DepositAsync(string merchantNo, string apiKey, DepositOrder order)
        {
            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            var sign = MaticPayHasher.ComputeHash(apiKey, orderDict);

            orderDict["sign"] = sign;

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);

            var response = await _httpClient
                .PostAsync("/api/pay/V2", content)
                .ConfigureAwait(false);

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _ = response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<MaticPayDepositResponse>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;
        }

        public async ValueTask<MaticPayWithdrawResponse?> WithdrawAsync(string merchantNo, string apiKey, WithdrawOrder order)
        {
            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            var sign = MaticPayHasher.ComputeHash(apiKey, orderDict);

            orderDict["sign"] = sign;

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);

            var response = await _httpClient
                .PostAsync("/api/defray/V2", content)
                .ConfigureAwait(false);

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _ = response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<MaticPayWithdrawResponse>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;
        }

        public async ValueTask<MaticPayTransactionQueryResponse?> TransactionQueryAsync(string merchantNo, string apiKey, TransactionQueryOrder order)
        {
            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            var sign = MaticPayHasher.ComputeHash(apiKey, orderDict);

            orderDict["sign"] = sign;

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);

            var response = await _httpClient
                .PostAsync("/api/defray/queryV2", content)
                .ConfigureAwait(false);

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _ = response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<MaticPayTransactionQueryResponse?>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;
        }
    }
}