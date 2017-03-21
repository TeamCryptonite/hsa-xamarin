using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HsaServiceDtos;
using Xamarin.Forms;
using Plugin.Media;

namespace HSAManager
{
	public partial class OCREntry : ContentPage
	{
        private BizzaroClient client = new BizzaroClient();

		public OCREntry()
		{
			InitializeComponent();
		}

		private async void TakePhotoButton_OnClicked(object sender, System.EventArgs e)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":(No camera available.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(
				new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					SaveToAlbum = true
				});
			if (file == null)
				return;
			PathLabel.Text = file.AlbumPath;

			MainImage.Source = ImageSource.FromStream(() =>
			{
				var stream = file.GetStream();
				file.Dispose();
				return stream;
			});

            // Debug Testing
            var client = new HttpClient();

		    var streamFromUrl =
		        await client.GetStreamAsync(
		            "http://i1.wp.com/savewithcouponing.files.wordpress.com/2012/07/walmart-receipt-7-4-12.jpg?w=720");
            StartOcrProcess(file.GetStream());
		}

		private async void PickPhotoButton_OnClicked(object sender, System.EventArgs e)
		{
			await CrossMedia.Current.Initialize();
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await DisplayAlert("Oops", "Pick photo is not supported !", "OK");
				return;
			}
			var file = await CrossMedia.Current.PickPhotoAsync();
			if (file == null)
				return;

			PathLabel.Text = "Photo Path" + file.Path;
			MainImage.Source = ImageSource.FromStream(() =>
			{
				var stream = file.GetStream();
				file.Dispose();
				return stream;
			});
             
		}

	    private async void StartOcrProcess(Stream image)
	    {
            var newReceipt = await client.Receipts.PostNewReceipt(new ReceiptDto());
            await client.Receipts.UploadReceiptImage(newReceipt.ReceiptId, image);
            
            string ocrUrl = await client.Receipts.OcrReceiptImage(newReceipt.ReceiptId);
	        Tuple<string, ReceiptDto> ocrResult = await client.Receipts.CheckOcrResults(ocrUrl);
	        do
	        {
	            OcrStatus.Text = ocrResult.Item1;
                await Task.Delay(500);
                ocrResult = await client.Receipts.CheckOcrResults(ocrUrl);
            } while (ocrResult.Item2 == null);
	        OcrStatus.Text = ocrResult.Item1;
	        ListView.ItemsSource = ocrResult.Item2.LineItems;

	    }
	}
}