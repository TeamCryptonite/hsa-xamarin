using System;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Handle_Clicked_Receipt(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReceiptVaultDashboard());
        }

        private void Handle_Clicked_Charts(object sender, EventArgs e)
        {
            Navigation.PushAsync(new data());
        }

        private void Handle_Clicked_Products(object sender, EventArgs e)
        {
            Navigation.PushAsync(new products());
        }

        private void Handle_SL(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShoppingListLists());
        }

        private async void handleLogout(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Logout", "You will be logged out", "Ok", "Cancel");
            if (answer)
            {
                App.PCApplication.UserTokenCache.Clear(App.PCApplication.ClientId);
                await Navigation.PopToRootAsync();
            }
        }
    }
}