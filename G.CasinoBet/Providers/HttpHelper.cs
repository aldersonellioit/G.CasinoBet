using Common;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace G.CasinoBet.Providers
{
    public static class HttpHelper
    {
        public static string PostLog(string uri, string data, string contentType, string method = "POST", string authHeader = "", string api = "", string ip = "")
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;
                request.Timeout = 300;

                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add("Authorization", "bearer " + authHeader);

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                //_Elog.WriteLog(ex.ToString() + uri + data + api + ip);
                return "{'status':'400','message':'Unauthorized access'}";
            }
            catch (Exception ex)
            {
                //_Elog.WriteLog(ex.ToString() + uri + data + api + ip);
                return "{'status':'400','message':'" + ex.ToString() + "'}";
            }
        }
    }
}
