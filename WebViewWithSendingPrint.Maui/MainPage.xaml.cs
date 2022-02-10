using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System;
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
            MainWebView.HeightRequest = (deviceHeight / deviceDensity) - 50; // The device icons on top get in the way so - 50
#endif
            string documentsWebsite = _configuration["DocumentsWebsite"];
            string clientId = _configuration["ClientId"];
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
