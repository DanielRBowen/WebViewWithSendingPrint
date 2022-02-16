using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.Text;

namespace WebViewWithSendingPrint.Maui
{
    public partial class MainPage : ContentPage
    {
        private readonly IConfiguration _configuration;
        public MainPage(
            IConfiguration configuration)
        {
            InitializeComponent();
            //https://www.syncfusion.com/blogs/post/learn-how-to-use-dependency-injection-in-net-maui.aspx
            _configuration = configuration;

#if ANDROID
            var deviceDensity = DeviceDisplay.MainDisplayInfo.Density;
            var deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
            MainWebView.WidthRequest = (deviceWidth / deviceDensity);
            var deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
            MainWebView.HeightRequest = (deviceHeight / deviceDensity);
#endif
            string documentsWebsiteFromConfiguration = _configuration["EatOnTheWebSite"];

            if (string.IsNullOrWhiteSpace(documentsWebsiteFromConfiguration))
            {
                Debug.WriteLine("Couldn't get the webview source from IConfiguraton");
            }
            else
            {
                Debug.WriteLine("Got the webview source from IConfiguraton");
            }

            string documentsWebsite = AppSettingsManager.Settings["DocumentsWebsite"];
            string clientId = AppSettingsManager.Settings["ClientId"];
            var webViewSourceBuilder = new StringBuilder();
            webViewSourceBuilder.Append(documentsWebsite);
            webViewSourceBuilder.Append("/documents");

            if (string.IsNullOrWhiteSpace(clientId) == false)
            {
                webViewSourceBuilder.Append("?ClientId=" + clientId);
            }

            MainWebView.Source = new Uri(webViewSourceBuilder.ToString()).ToString();
            // You can see how to implement starting to listen to documents in the Index.razor page.
        }
    }
}
