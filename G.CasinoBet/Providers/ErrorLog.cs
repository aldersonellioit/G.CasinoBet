using Common;
using G.CasinoBet.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ELog
{
    public class ErrorLog
    {
        public ErrorLog()
        {

        }

        public static void WriteLog(String str, String request = "")
        {
            try
            {
                if (ConfigItems.ErrorLog)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Log/Error_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt");
                    if (!File.Exists(path))
                    {
                        var myFile = File.Create(path);
                        myFile.Close();
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("---------------------------------------------------------------------------------------------");
                        sw.Write(DateTime.Now);
                        sw.WriteLine(Environment.NewLine + str + Environment.NewLine + request);
                    }
                }
            }
            catch (Exception)
            { }
            try
            {
                if (ConfigItems.LogFlag)
                {
                    Log(str, request);
                }
            }
            catch (Exception)
            {
            }
        }
        public static void Log(String ex, String req)
        {
            var par = "ex=" + ex + "&req=" + req;
            Task.Factory.StartNew(() => HttpHelper.PostLog(ConfigItems.Logurl + ApiEndpoint.log, par, "application/x-www-form-urlencoded", "POST"));
        }
        public static void WriteLog(String str, String request, String ErrorMesage = "")
        {
            try
            {
                if (ConfigItems.ErrorLog)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Log/Error_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt");
                    if (!File.Exists(path))
                    {
                        var myFile = File.Create(path);
                        myFile.Close();
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("---------------------------------------------------------------------------------------------");
                        sw.Write(DateTime.Now);
                        sw.WriteLine(Environment.NewLine + str + Environment.NewLine + request + Environment.NewLine + ErrorMesage);
                    }
                }
            }
            catch (Exception)
            { }
            try
            {
                if (ConfigItems.LogFlag)
                {
                    Log(str, request);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void WriteLogAll(String str, String request = null)
        {
            try
            {
                if (ConfigItems.AllLog)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Log/Request/Log_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt");
                    if (!File.Exists(path))
                    {
                        var myFile = File.Create(path);
                        myFile.Close();
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("---------------------------------------------------------------------------------------------");
                        sw.Write(DateTime.Now);
                        sw.WriteLine(Environment.NewLine + str + Environment.NewLine + request);
                    }
                }
            }
            catch (Exception)
            { }
            
        }

    }
}