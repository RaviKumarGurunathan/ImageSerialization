﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImageSave.Models;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;

namespace ImageSave.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostPath)
        {
            _hostingEnvironment = hostPath;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RenderImage()
        {
            // Write the image string to Disk
            var imageData = "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";
            var imagePngData = "iVBORw0KGgoAAAANSUhEUgAAAqYAAABUCAIAAADf3GLzAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAAhZJREFUeF7t1QdWE1AURdE3/4GJvSzpqAiCpheSkJ6Ycdy/z9pzOHWUJEkNZPmSJDWR5UuS1ER1OB4BgHh1OBwBgHiWDwBNOC1fkiTlZ/mSJDVR7Q8HACBe7fcHACCe5QNAE2q3PwAA8SwfAJpQ290BAIh3Wv4eAIhn+QDQhNps9wBAPMsHgCbUerMDAOJZPgA0oVabHQAQr5brHQAQ77T8LQAQr15XWwAgXi2WWwAgXs2XGwAgXs1fNwBAvJotNgBAvHpZrAGAeDWdrwGAeDWZrwGAeDWerQCAeDV6WQEA8Wo4XQEA8WowXQIA8ao/WQIA8ao3fgUA4lV39AoAxKvOcAEAxKt/gwUAEK/+DuYAQLx67s8BgHj11JsBAPHqsTsDAOLVQ2cGAMSr3/9eAIB4df/3BQCIV7+epwBAvPr5PAUA4tXd0wQAiFe3fyYAQLy6eRwDAPHq+mEMAMSrq4cRABCvLn+PAIB4dXE/AgDi1fn9EACIV99/DQGAePX95xAAiFfffgwAgHiWDwBNqK93AwAgXn256wMA8erzbR8AiGf5ANCE+nTTBwDi1cebHgAQrz5e9wCAePXhugcAxKsPVz0AIF69v+oBAPHq3WUXAIhn+QDQBMsHgCbU28suABCv3l50AYB4lg8ATaiziy4AEO+0/A4AEM/yAaAJdXbeAQDi1ZvzDgAQz/IBoAl1lCRJ8R2P/wErL7gat+6PcAAAAABJRU5ErkJggg==";
            byte[] imageBytes = Convert.FromBase64String(imagePngData);
            MemoryStream data = new MemoryStream(imageBytes);
            string filePath = _hostingEnvironment.WebRootPath + "\\" + "App_Data";
            using (var reader = new System.IO.StreamReader(data))
            {
                string contentAsString = reader.ReadToEnd();
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(contentAsString);
                if (System.IO.File.Exists(filePath + "\\ImageTest.png"))
                {
                    System.IO.File.Delete(filePath + "\\ImageTest.png");
                }
                System.IO.File.WriteAllBytes(filePath + "\\ImageTest.png", bytes);
                reader.Close();
                reader.Dispose();
            }

            // Read and Display the above written image from disk
            Bitmap brokenImage = new Bitmap(filePath + "\\ImageTest.png");
            byte[] brokenImageData;
            using (MemoryStream ms = new MemoryStream())
            {
                brokenImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                brokenImageData = ms.ToArray();
            }

            return File(brokenImageData, "image/Jpeg");
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
    }
}
