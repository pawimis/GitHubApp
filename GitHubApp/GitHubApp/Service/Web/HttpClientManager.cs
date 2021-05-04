using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GitHubApp.Service.Web
{
    public class HttpClientManager
    {
        #region Members

        private readonly SemaphoreSlim _asyncLock;
        private static Dictionary<string, HttpClient> HttpClientDict;
        #endregion

        internal HttpClientManager()
        {
            _asyncLock = new SemaphoreSlim(1, 1);
        }

        #region  Methods

        private HttpClient CreateHttpClient(string url)
        {

            HttpClient _httpClient = new HttpClient()
            {
                BaseAddress = new UriBuilder(new Uri(url)).Uri
            };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _httpClient;
        }
        #endregion

        #region Internal Methods

        internal async Task<HttpClient> GetHttpClient(string baseUrl)
        {
            await _asyncLock.WaitAsync();
            try
            {
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new Exception("Empty base URL");
                }

                if (HttpClientDict == null)
                {
                    HttpClientDict = new Dictionary<string, HttpClient>();
                }

                if (HttpClientDict.ContainsKey(baseUrl))
                {
                    if (HttpClientDict[baseUrl] == null)
                    {
                        HttpClientDict.Add(baseUrl, CreateHttpClient(baseUrl));
                    }
                }
                else
                {
                    HttpClientDict.Add(baseUrl, CreateHttpClient(baseUrl));
                }

                return HttpClientDict[baseUrl];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _asyncLock.Release();
            }
        }


        internal string GetEnpointPath(string baseURL, string endpointName, List<KeyValuePair<string, string>> GetQueryParams = null)
        {

            UriBuilder uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Path += "/" + endpointName;
            if (GetQueryParams == null)
            {
                return uriBuilder.Uri.ToString();
            }

            System.Collections.Specialized.NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (KeyValuePair<string, string> pair in GetQueryParams)
            {
                parameters[pair.Key] = pair.Value;
            }
            uriBuilder.Query = parameters.ToString();
            if (uriBuilder.Query.Contains("%2c"))
            {
                uriBuilder.Query = uriBuilder.Query.Replace("%2c", ",");
            }

            return uriBuilder.Uri.ToString();

        }


        #endregion
    }
}
