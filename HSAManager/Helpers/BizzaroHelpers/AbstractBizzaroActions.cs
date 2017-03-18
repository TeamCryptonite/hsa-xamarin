using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using HsaServiceDtos;

// ReSharper disable once CheckNamespace
namespace HSAManager
{
    public abstract class AbstractBizzaroActions
    {
        protected readonly string authToken;
        protected readonly RestClient client;

        protected AbstractBizzaroActions(string baseUrl = "https://bizzaro.azurewebsites.net/api")
        {
            this.authToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IklkVG9rZW5TaWduaW5nS2V5Q29udGFpbmVyLnYyIn0.eyJleHAiOjE0ODk4Njg5NzEsIm5iZiI6MTQ4OTg2NTM3MSwidmVyIjoiMS4wIiwiaXNzIjoiaHR0cHM6Ly9sb2dpbi5taWNyb3NvZnRvbmxpbmUuY29tL2ZlYTU2ZDZkLTk2ZjEtNDIzNi1iN2QzLTFjYWFjMjZlYTM0ZC92Mi4wLyIsInN1YiI6ImM4NWQ0MTdjLWM3ZTYtNDExNy05ZDdhLTI3YWU2ZjU3Zjk3NSIsImF1ZCI6IjZhOTljNTdlLTM0ODktNGU5MS1hMzk4LWU5M2Y4Y2VjNGFmOCIsIm5vbmNlIjoiZGVmYXVsdE5vbmNlIiwiaWF0IjoxNDg5ODY1MzcxLCJhdXRoX3RpbWUiOjE0ODk4NjUzNzEsIm9pZCI6ImM4NWQ0MTdjLWM3ZTYtNDExNy05ZDdhLTI3YWU2ZjU3Zjk3NSIsImZhbWlseV9uYW1lIjoiSHV0c29uIiwicG9zdGFsQ29kZSI6IjY1MjMzIiwiZ2l2ZW5fbmFtZSI6IlBlYXJzZSIsIm5hbWUiOiJQZWFyc2UiLCJlbWFpbHMiOlsicGVhcnNlLjIwMDhAZ21haWwuY29tIl0sInRmcCI6IkIyQ18xX0hTQV9TaWduVXBfU2lnbkluX0RlZmF1bHQifQ.sp_8TvuYxSBVJNyMmOQfsJaJisEqfyFzl4oo8Unp2QT0XHpN9kulW04qF2uHlayRjpVhlPkb31irsV35uUi2A-BHDhRNAJ0j-xcEy4xqaKZhwNZsYQ9_sR_czoVrfHjs1LnXvLcPEtnXXTKJNxeqJF91XK-CBjvlobHLNOd7XPK0OrLay4NDvsoifkvPiIgXKlK9NtH0CCsql24Yr-FrTTYY8mGPcVq6fJHqmRFB1ImK9RcZszRDrIZ2IrIrIbu3P01DxqETrQ3nLuuA4zSUIWlShmzsnmY7WD7cPxEMxWUG4eQUkEDYeagTVhaT4o-qkG5QT2IeGZyhu4LGL66zWA"; 
                             //Application.Current.Properties["authKey"].ToString();
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
                throw new Exception("Could not process HTTP call. " + response.StatusDescription + ". " + response.Data);

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
    }
}