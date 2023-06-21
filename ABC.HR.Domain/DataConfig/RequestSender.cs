using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;


namespace ABC.HR.Domain.DataConfig
{
    public class RequestSender
    {

        private static IHttpContextAccessor httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static RequestSender Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequestSender();
                }
                return instance;
            }
        }


        private static RequestSender instance;
  

        private RequestSender()
        {
        }
        
        public SResponse CallAPI(string otype, string action, string method, string inpobject = "")
        {
           

            string AccessToken = string.Empty;
            //string userData = httpContextAccessor.HttpContext.Session.GetString("");
            //if (!string.IsNullOrEmpty(userData))
            //{
            //    UserDetailAuthToken userDetails = JsonConvert.DeserializeObject<UserDetailAuthToken>(userData);
            //    AccessToken = userDetails.Token;
            //}
            SResponse waresp = new SResponse();
            string resp = "";
            try
            {
                //https://apps.ab-sol.net/abc.hr.api/
                // http://localhost:37474/

                //https://localhost:44392/
                //https://abc.vked.net/
                //http://199.231.160.216/abchrapi/
                //http://45.35.97.246/abcdhrapi/
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44392/" + otype + "/" + action);
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://45.35.97.246:5595/abcdhrapi/" + otype + "/" + action);
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://199.231.160.216/abchrapi/" + otype + "/" + action);
                request.Headers.Add("Authorization", "Bearer " + AccessToken);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = method;
                if (System.Diagnostics.Debugger.IsAttached)
                    request.Timeout = 120000;
                else
                    request.Timeout = 40000;

                request.ContentType = "application/json";
                request.ContentLength = 0;

                if (inpobject != "")
                {
                    byte[] byteData = Encoding.UTF8.GetBytes(inpobject);
                    request.ContentLength = byteData.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(byteData, 0, byteData.Length);
                    stream.Close();
                }
                ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(
                delegate (
                object sender2,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                });

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                resp = sr.ReadToEnd();
                waresp.Status = true;
                waresp.Resp = resp;
                return waresp;
            }
            catch (WebException wex)
            {
                waresp.Status = false;
                waresp.Error = wex.Message;
                if (wex.Message == "error: (403) Forbidden.")
                    waresp.ErrorCode = 403;
                else if (wex.Message == "The remote server returned an error: (401) Unauthorized.")
                    waresp.ErrorCode = 401;
                else
                    waresp.ErrorCode = 0;
                waresp.Resp = wex.Message;
                return waresp;
            }
            catch (Exception ex)
            {
                waresp.Status = false;
                waresp.Error = ex.Message;
                waresp.ErrorCode = 0;
                return waresp;
            }

            finally
            {

            }
        }

        public SResponse CallYouTubeAPI(string videoID)
        {
            SResponse waresp = new SResponse();
            string resp = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/oembed?url=https://www.youtube.com/watch?v=" + videoID + "&t=10s&format=json");
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                if (System.Diagnostics.Debugger.IsAttached)
                    request.Timeout = 120000;
                else
                    request.Timeout = 40000;

                request.ContentType = "application/json";
                request.ContentLength = 0;


                ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(
                delegate (
                object sender2,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                });

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                resp = sr.ReadToEnd();
                waresp.Status = true;
                waresp.Resp = resp;
                return waresp;
            }
            catch (WebException wex)
            {

                waresp.Status = false;
                waresp.Error = wex.Message;
                if (wex.Message == "error: (403) Forbidden.")
                    waresp.ErrorCode = 403;
                else
                    waresp.ErrorCode = 0;
                waresp.Resp = wex.Message;
                return waresp;
            }
            catch (Exception ex)
            {
                waresp.Status = false;
                waresp.Error = ex.Message;
                waresp.ErrorCode = 0;
                return waresp;
            }

            finally
            {

            }
        }
        public class SResponse
        {
            public SResponse() { }

            public string Error { get; set; }
            public int ErrorCode { get; set; }
            public string Key { get; set; }
            public string msg { get; set; }
            public string Resp { get; set; }
            public bool Status { get; set; }

            public enum RequestMethod
            {
                GET,
                POST,
                DELETE,
                PUT,
            }
        }
    }
}