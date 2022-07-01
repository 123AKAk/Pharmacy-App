using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIproj
{
    class Subscription
    {
        public static void count(string name, string status)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://154.16.202.133:81/api/ubs/stats/count");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("cache-control", "no-cache");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"vendor_name\":\"" + name + "\"," +
                                    "\"bio_fingerprint\":\"" + status + "\"}";

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // Stat written to database successully
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: Contact the administrator.\n\nMore Information:\n" + ex.Message);
                return;
            }
        }
    }
}
