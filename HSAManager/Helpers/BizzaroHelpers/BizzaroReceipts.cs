using System;
using System.Collections.Generic;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager
{
    public class BizzaroReceipts : AbstractBizzaroActions
    {
        public BizzaroReceipts(string authToken, string baseUrl) : base(authToken, baseUrl)
        {
        }

        public IEnumerable<ReceiptDto> GetListOfReceipts()
        {
            var request = new RestRequest("receipts", Method.GET);
            request.AddParameter("Authorization", "Bearer " + authToken, ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            var response = client.Execute<IEnumerable<ReceiptDto>>(request).Result;
            return response.Data;
        }

        public ReceiptDto GetOneReceipt(int receiptId)
        {
            if (receiptId < 1)
                throw new Exception("Receipt ID must be greater than 0.");
            var request = new RestRequest($"receipts/{receiptId}", Method.GET);
            request.AddParameter("Authorization", "Bearer " + authToken, ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var response = client.Execute<ReceiptDto>(request).Result;
            return response.Data;
        }

        public ReceiptDto PostNewReceipt(ReceiptDto receiptDto)
        {
            var request = new RestRequest("receipts", Method.POST);
            request.AddParameter("Authorization", "Bearer " + authToken, ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            request.AddParameter("application/json", receiptDto, ParameterType.RequestBody);

            var response = client.Execute<ReceiptDto>(request).Result;
            return response.Data;
        }
    }
}