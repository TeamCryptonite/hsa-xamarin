using Xamarin.Forms;

namespace HSAManager
{
	public partial class HSAManagerPage : ContentPage
	{
		void Handle_Clicked_Receipt(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new receiptVault());
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

		public HSAManagerPage()
		{
			InitializeComponent();
		}
	}
}
