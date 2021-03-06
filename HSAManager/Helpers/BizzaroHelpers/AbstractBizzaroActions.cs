﻿using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace

namespace HSAManager.Helpers.BizzaroHelpers
{
    public abstract class AbstractBizzaroActions
    {
        protected readonly string authToken;
        protected readonly RestClient client;

        protected AbstractBizzaroActions(string baseUrl = "https://bizzaro.azurewebsites.net/api")
        {
            authToken = Application.Current.Properties["authKey"].ToString();
            client = new RestClient(baseUrl) {IgnoreResponseStatusCode = true};
        }

        private void AddHeadersToRequest(IRestRequest request, object bodyData = null)
        {
            request.AddParameter("Authorization", "Bearer " + authToken, ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            if (bodyData != null)
                request.AddParameter("application/json", bodyData, ParameterType.RequestBody);
        }

        public async Task<T> CallBizzaro<T>(IRestRequest request, object bodyData = null)
        {
            AddHeadersToRequest(request, bodyData);
            
            var response = await client.Execute<T>(request);

            if (!response.IsSuccess)
                throw new Exception("Could not process HTTP call. " + response.StatusDescription + ". " +
                                    response.Content);

            return response.Data;
        }

       

        public async Task<StatusOnlyDto> CallBizzaro(IRestRequest request, object bodyData = null)
        {
            var statusReturn = new StatusOnlyDto();
            AddHeadersToRequest(request, bodyData);

            var response = await client.Execute(request);

            if (!response.IsSuccess)
                statusReturn.StatusMessage = "Could not process HTTP call. " + response.StatusDescription;
            else
                statusReturn.StatusMessage = "Success";

            return statusReturn;
        }

        public async Task<JArray> CallBizzaroJArray(IRestRequest request, object bodyData = null)
        {
            AddHeadersToRequest(request, bodyData);

            var response = await client.Execute(request);

            if (!response.IsSuccess)
                throw new Exception("Could not process HTTP call. " + response.StatusDescription + ". " +
                                    response.Content);

            return JArray.Parse(response.Content);

            //return statusReturn;
        }
    }
}