using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace WebApp.Code
{
    public class ApiRequest
    {
        private readonly IConfigurationSection _apiUrl;
        private HttpClient _httpClient;

        public ApiRequest(IConfigurationSection apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<HttpResponseMessage> ExecuteRequest(HttpRequest httpRequest, HttpResponse httpResponse, HttpMethod httpMethod, string url, object bodyData)
        {
            _httpClient = new HttpClient();
            var request = new HttpRequestMessage(httpMethod, _apiUrl.Value + url);
            if (!(url == "Login/Login" || (url == "Users" && httpMethod == HttpMethod.Post)))
            {
                var token = httpRequest.Cookies.FirstOrDefault(x => x.Key == "token");
                if (!string.IsNullOrEmpty(token.Value))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                }
                else
                {
                    return null;
                }
            }
            if (bodyData != null)
            {
                var content = new StringContent(JsonConvert.SerializeObject(bodyData), Encoding.UTF8, MediaTypeNames.Application.Json);
                request.Content = content;
            }
            var result = await _httpClient.SendAsync(request);
            if (result == null || result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                httpRequest.Cookies.ToList().ForEach(cookie => httpResponse.Cookies.Delete(cookie.Key));
                httpResponse.Redirect("/Login");
            }
            return result;
        }
    }
}
