using System;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager
{
    public class BizzaroReceipts : AbstractBizzaroActions
    {
        public BizzaroReceipts(string authToken, string baseUrl) : base(authToken, baseUrl)
        {
        }

        public Paginator<ReceiptDto> GetListOfReceipts()
        {
            var request = new RestRequest("receipts", Method.GET);
            return new Paginator<ReceiptDto>(authToken, baseUrl, request);
        }

        public ReceiptDto GetOneReceipt(int receiptId)
        {
            if (receiptId < 1)
                throw new Exception("Receipt ID must be greater than 0.");
            var request = new RestRequest($"receipts/{receiptId}", Method.GET);

            return CallBizzaro<ReceiptDto>(request);
        }

        public ReceiptDto PostNewReceipt(ReceiptDto receiptDto)
        {
            var request = new RestRequest("receipts", Method.POST);

            return CallBizzaro<ReceiptDto>(request, receiptDto);
        }
    }
}