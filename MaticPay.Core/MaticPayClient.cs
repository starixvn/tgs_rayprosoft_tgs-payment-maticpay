using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tgs.Payment.MaticPay
{
    public class MaticPayClient
    {
        private readonly HttpClient _httpClient;
        private readonly MaticPayHasher _maticPayHasher;
        private readonly ILogger<MaticPayClient> _logger;
        private readonly JsonSerializerOptions? _jsonSerializerOptions;

        public MaticPayClient(
            HttpClient httpClient,
            IOptions<MaticPayOptions> optionsAccessor,
            MaticPayHasher maticPayHasher,
            ILogger<MaticPayClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonSerializerOptions = optionsAccessor.Value.SystemSerializerOptions;
            _maticPayHasher = maticPayHasher ?? throw new ArgumentNullException(nameof(maticPayHasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<MaticPayDepositResponse?> DepositAsync(string merchantNo, string apiKey, DepositOrder order)
        {
            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            var sign = _maticPayHasher.ComputeHash(apiKey, orderDict);

            orderDict["sign"] = sign;

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await _httpClient
                .PostAsync("/api/pay/V2", content)
                .ConfigureAwait(false);

            _logger.LogInformation($"Deposit StatusCode: {response.StatusCode}");

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogInformation($"Deposit Body: {bodyJson}");

            response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<MaticPayDepositResponse>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;
        }

        public async ValueTask<MaticPayWithdrawResponse?> WithdrawAsync(string merchantNo, string apiKey, WithdrawOrder order)
        {
            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            var sign = _maticPayHasher.ComputeHash(apiKey, orderDict);

            orderDict["sign"] = sign;

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);

            var response = await _httpClient
                .PostAsync("/api/defray/V2", content)
                .ConfigureAwait(false);

            _logger.LogInformation($"Withdraw StatusCode: {response.StatusCode}");

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogInformation($"Withdraw Body: {bodyJson}");

            response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<MaticPayWithdrawResponse>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;
        }
    }
}