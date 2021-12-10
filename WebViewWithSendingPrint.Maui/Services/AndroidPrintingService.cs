#if ANDROID
using Android.Bluetooth;
using Java.Util;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebViewWithSendingPrint.Maui.Services
{
    public class AndroidPrintingService : IPrintingService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AndroidPrintingService> _logger;
        private readonly OnScreenLogs _onScreenLogs;

        public AndroidPrintingService(IConfiguration configuration, ILogger<AndroidPrintingService> logger, OnScreenLogs onScreenLogs)
        {
            _configuration = configuration;
            _logger = logger;
            _onScreenLogs = onScreenLogs;
        }

        /// <summary>
        /// From
        /// https://youtu.be/f58JxtDfTq4
        /// </summary>
        /// <param name="printString"></param>
        public async void Print(string printString)
        {
#if ANDROID
            try
            {
                BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                BluetoothDevice bluetoothDevice = (from bondedDevice in bluetoothAdapter?.BondedDevices
                                                   where bondedDevice?.Name == _configuration["PrinterName"]
                                                   select bondedDevice).FirstOrDefault();

                if (bluetoothDevice == null)
                {
                    bluetoothDevice = bluetoothAdapter?.BondedDevices.FirstOrDefault();
                }

                if (bluetoothDevice == null)
                {
                    throw new NullReferenceException("No bluetooth devices were found");
                }

                await Task.Delay(1000);
                BluetoothSocket bluetoothSocket = bluetoothDevice.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                bluetoothSocket?.Connect();
                byte[] buffer = Encoding.UTF8.GetBytes(printString);
                bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                bluetoothSocket?.Close();

            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString(), exception);
                _onScreenLogs.LastLogEntry = exception.ToString();
            }
#endif
        }
    }
}
