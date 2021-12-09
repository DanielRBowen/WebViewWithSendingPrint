using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

namespace WebViewWithSendingPrint.Maui.Services
{
    public class WindowsPrintingService : IPrintingService
    {
        private readonly IConfiguration _configuration;

        public WindowsPrintingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Print(string printString)
        {
            string printerName = _configuration["PrinterName"];

            _ = string.IsNullOrWhiteSpace(printerName)
              ? RawPrinterHelper.SendStringToPrinter(DefaultPrinter, printString)
              : RawPrinterHelper.SendStringToPrinter(printerName, printString);
        }

        public static string DefaultPrinter => new PrinterSettings().PrinterName;
    }
}
