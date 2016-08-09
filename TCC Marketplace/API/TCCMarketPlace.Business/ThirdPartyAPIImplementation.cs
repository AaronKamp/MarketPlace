using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TCCMarketPlace.Model.ExceptionHandlers;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Business
{
    public class ThirdPartyAPIImplementation
    {
        const string ExceptionSeperator = "*************************************************************************************************";
        public static async Task<string> GetValues(string address)
        {
            var result = await GetExternalResponse(address);
            return result;
        }

        public static async Task<string> PostValues(string address,string param)
        {
            var result = await UpdateValues(address,param);
            return result;
        }

        public static async Task<string> PostValues(string address, string body, AuthorizationHeader header)
        {
            var result = await UpdateValues(address, body, header);
            return result;
        }

        public static async Task<string> DeleteValues(string address)
        {
            var result = await GetDeleteResponse(address);
            return result;
        }

        private static async Task<string> GetDeleteResponse(string address)
        {
            HttpResponseMessage response = null;

            string result = string.Empty;
            try
            {
                var client = new HttpClient();
                response = await client.DeleteAsync(address);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                new Log4NetLogger().Log(ComposeExceptionLog(ex, address), ex, LogLevelEnum.Error);
                if (response != null)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            throw new BusinessException("Details Unavailable");
                           
                        case System.Net.HttpStatusCode.BadRequest:
                            throw new BusinessException("Invalid Request or a validation error occured");

                        case System.Net.HttpStatusCode.Unauthorized:
                            throw new BusinessException("User is not authorized to complete the process");

                        default: throw new BusinessException("Details Unavailable");
                    }
                }
            }
            return result;

        }
        private static async Task<string> GetExternalResponse(string address)
        {
            HttpResponseMessage response=null;
           
            string result = string.Empty;
            try
            {
                var client = new HttpClient();
                response = await client.GetAsync(address);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();                
            }                       
            catch (Exception ex)
            {
                new Log4NetLogger().Log(ComposeExceptionLog(ex, address), ex, LogLevelEnum.Error);
                if(response!=null)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound: throw new BusinessException("Details Unavailable");
                                                                 
                        case System.Net.HttpStatusCode.BadRequest: throw new BusinessException("Invalid Request or a validation error occured");
                                                                  
                        default: throw new BusinessException("Details Unavailable");
                    }
                }               
            }
            return result;
        }

        private static async Task<string> UpdateValues(string address,string param)
        {
            HttpResponseMessage response = null;
            string result = string.Empty;
            try
            {
                var client = new HttpClient();
                HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
                response = await client.PostAsync(address, contentPost);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                new Log4NetLogger().Log(ComposeExceptionLog(ex, address), ex, LogLevelEnum.Error);
                if (response != null)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            throw new BusinessException("Record cannot be found");
                        case System.Net.HttpStatusCode.BadRequest:
                            throw new BusinessException("Invalid Request or a validation error occured");
                        default: throw new BusinessException("Internal Server error");
                    }
                }

            }
            return result;
        }

        private static async Task<string> UpdateValues(string address, string body, AuthorizationHeader header)
        {
            HttpResponseMessage response = null;
            string result = string.Empty;
            try
            {
                var client = new HttpClient();
                HttpContent contentPost = new StringContent(body, Encoding.UTF8, header.Content_Type);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Scheme.ToString(), header.Parameter);
                response = await client.PostAsync(address, contentPost);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                new Log4NetLogger().Log(ComposeExceptionLog(ex, address), ex, LogLevelEnum.Error);
                if (response != null)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            throw new BusinessException("Record cannot be found");
                        case System.Net.HttpStatusCode.BadRequest:
                            throw new BusinessException("Invalid Request or a validation error occured");
                        default: throw new BusinessException("Internal Server error");
                    }
                }

            }
            return result;
        }

        public static async Task<string> GetBearerToken(string address, string body, AuthorizationHeader header)
        {
            HttpResponseMessage response = null;
            string result = string.Empty;
            try
            {
                var client = new HttpClient();
                var requestToken = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(address),
                    Content = new StringContent("grant_type=client_credentials")
                };

                requestToken.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                requestToken.Headers.TryAddWithoutValidation("Authorization", String.Format("Basic {0}", header.Parameter));

                response = await client.SendAsync(requestToken);
                response.EnsureSuccessStatusCode();
                var bearerData = await response.Content.ReadAsStringAsync();
                result = JObject.Parse(bearerData)["access_token"].ToString();
            }
            catch (Exception ex)
            {
                new Log4NetLogger().Log(ComposeExceptionLog(ex, address), ex, LogLevelEnum.Error);
                if (response != null)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            throw new BusinessException("Record cannot be found");
                        case System.Net.HttpStatusCode.BadRequest:
                            throw new BusinessException("Invalid Request or a validation error occured");
                        default: throw new BusinessException("Internal Server error");
                    }
                }

            }
            return result;
        }
        private static string ComposeExceptionLog(Exception ex, string address)
        {
            Guid execptionIdentifier = Guid.NewGuid();
            var sbErrorLog = new StringBuilder();

            sbErrorLog.AppendLine(ExceptionSeperator);
            sbErrorLog.AppendLine(execptionIdentifier.ToString());
            sbErrorLog.AppendLine("Third Party API call error");
            sbErrorLog.AppendLine("Error occurred for request- " + address);
            sbErrorLog.AppendLine("Exception- " + ex.Message);
            sbErrorLog.AppendLine(ExceptionSeperator);

            return sbErrorLog.ToString();
        }
    }
}
