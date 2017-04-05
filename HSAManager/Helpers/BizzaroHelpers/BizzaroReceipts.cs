using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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

        public async Task<StatusOnlyDto> DeleteReceipt(int receiptId)
        {
            var request = new RestRequest($"receipts/{receiptId}", Method.DELETE);

            return await CallBizzaro(request);
        }

        public async Task<LineItemDto> AddReceiptListItem(int receiptId,
            LineItemDto newReceiptListItem)
        {
            var request = new RestRequest($"receipts/{receiptId}/lineitems", Method.POST);

            return await CallBizzaro<LineItemDto>(request, newReceiptListItem);
        }

        public async Task<StatusOnlyDto> DeleteShoppingListItem(int receiptId, int receiptListItemId)
        {
            var request = new RestRequest($"receipts/{receiptId}/lineitems/{receiptListItemId}",
                Method.DELETE);

            return await CallBizzaro(request);
        }

        public async Task<StatusOnlyDto> UpdateShoppingListItem(int receiptId,
            LineItemDto updatedLineItem)
        {
            if (updatedLineItem.LineItemId < 1)
                return new StatusOnlyDto { StatusMessage = "Receipt Line Items must include an ID" };
            var request =
                new RestRequest(
                    $"receipts/{receiptId}/lineitems/{updatedLineItem.LineItemId}",
                    Method.PATCH);

            return await CallBizzaro(request, updatedLineItem);
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

        public async Task<string> OcrNewReceiptImage(Stream image)
        {
            var newReceipt = await PostNewReceipt(new ReceiptDto());
            await UploadReceiptImage(newReceipt.ReceiptId, image);
            return await OcrExistingReceiptImage(newReceipt.ReceiptId);
        }

        public async Task<string> OcrExistingReceiptImage(int receiptId)
        {

            var request = new RestRequest("receipts/{id}/receiptimageocr", Method.POST);
            request.AddUrlSegment("id", receiptId);
            return await CallBizzaro<string>(request);
        }

        //public async Task<Tuple<string, ReceiptDto>> CheckOcrResults(string ocrUrl)
        //{
        //    string returnString = "";
        //    ReceiptDto returnReceipt = null;   
        //    var httpClient = new HttpClient();
        //    var response = await httpClient.GetStringAsync(ocrUrl);
        //    var responseObj = JObject.Parse(response);

        //    if (responseObj.GetValue("Status") != null)
        //        returnString = responseObj.GetValue("Status").Value<string>();

        //    if (responseObj.GetValue("LineItems") != null)
        //    {
        //        var lineItems = ((JArray)responseObj.GetValue("LineItems")).ToObject<List<LineItemDto>>();
        //        var receipt = new ReceiptDto() {LineItems = new List<LineItemDto>()};

        //        foreach (var lineItem in lineItems)
        //        {
        //            receipt.LineItems.Add(lineItem);
        //        }

        //        returnReceipt = receipt;
        //    }

        //    return new Tuple<string, ReceiptDto>(returnString, returnReceipt);
        //}
    }
}