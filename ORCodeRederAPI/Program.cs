using Nito.AsyncEx;
using System;
using System.IO;
using System.Net.Http;

namespace ORCodeRederConsole
{
    class Program
    {
        /// <summary>
        /// args[0] is used to pass in file path
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
            {
                Console.WriteLine("Only 1 parameter is expected which will be file path of the QR code to be scanned");
                return;
            }
            AsyncContext.Run(() => ConvertToData(args[0]));
            Console.ReadLine();
        }
        static async void ConvertToData(string filePath)
        {
            if (!File.Exists(filePath))
            {

                Console.WriteLine("No file found to be scanned, please check path provided as argument");
                return;
            }

            var fileStream = new FileStream(filePath, FileMode.Open);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StreamContent(fileStream), "file", "LHfile");
                var response = await client.PostAsync("http://api.qrserver.com/v1/read-qr-code/", formData);
                var qrCodeData = response.Content.ReadAsStringAsync();
                Console.WriteLine(qrCodeData.Result);
            }
        }
    }
}
