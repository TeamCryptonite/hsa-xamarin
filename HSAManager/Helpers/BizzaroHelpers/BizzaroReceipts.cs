using System;
using System.IO;
using System.Threading.Tasks;
using HsaServiceDtos;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class BizzaroReceipts : AbstractBizzaroActions
    {
        public BizzaroReceipts()
        {
        }

        public BizzaroReceipts(string baseUrl) : base(baseUrl)
        {
        }

        public Paginator<ReceiptDto> GetListOfReceipts(string query = null)
        {
            var request = new RestRequest("receipts", Method.GET);
            if (!string.IsNullOrWhiteSpace(query))
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

            var status = await CallBizzaro(request, updatedReceipt);

            return status;
        }

        public async Task<StatusOnlyDto> UploadReceiptImage(int receiptId, Stream imageStream,
            string imageExtension = "jpg")
        {
            var bizzaroRequest = new RestRequest("receipts/{id}/receiptimage", Method.POST);
            bizzaroRequest.AddUrlSegment("id", receiptId);
            bizzaroRequest.AddOrUpdateQueryParameter("imagetype", imageExtension);
            var bizzaroResponse = await CallBizzaro<JObject>(bizzaroRequest);

            var blobUrl = bizzaroResponse.GetValue("PictureUrl").Value<string>();
            var blobClient = new RestClient(blobUrl) {IgnoreResponseStatusCode = true};
            var blobRequest = new RestRequest(Method.PUT);

            var ms = new MemoryStream();
            await imageStream.CopyToAsync(ms);
            blobRequest.AddParameter("image/" + imageExtension, ms.ToArray(), ParameterType.RequestBody);
            blobRequest.AddHeader("x-ms-blob-type", "BlockBlob");

            var blobResponse = await blobClient.Execute(blobRequest);
            if (blobResponse.IsSuccess)
                return new StatusOnlyDto {StatusMessage = "Success"};

            return new StatusOnlyDto {StatusMessage = "Could Not Upload Receipt Image"};
        }
    }
}