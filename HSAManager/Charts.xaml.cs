using System;
using System.ComponentModel;
using HSAManager.Helpers.BizzaroHelpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Charts : ContentPage
    {
        private readonly BizzaroClient client = new BizzaroClient();

        private DateTime oldStartDate;
        private DateTime oldEndDate;

        public Charts()
        {
            InitializeComponent();

            CreateChartHtml();

            StartDate.Date = DateTime.Now.AddMonths(-6);
            StartDate.MinimumDate = DateTime.Parse("01/01/2000");
            StartDate.MaximumDate = DateTime.Now;
            oldStartDate = StartDate.Date;

            EndDate.Date = DateTime.Now;
            EndDate.MinimumDate = DateTime.Parse("01/01/2000");
            EndDate.MaximumDate = DateTime.Now;
            oldEndDate = EndDate.Date;
        }

        private async void CreateChartHtml(DateTime? startDate = null, DateTime? endDate = null, BizzaroAggregate.TimePeriod? timePeriod = null)
        {
            ActivityIndicator.IsVisible = true;
            ActivityIndicator.IsRunning = true;
            var htmlSource = new HtmlWebViewSource();

            var csJsonData = await client.Aggregate.GetSpendingOverTime(startDate, endDate, timePeriod);
            var htmlString = @"
<!DOCTYPE html>
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

        private void Date_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StartDate.Date.Equals(oldStartDate) && EndDate.Date.Equals(oldEndDate))
                return;
            oldStartDate = StartDate.Date;
            oldEndDate = EndDate.Date;
            CreateChartHtml(StartDate.Date, EndDate.Date);
        }
    }
}