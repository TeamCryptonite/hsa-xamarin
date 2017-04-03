using Microsoft.Identity.Client;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class App : Application
    {
        // app coordinates
        public static string ClientID = "6a99c57e-3489-4e91-a398-e93f8cec4af8";
        public static string[] Scopes = {ClientID};


        public static string SignUpSignInpolicy = "B2C_1_HSA_SignUp_SignIn_Default";
        public static string ResetPasswordpolicy = "B2C_1_HsaPasswordResetDefault";

        public static string Authority =
            "https://login.microsoftonline.com/cryptonitehsaservice.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1_HSA_SignUp_SignIn_Default";

        public static string APIbaseURL = "https://bizzaro.azurewebsites.net";

        public App()
        {
            //InitializeComponent();

            //MainPage = new NavigationPage(new HSAManagerPage())
            //{
            //	BarBackgroundColor = Color.FromHex("#00cccc"),
            //	BarTextColor = Color.White
            //};


            PCApplication = new PublicClientApplication(Authority, ClientID);
            //{
            //    RedirectUri = redirectURI,
            //};

            // The root page of your application

            //Actual
            MainPage = new NavigationPage(new StartScreen());

            //Testing Purposes
            //MainPage = new NavigationPage(new HSAManager.Dashboard());
        }

        // the app
        public static PublicClientApplication PCApplication { get; set; }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}