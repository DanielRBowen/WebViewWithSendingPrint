using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using Microsoft.Extensions.Logging;

namespace WebViewWithSendingPrint.Maui.Services
{
    public class WindowsPrintingService : IPrintingService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<WindowsPrintingService> _logger;
        private readonly OnScreenLogs _onScreenLogs;

        public WindowsPrintingService(IConfiguration configuration, ILogger<WindowsPrintingService> logger, OnScreenLogs onScreenLogs)
        {
            _configuration = configuration;
            _logger = logger;
            _onScreenLogs = onScreenLogs;
        }

        public void Print(string printString)
        {
            _onScreenLogs.LastLogEntry = "Called print";
            string printerName = _configuration["PrinterName"];

            _ = string.IsNullOrWhiteSpace(printerName)
              ? RawPrinterHelper.SendStringToPrinter(DefaultPrinter, printString)
              : RawPrinterHelper.SendStringToPrinter(printerName, printString);
        }

        public static string DefaultPrinter => new PrinterSettings().PrinterName;
    }
}
