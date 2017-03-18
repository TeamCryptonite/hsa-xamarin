using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager
{
    public class BizzaroReceipts : AbstractBizzaroActions
    {
        public BizzaroReceipts() { }
        public BizzaroReceipts(string baseUrl) : base(baseUrl) { }

        public Paginator<ReceiptDto> GetListOfReceipts(string query = null)
        {
            var request = new RestRequest("receipts", Method.GET);
            if(!string.IsNullOrWhiteSpace(query))
                request.AddOrUpdateQueryParameter("query", query);

            return new Paginator<ReceiptDto>(this, request);
        }

        public async Task<ReceiptDto> GetOneReceipt(int receiptId)
        {
            if (receiptId < 1)
                throw new Exception("Receipt ID must be greater than 0.");
            var request = new RestRequest($"receipts/{receiptId}", Method.GET);

            return await CallBizzaro<ReceiptDto>(request);
        }

        public async Task<ReceiptDto> PostNewReceipt(ReceiptDto receiptDto)
        {
            var request = new RestRequest("receipts", Method.POST);

            return await CallBizzaro<ReceiptDto>(request, receiptDto);
        }

        public async Task<StatusOnlyDto> UpdateReceipt(int receiptId, ReceiptDto updatedReceipt)
        {
            var request = new RestRequest("receipts/{id}", Method.PATCH);
            request.AddUrlSegment("id", receiptId);

            StatusOnlyDto status = await CallBizzaro(request, updatedReceipt);

            return status;
        }
    }
}