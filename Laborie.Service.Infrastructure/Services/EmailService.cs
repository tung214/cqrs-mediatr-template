using Laborie.Service.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Laborie.Service.Infrastructure.Services
{
    public class EmailService(ILogger<EmailService> logger
        , IConfiguration configuration
    )
        : IEmailService
    {
        public async Task SendMail(string to, string subject, string body)
        {
            try
            {
                HttpClient _httpClient = new();
                var uri = configuration.GetSection("EmailService:Uri").Value;

                var data = new
                {
                    to = new List<string> { to },
                    subject,
                    message = body
                };
                var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    logger.LogError("Send mail to {to} fail: {response}", to, responseContent);
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Send mail to {to} fail: {exception}", to, ex.Message);
            }

        }
    }
}