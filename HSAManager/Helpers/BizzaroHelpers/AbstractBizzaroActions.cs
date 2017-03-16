using System;
using System.Net;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace HSAManager
{
    public abstract class AbstractBizzaroActions
    {
        protected readonly string authToken;
        protected readonly RestClient client;

        protected AbstractBizzaroActions(string authToken, string baseUrl)
        {
            this.authToken = authToken;
            client = new RestClient(baseUrl);
            client.IgnoreResponseStatusCode = true;
        }

        public T CallBizzaro<T>(IRestRequest request, object bodyData = null)
        {
            request.AddParameter("Authorization", "Bearer " + authToken, ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            if (bodyData != null)
                request.AddParameter("application/json", bodyData, ParameterType.RequestBody);

            var response = client.Execute<T>(request).Result;
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                throw new Exception("Could not process HTTP call. " + response.StatusDescription);

            return response.Data;
        }
    }
}