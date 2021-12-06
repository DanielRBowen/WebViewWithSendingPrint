using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using PDFtoPrinter;
using Syroot.Windows.IO;

namespace WebViewWithSendingPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _hubConnection;

        public MainWindow()
        {
            InitializeComponent();

            // Cntl + F5 is hard refresh if changes are not showing. You can still dev tools when hitting F12 when focused.
            bool.TryParse(AppSettingsManager.Settings["IsDebugMode"], out bool isDebugMode);

            Dispatcher.BeginInvoke(async () =>
            {
                await DocumentsWebView.EnsureCoreWebView2Async();

                if (isDebugMode)
                {
                    DocumentsWebsitesWindow.WindowState = WindowState.Normal;
                    DocumentsWebsitesWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    DocumentsWebsitesWindow.Topmost = false;
                    DocumentsWebView.CoreWebView2.Settings.AreDevToolsEnabled = true;
                }
                else
                {
                    DocumentsWebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
                }
            });

            string documentsWebsite = AppSettingsManager.Settings["DocumentsWebsite"];
            string clientId = AppSettingsManager.Settings["ClientId"];
            var webViewSourceBuilder = new StringBuilder();
            webViewSourceBuilder.Append(documentsWebsite);
            webViewSourceBuilder.Append("/documents");

            if (string.IsNullOrWhiteSpace(clientId) == false)
            {
                webViewSourceBuilder.Append("?ClientId=" + clientId);
            }

            DocumentsWebView.Source = new Uri(webViewSourceBuilder.ToString());

            Dispatcher.BeginInvoke(async () =>
            {
                await StartListeningToDocuments(documentsWebsite, clientId);
            });
        }

        public async Task StartListeningToDocuments(string documentsWebsite, string clientId)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl($"{documentsWebsite}/documentshub?ClientId={clientId}").WithAutomaticReconnect().Build();
            _hubConnection.On("printDocument", (string documentString) => PrintDocument(documentString));
            await _hubConnection.StartAsync();
        }

        public void PrintDocument(string documentString)
        {
            string printerName = AppSettingsManager.Settings["PrinterName"];

            _ = string.IsNullOrWhiteSpace(printerName)
                ? RawPrinterHelper.SendStringToPrinter(DefaultPrinter, documentString)
                : RawPrinterHelper.SendStringToPrinter(printerName, documentString);
        }

        public void TestPrinter()
        {
            var localPrintServerName = new LocalPrintServer().Name;
            Debug.WriteLine(localPrintServerName);
        }

        public static List<string> InstalledPrinters
        {
            get
            {
                return (from PrintQueue printer in new LocalPrintServer().GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local,
                EnumeratedPrintQueueTypes.Connections }).ToList()
                        select printer.Name).ToList();
            }
        }

        // https://stackoverflow.com/questions/86138/whats-the-best-way-to-get-the-default-printer-in-net
        public static string DefaultPrinter => new PrinterSettings().PrinterName;
    }
}
