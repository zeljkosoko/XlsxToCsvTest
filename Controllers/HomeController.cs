using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XlsxToCsvTest.Models;

namespace XlsxToCsvTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void Download()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("accept", "*/*");
                byte[] filedata = client.DownloadData("http://bakerhughesrigcount.gcs-web.com/static-files/b562fc2a-b229-41eb-8407-54dda5dc7295");

                using (Stream ms = new MemoryStream(filedata))
                {
                    string outputFile = Directory.GetCurrentDirectory() + "\\" + "rigCountsFinal.xlsx";
                    using (var localStream = System.IO.File.Create(outputFile))
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = ms.Read(buffer, 0, buffer.Length);
                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);
                            // Increment total bytes processed
                        } while (bytesRead > 0);

                        localStream.Flush();
                    }
                }
            }
        }
    }
}
