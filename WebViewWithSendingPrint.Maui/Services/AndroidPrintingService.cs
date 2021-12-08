using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebViewWithSendingPrint.Maui.Services
{
    public class AndroidPrintingService : IPrintingService
    {
        private readonly IConfiguration _configuration;

        public AndroidPrintingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Print(string printString)
        {
            throw new NotImplementedException();
        }
    }
}
