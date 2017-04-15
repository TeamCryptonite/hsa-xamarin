using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Charts : ContentPage
    {
        private readonly BizzaroClient client = new BizzaroClient();

        public Charts()
        {
            InitializeComponent();

            CreateChartHtml();

            //ChartBrowser.Source = "https://developer.xamarin.com/guides/xamarin-forms/user-interface/webview/";
        }

        private async void CreateChartHtml()
        {
            ActivityIndicator.IsVisible = true;
            ActivityIndicator.IsRunning = true;
            var htmlSource = new HtmlWebViewSource();

            var csJsonData = await client.Aggregate.GetSpendingOverTime();
            var htmlString = @"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"" />
    <title>Charts Testing</title>
</head>

<body>
    <canvas id=""chart"" width=""400"" height=""500""></canvas>

    <script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js""></script>

    <script>
        var jsonData = {0};
        var ctx = ""chart"";
        
        // Convert JSON to Chart.js format
        var dataLabels = [];
        var dataData = [];
        var dataColors = [];
        
        jsonData.forEach(function(packet) {{
            dataLabels.push(packet.GroupKey);
            dataData.push(packet.TotalSpent);
            dataColors.push('#'+Math.floor(Math.random()*16777215).toString(16));
        }})
        
        console.dir(dataData);
        
        var data = {{
            labels: dataLabels,
            datasets: [{{
                data: dataData,
                backgroundColor: dataColors,
            }}]
        }};
        var myPieChart = new Chart(ctx, {{
            type: 'pie',
            data: data
        }});
        console.dir(jsonData);
    </script>
</body>

</html>";
            htmlString = string.Format(htmlString, csJsonData);
            htmlSource.Html = htmlString;

            ChartBrowser.Source = htmlSource;

            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
        }
    }
}