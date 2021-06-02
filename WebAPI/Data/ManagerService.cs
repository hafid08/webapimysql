using Loyalto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class ManagerService : IManagerService
    {
        private readonly HttpClient httpClient;
        private readonly string AUTH = "Authorization";
        private readonly IConfiguration configuration;
        private readonly string url;
        public ManagerService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.url = configuration["ValutaUrl"];
        }

        public async Task<string> Invoke(string Method, string Body, string Token, string Key, int Id = 0)
        {
            var uri = new Uri(url);
            if (Id > 0)
            {
                uri = new Uri(url + Id);
            }
            //_httpClient.BaseAddress = new Uri(Uri);
            //int _TimeoutSec = 90;
            //_httpClient.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            string _ContentType = "application/json";
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            var _CredentialBase64 = Token;
            httpClient.DefaultRequestHeaders.Add(AUTH, String.Format("{0}", _CredentialBase64));
            var _UserAgent = "d-fens Http_httpClientient";
            // You can actually also set the User-Agent via a built-in property
            httpClient.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
            httpClient.DefaultRequestHeaders.Add("api-key", Key);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // _httpClient.DefaultRequestHeaders.Add("Content-Type", _ContentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."

            HttpResponseMessage response;
            var _Method = new HttpMethod(Method);

            switch (_Method.ToString().ToUpper())
            {
                case "GET":
                case "HEAD":
                    // synchronous request without the need for .ContinueWith() or await
                    response = httpClient.GetAsync(uri).Result;
                    break;
                case "POST":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _Body = new StringContent(Body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = httpClient.PostAsync(uri, _Body).Result;
                    }
                    break;
                case "PUT":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _Body = new StringContent(Body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = httpClient.PutAsync(uri, _Body).Result;
                    }
                    break;
                case "DELETE":
                    response = httpClient.DeleteAsync(uri).Result;
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            // either this - or check the status to retrieve more information
            response.EnsureSuccessStatusCode();
            // get the rest/content of the response in a synchronous way
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
