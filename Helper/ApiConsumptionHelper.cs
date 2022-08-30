using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TravelRequisitionSystem.Enums;
using TravelRequisitionSystem.Models;


namespace TravelRequisitionSystem.Helper
{
    public class ApiConsumptionHelper
    {
        public async Task<ApiResponse<T>> ConsumeAPI<T>(string url, object requestContent, ApiMethod apiMethod, bool isRequiredAuthentication, AuthenticationType? authenticationType, string authenticationToken)
        {
            string msg = "";

            ApiResponse<T> response = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 10, 0); //10 minutes               

                client.DefaultRequestHeaders.Clear();

                if (isRequiredAuthentication)
                {
                    if (authenticationType.HasValue && !string.IsNullOrEmpty(authenticationToken))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Enum.GetName(typeof(AuthenticationType), authenticationType), authenticationToken);
                    }
                }
               
                client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("NoCache");
              
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                
                HttpResponseMessage res = null;

                string serializedContent = "";

                StringContent stringContent = null;

                if (apiMethod == ApiMethod.GET)
                {
                    res = await client.GetAsync(url);
                }
                else
                {
                    if (requestContent != null)
                    {
                        serializedContent = JsonConvert.SerializeObject(requestContent);

                        stringContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

                        res = await client.PostAsync(url, stringContent);
                    }
                    else
                    {
                        res = await client.PostAsync(url, null);
                    }
                }
                
                if (res.IsSuccessStatusCode)
                {                   

                    var responseInString = await res.Content.ReadAsStringAsync();

                    if (Utility.IsJsonValid(responseInString))
                    {
                        response.TProperty = JsonConvert.DeserializeObject<T>(responseInString.Substring(1, responseInString.Length - 2));
                    }
                }             
                
            }

            return response;
        }



    }
}
