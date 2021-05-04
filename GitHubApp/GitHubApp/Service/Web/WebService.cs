using GitHubApp.Utils;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GitHubApp.Service.Web
{
    public class WebService
    {
        #region Members
        private bool _requiresAuthorisation = true;
        private readonly HttpClientManager _httpClientManager;
        private string token;
        #endregion

        #region Properties
        protected bool RequiresAuthorisation
        {
            get => _requiresAuthorisation;
            set => _requiresAuthorisation = value;
        }
        public string Token { get => token; set => token = value; }
        #endregion Properties

        public WebService()
        {
            _httpClientManager = new HttpClientManager();
        }
        #region Protected Methods
        #region Get Methods
        protected async Task<ServiceStatusMessage> MakeGetRequest(string baseURL, string endpoint, List<KeyValuePair<string, string>> parameters = null, bool authorize = true)
        {
            ServiceStatusMessage responseMessage = new ServiceStatusMessage();
            RequiresAuthorisation = authorize;
            try
            {
                HttpClient client = await GetHttpClientAsync(baseURL);
                string url = _httpClientManager.GetEnpointPath(baseURL, endpoint, parameters);
                HttpResponseMessage response = await client.GetAsync(url);
                responseMessage = ResponseDataExtraction(response);
            }
            catch (Exception ex)
            {

                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        protected async Task<ServiceStatusMessage<TEntity>> MakeGetRequestReturnObject<TEntity>(string baseURL, string endpoint,
            List<KeyValuePair<string, string>> parameters = null, bool authorize = true)
        {
            ServiceStatusMessage<TEntity> responseMessage = new ServiceStatusMessage<TEntity>();
            RequiresAuthorisation = authorize;

            try
            {
                string url = string.Empty;
                HttpClient client = await GetHttpClientAsync(baseURL);
                if (endpoint == null && parameters == null)
                {
                    url = baseURL;
                }
                else
                {
                    url = _httpClientManager.GetEnpointPath(baseURL, endpoint, parameters);
                }

                HttpResponseMessage response = await client.GetAsync(url);
                responseMessage = await ResponseDataExtraction<TEntity>(response);
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }



        #endregion Get Methods

        #region General Methods
        protected virtual async Task<HttpClient> GetHttpClientAsync(string baseURL)
        {
            HttpClient client = await _httpClientManager.GetHttpClient(baseURL);

            if (RequiresAuthorisation)
            {
                Token = await new SecureStorageService().Get(StorageKeys.USER_TOKEN);
                if (string.IsNullOrWhiteSpace(Token))
                {
                    throw new Exception("Token empty, fix it pls");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", Token);
            }

            return client;
        }

        #endregion General Methods
        #endregion Protected Methods

        #region  Methods

        public async Task<ServiceStatusMessage> PostRequestAsync(string baseURL, string endpoint, string jsonContent, bool authorize = true)
        {
            ServiceStatusMessage responseMessage = new ServiceStatusMessage();
            RequiresAuthorisation = authorize;

            try
            {
                HttpClient client = await GetHttpClientAsync(baseURL);
                string url = _httpClientManager.GetEnpointPath(baseURL, endpoint);
                StringContent bodyContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, bodyContent);
                responseMessage = ResponseDataExtraction(response);
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }



        public async Task<ServiceStatusMessage<TEntity>> PostRequestReturnObjectAsync<TEntity>(string baseURL, string endpoint, string jsonContent, bool authorize = true)
        {
            ServiceStatusMessage<TEntity> responseMessage = new ServiceStatusMessage<TEntity>();
            RequiresAuthorisation = authorize;
            try
            {
                HttpClient client = await GetHttpClientAsync(baseURL);
                string url = _httpClientManager.GetEnpointPath(baseURL, endpoint);
                StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, httpContent);
                responseMessage = await ResponseDataExtraction<TEntity>(response);
            }
            catch (Exception ex)
            {

                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }



        public async Task<ServiceStatusMessage<TEntity>> PutRequestAsync<TEntity>(string baseURL, string endpoint, string jsonContent, bool authorize = true, string mediaType = "application/json")
        {
            ServiceStatusMessage<TEntity> responseMessage = new ServiceStatusMessage<TEntity>();
            RequiresAuthorisation = authorize;
            try
            {
                HttpClient client = await GetHttpClientAsync(baseURL);
                string url = _httpClientManager.GetEnpointPath(baseURL, endpoint);
                HttpResponseMessage response = null;
                StringContent bodyContent = new StringContent(jsonContent, Encoding.UTF8, mediaType);
                response = await client.PutAsync(url, bodyContent);
                responseMessage = await ResponseDataExtraction<TEntity>(response);
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }

            return responseMessage;
        }
        private static ServiceStatusMessage ResponseDataExtraction(HttpResponseMessage response)
        {
            return new ServiceStatusMessage
            {
                StatusCode = response.StatusCode.ToString(),
                DidSucceed = response.IsSuccessStatusCode,
                ResponseMessage = response.ReasonPhrase
            };
        }
        private static async Task<ServiceStatusMessage<TEntity>> ResponseDataExtraction<TEntity>(HttpResponseMessage response)
        {
            ServiceStatusMessage<TEntity> responseMessage = new ServiceStatusMessage<TEntity>
            {
                StatusCode = response.StatusCode.ToString(),
                DidSucceed = response.IsSuccessStatusCode,
                ResponseMessage = response.ReasonPhrase
            };
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    responseMessage.Entity = JsonConvert.DeserializeObject<TEntity>(content);
                }
            }
            else
            {
                if (response.Content != null)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        responseMessage.Content = content;
                    }
                }
            }
            return responseMessage;
        }

        #endregion  Methods
    }
}
