using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CountryCheckerPrototype.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string wipMania = "http://api.wipmania.com/{{ip}}";
            string hostip = "http://api.hostip.info/get_json.php?ip={{ip}}";

            string ip = GetIp();

            wipMania = wipMania.Replace("{{ip}}", ip);
            hostip = hostip.Replace("{{ip}}", ip);

            string wipManiaResponse = HttpGetRequest(wipMania);
            string hostipResponse = HttpGetRequest(hostip);

            ViewBag.ClientIP = ip;
            ViewBag.HostipResponse = hostipResponse;
            ViewBag.WipManiaResponse = wipManiaResponse;
            return View();
        }

        public ActionResult hostip()
        {
            string data = "";



            return Json(data);
        }

        public string GetIp()
        {
            string ip = "";
            ip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] as String;

            if (String.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"] as String;
            }

            return ip;
        }

        static public string HttpGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string data = "";
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    data = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                //throw;
            }

            return data;
        }
    }
}