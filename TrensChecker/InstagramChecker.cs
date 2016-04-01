//Author = +Haze

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Collections;

namespace FlatUIRemake
{
    public class InstagramChecker
    {
        private CookieContainer session;
        private readonly string userAgent;

        public InstagramChecker()
        {
            session = new CookieContainer();
            userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36";
            IntializeSession();
        }

        private void IntializeSession()
        {
            var req = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.Headers.Add("accept-encoding", "gzip, deflate, sdch");
            req.Headers.Add("accept-language", "en-US,en;q=0.8");
            req.Headers.Add("upgrade-insecure-requests", "1");
            req.CookieContainer = session;
            req.UserAgent = userAgent;

            using (var res = (HttpWebResponse)req.GetResponse()) { }
        }

        public bool IsUsernameAvailable(string username)
        {
            var req = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/accounts/web_create_ajax/attempt/");
            req.Method = "POST";
            req.Accept = "*/*";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.Referer = "https://www.instagram.com/";
            req.CookieContainer = session;
            req.UserAgent = userAgent;
            req.Headers.Add("x-csrftoken", GetCsrfToken(session));
            req.Headers.Add("x-instagram-ajax", "1");
            req.Headers.Add("x-requested-with", "XMLHttpRequest");

            using (var rs = req.GetRequestStream())
            {
                string postForm = string.Format("email={0}&password={1}&username={2}&first_name={3}", "dad@gmail.com", "hey", username, "John");
                byte[] payload = Encoding.UTF8.GetBytes(postForm);

                rs.Write(payload, 0, payload.Length);
            }

            using (var res = (HttpWebResponse)req.GetResponse())
            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                return !sr.ReadToEnd().Contains("errors\":{\"username");
            }
        }

        private string GetCsrfToken(CookieContainer cookieContainer)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance;
            var table = (Hashtable)cookieContainer.GetType().InvokeMember("m_domainTable", flags, null, cookieContainer, new object[] { });

            foreach (Cookie cookie in cookieContainer.GetCookies(new Uri("http://www.instagram.com/")))
            {
                if (cookie.Name == "csrftoken")
                {
                    return cookie.Value;
                }
            }

            return null;
        }
    }
}