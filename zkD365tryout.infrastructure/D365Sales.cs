using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using zkD365tryout.infrastructure.Interface;


namespace zkD365tryout.infrastructure




{
    public class D365Sales : ID365Sales
    {

        private readonly HttpClient _httpClient;

        private readonly string _clientId;

        private readonly string _clientSecret;

        private readonly string _tenantId;

        private readonly string _crmUrl;


        public D365Sales(HttpClient httpClient, IConfiguration configuration)

        {

            _httpClient = httpClient;

            _clientId = configuration["Authentication:ClientId"];

            _clientSecret = configuration["Authentication:ClientSecret"];

            _tenantId = configuration["Authentication:TenantId"];

            _crmUrl = configuration["Authentication:DynamicsCrmUrl"];

            _httpClient = httpClient;
        }



        public async Task<string> GetAccessTokenAsync()

        {

            var app = ConfidentialClientApplicationBuilder.Create(_clientId)

            .WithClientSecret(_clientSecret)

            .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))

            .Build();



            var result = await app.AcquireTokenForClient(new[] { $"{_crmUrl}/.default" }).ExecuteAsync();

            return result.AccessToken;

        }

        public async Task<string> CreateD365SalesAccountName(string token,string name) {

            // Set default headers
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            var requestUrl = $"{_crmUrl}/api/data/v9.2/accounts";
            var body = new
            {
                name = name
            };

            var json = JsonSerializer.Serialize(body);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(requestUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"Failed to create account. Status: {response.StatusCode}, Content: {errorContent}");
            }

            return response.Headers.Location.ToString();
        }

    }
}
