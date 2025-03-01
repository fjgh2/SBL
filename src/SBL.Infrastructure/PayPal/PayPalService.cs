using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SBL.Domain.Common;
using SBL.Infrastructure.PayPal.Models;
using SBL.Services.Contracts.Services;

namespace SBL.Infrastructure.PayPal;

public class PayPalService : IPaymentService
{
    private readonly HttpClient _httpClient;

    private readonly string _clientId;

    private readonly string _secret;

    private readonly string _returnUrl;

    private readonly string _cancelUrl;

    private readonly string _baseUrl;

    public PayPalService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _clientId = configuration["PayPal:ClientId"];
        _secret = configuration["PayPal:Secret"];
        _baseUrl = "https://api.sandbox.paypal.com";
        _returnUrl = configuration["PayPal:ReturnUrl"];
        _cancelUrl = configuration["PayPal:CancelUrl"];

        if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_secret))
        {
            throw new ArgumentException("PayPal client ID and secret must be configured");
        }
    }

    public async Task<Result<PayPalRedirectResult>> CreatePaymentAsync(PaymentRequest request)
    {
        try
        {
            var token = await GetAccessTokenAsync();

            var payment = new
            {
                intent = "sale",
                payer = new
                {
                    payment_method = "paypal"
                },
                transactions = new[]
                {
                    new
                    {
                        amount = new
                        {
                            total = request.Amount.ToString("0.00"),
                            currency = request.Currency
                        },
                        description = request.Description,
                        invoice_number = request.InvoiceNumber
                    }
                },
                redirect_urls = new
                {
                    return_url = $"{_returnUrl}?orderId={request.OrderId}",
                    cancel_url = $"{_cancelUrl}?orderId={request.OrderId}"
                }
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonSerializer.Serialize(payment);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/v1/payments/payment", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                return Result.Fail<PayPalRedirectResult>($"Failed to create PayPal payment: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PayPalPaymentResponse>(responseContent);

            string approvalUrl = null;
            if (paymentResponse?.Links != null)
            {
                foreach (var link in paymentResponse.Links)
                {
                    if (link.Rel == "approval_url")
                    {
                        approvalUrl = link.Href;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(approvalUrl))
            {
                return Result.Fail<PayPalRedirectResult>("No approval URL found in PayPal response");
            }

            return Result.Success(new PayPalRedirectResult
            {
                PaymentId = paymentResponse.Id,
                RedirectUrl = approvalUrl
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<PayPalRedirectResult>($"Exception during payment creation: {ex.Message}");
        }
    }

    public async Task<Result<PaymentExecutionResult>> ExecutePaymentAsync(string paymentId, string payerId)
    {
        try
        {
            var token = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var execution = new { payer_id = payerId };
            var jsonContent = JsonSerializer.Serialize(execution);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                $"{_baseUrl}/v1/payments/payment/{paymentId}/execute", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                return Result.Fail<PaymentExecutionResult>($"Failed to execute PayPal payment: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var executionResponse = JsonSerializer.Deserialize<PayPalExecutionResponse>(responseContent);

            return Result.Success<PaymentExecutionResult>(new PaymentExecutionResult
            {
                PaymentId = paymentId,
                State = executionResponse?.State,
                TransactionId = executionResponse?.Transactions?[0]?.RelatedResources?[0]?.Sale?.Id
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<PaymentExecutionResult>($"Exception during payment execution: {ex.Message}");
        }
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await _httpClient.PostAsync($"{_baseUrl}/v1/oauth2/token", requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get PayPal access token: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(content);

        return tokenResponse?.AccessToken;
    }
}
