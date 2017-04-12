using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HsaServiceDtos;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HSAManager
{
	public partial class OCREntry : ContentPage
	{
        private BizzaroClient client = new BizzaroClient();

		public OCREntry()
		{
			InitializeComponent();
		}

        private async void TakePhotoButton_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":(No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true
                });
            if (file == null)
                return;

            StartOcrProcess(file.GetStream());

            // Dispose of file
            file.Dispose();
		    
		}

        private async void PickPhotoButton_OnClicked(object sender, EventArgs e)
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

            StartOcrProcess(file.GetStream());

            // Dispose of file
            file.Dispose();

        }

	    private async void StartOcrProcess(Stream image)
	    {
	        OcrActivityIndicator.IsRunning = true;
            var status = await client.Receipts.OcrNewReceiptImage(image);

	        if (status.StatusMessage != "Success")
	        {
	            await DisplayAlert("Server Message", status.StatusMessage, "Ok");
	        }
	        await Navigation.PopAsync();
	    }
	}
}