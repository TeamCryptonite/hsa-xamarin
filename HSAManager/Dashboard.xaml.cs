using Xamarin.Forms;

namespace HSAManager
{
	public partial class Dashboard : ContentPage
	{
		public Dashboard()
		{

			InitializeComponent();
		}

		void Handle_Clicked_Receipt(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new ReceiptVaultDashboard());
		}

		void Handle_Clicked_Charts(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new data());
		}

		void Handle_Clicked_Management(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new balanceManagement());
		}

		void Handle_Clicked_Products(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new products());
		}

		void Handle_SL(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new shoppingList());
		}

		async void handleLogout(object sender, System.EventArgs e)
		{
			var answer = await DisplayAlert("Question?", "You will be logged out", "Ok", "Cancel");
			if (answer == true)
			{
				App.PCApplication.UserTokenCache.Clear(App.PCApplication.ClientId);
				await Navigation.PopToRootAsync();
			}
		}
	}
}