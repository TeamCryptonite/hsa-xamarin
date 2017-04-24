using System;
using System.ComponentModel;
using System.Diagnostics;
using HSAManager.Helpers.BizzaroHelpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Charts : ContentPage
    {
        private readonly BizzaroClient client = new BizzaroClient();

        private DateTime? oldStartDate = null;
        private DateTime? oldEndDate = null;
        private BizzaroAggregate.TimePeriod? oldTimePeriod = null;

        public Charts()
        {
            InitializeComponent();

            CreateChartHtml();

            ChartFormat.SelectedIndex = 0;
            oldTimePeriod = ConvertStringToTimePeriod((string) ChartFormat.SelectedItem);

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

        var graphOptions = {{
            onClick: graphClickEvent
        }}
        
        console.dir(dataData);
        
        var data = {{
            labels: dataLabels,
            datasets: [{{
                data: dataData,
                backgroundColor: dataColors,
            }}],
            options: graphOptions
        }};
        var myPieChart = new Chart(ctx, {{
            type: 'pie',
            data: data
        }});
        console.dir(jsonData);

        function graphClickEvent(event, array) {{
            window.location.href = array[0];
        }}
    </script>
</body>

</html>";
            htmlString = string.Format(htmlString, csJsonData);
            var htmlSource = new HtmlWebViewSource {Html = htmlString};

            var newChartBrowser = new WebView {Source = htmlSource, HeightRequest = 500};

            MainStack.Children.Insert(MainStack.Children.IndexOf(ChartBrowser), newChartBrowser);
            MainStack.Children.Remove(ChartBrowser);

            ChartBrowser = newChartBrowser;

            ChartBrowser.Navigating += (sender, e) =>
            {
                Debug.WriteLine(e.Url);
                e.Cancel = true;
            };

            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
        }

        private void CreateChartFromUIValues()
        {
            CreateChartHtml(StartDate.Date, EndDate.Date, ConvertStringToTimePeriod((string)ChartFormat.SelectedItem));
        }

        private void Date_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (oldStartDate == null || oldEndDate == null)
                return;
            if (StartDate.Date.Equals(oldStartDate) && EndDate.Date.Equals(oldEndDate))
                return;
            oldStartDate = StartDate.Date;
            oldEndDate = EndDate.Date;
            CreateChartFromUIValues();
        }

        private void ChartFormat_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldTimePeriod == null)
                return;
            if (ConvertStringToTimePeriod((string) ChartFormat.SelectedItem).Equals(oldTimePeriod))
                return;

            oldTimePeriod = ConvertStringToTimePeriod((string) ChartFormat.SelectedItem);
            CreateChartFromUIValues();
        }

        private BizzaroAggregate.TimePeriod? ConvertStringToTimePeriod(string str)
        {
            BizzaroAggregate.TimePeriod? timePeriod = null;
            switch ((string)ChartFormat.SelectedItem)
            {
                case "Year and Month":
                    timePeriod = BizzaroAggregate.TimePeriod.YearMonth;
                    break;
                case "Year Only":
                    timePeriod = BizzaroAggregate.TimePeriod.Year;
                    break;

                case "Month Only":
                    timePeriod = BizzaroAggregate.TimePeriod.Month;
                    break;
                case "Year, Month, and Day":
                    timePeriod = BizzaroAggregate.TimePeriod.Day;
                    break;
                default:
                    timePeriod = null;
                    break;
            }

            return timePeriod;
        }
    }
}