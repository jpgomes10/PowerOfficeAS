using API.Models.OrganizationInfos;
using API.Models.OrganizationInfos.ValueObjects;
using API.Utils;
using System.Net;
using System.Text.Json;

namespace API.Services
{
    public class BrregClient : IBrregClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<BrregClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly Lazy<SemaphoreSlim> _semaphore = new(() => new SemaphoreSlim(2, 2));

        public BrregClient(HttpClient httpClient,IConfiguration configuration, ILogger<BrregClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration["BrregClient"];
            _logger = logger;
        }
        /// <summary>
        /// Retrieves information about an organization using the provided organization number.
        /// </summary>
        /// <param name="orgNo">The organization number to retrieve information for.</param>
        /// <returns>
        /// Returns an instance of the OrganizationInfo class containing information about the organization.
        /// If the request fails, returns null.
        /// </returns>
        public async Task<OrganizationInfo> GetOrganization(string orgNo)
        {
            await _semaphore.Value.WaitAsync();
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/enheter/{orgNo}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var organization = JsonSerializer.Deserialize<OrganizationInfo>(content);
                    organization.Status = StatusEnum.Active;
                    if (organization.Konkurs)
                        organization.Status = StatusEnum.Bankrupted;
                    if (organization.Naeringskode1 == null)
                        organization.Status = StatusEnum.Deleted;
                    return organization;
                }
                else
                {
                    var logLine = $"{orgNo} - {response.StatusCode}\n";
                    _logger.LogError(logLine);
                }
                return null;
            }
            finally
            {
                _semaphore.Value.Release();
            }
        }
    }
}
