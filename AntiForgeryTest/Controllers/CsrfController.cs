using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using AntiForgeryTest.Models;

namespace AntiForgeryTest.Controllers
{
    public class CsrfController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TrySpoof()
        {
            var spoofModel = new SpoofModel();
            return View(spoofModel);
        }

        [HttpPost]
        public ActionResult TrySpoof(SpoofModel model)
        {
            //var antiForgeryTokenValue = GetAntiForgeryToken("http://antiforgerytest.local/AntiForgery/AntiForgeryForm", "//input[@type='hidden' and @name='__RequestVerificationToken']");
            var spoofResult = SpoofPost("http://antiforgerytest.local/AntiForgery/AntiForgeryFormPost", model.AntiForgeryKey, model.AntiForgeryCookieValue);

            var resultHtmlPage = new HtmlDocument();
            resultHtmlPage.LoadHtml(spoofResult);
            var resultMessage = resultHtmlPage.DocumentNode.SelectSingleNode("//title").InnerText;
            ViewBag.SpoofResult = resultMessage;
 
            return View(model);
        }




        private string GetAntiForgeryToken(string url, string tokenXPath)
        {
            var rawHtml = "";
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                rawHtml = client.DownloadString(url);
                //...
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(rawHtml);
            var tokenNode = doc.DocumentNode.SelectSingleNode(tokenXPath);
            var tokenValue = tokenNode.Attributes["value"].Value;

            return tokenValue;
        }

        private string SpoofPost(string url, string antiForgeryKey, string antiForgeryCookieValue)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";

            CookieContainer cookies = new System.Net.CookieContainer();
            cookies.SetCookies(new Uri("http://antiforgerytest.local"), "__RequestVerificationToken_Lw=" + antiForgeryCookieValue);
            httpWebRequest.CookieContainer = cookies;

            var sb = new StringBuilder();
            sb.Append("__RequestVerificationToken=" + antiForgeryKey);

            byte[] requestBytes = Encoding.UTF8.GetBytes(sb.ToString());
            httpWebRequest.ContentLength = requestBytes.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            try
            {
                using (var response = httpWebRequest.GetResponse())
                {
                    return HandleResponse(response);
                }
                
            }
            catch (WebException e)
            {
                return HandleResponse(e.Response);
            }
        }

        private string HandleResponse(WebResponse response)
        {
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();

            return responseFromServer;
        }

    }
}
