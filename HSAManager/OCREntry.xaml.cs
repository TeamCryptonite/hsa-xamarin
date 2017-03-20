using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Media;

namespace HSAManager
{
	public partial class OCREntry : ContentPage
	{
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
	}
}