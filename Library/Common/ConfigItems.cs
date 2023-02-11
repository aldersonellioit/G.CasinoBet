using System;
using System.Configuration;

namespace Common
{
    public class ConfigItems
    {
        public static string Conn_AccDgroup
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conn_AccDgroup"].ToString();
            }
        }
        public static string Conn_LogDgroup
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conn_LogDgroup"].ToString();
            }
        }
        public static string Conn_Casino
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conn_Casino"].ToString();
            }
        }
        public static string Conn_Casinogroup
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conn_Casinogroup"].ToString();
            }
        }
        public static string AdminBookUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminBookUrl"].ToString();

            }
        }
        public static Boolean LogFlag
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["LogFlag"].ToString());
            }
        }
        public static string Logurl
        {
            get
            {
                return ConfigurationManager.AppSettings["Logurl"].ToString();
            }
        }
        public static string Logurlwa
        {
            get
            {
                return ConfigurationManager.AppSettings["Logurlwa"].ToString();
            }
        }
        public static Boolean ErrorLog
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["ErrorLog"].ToString());
            }
        }
        public static Boolean AllLog
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllLog"].ToString());
            }
        }
        public static string JwtIssuer
        {
            get
            {
                return ConfigurationManager.AppSettings["JwtIssuer"].ToString();
            }
        }
        public static string AudienceId
        {
            get
            {
                return ConfigurationManager.AppSettings["AudienceId"].ToString();
            }
        }
        public static string AudienceSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["AudienceSecret"].ToString();
            }
        }
        public static int DefaultJwtExpireInMin
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultJwtExpireInMin"]);
            }
        }
        public static string[] ExWords
        {
            get
            {
                return ConfigurationManager.AppSettings["ExWords"].ToString().Split(',');
            }
        }
        public static string CopUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CopUrl"].ToString();
            }
        }
        public static bool Encrypt
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["Encrypt"].ToString());
            }
        }
        public static string RedisLocal
        {
            get
            {
                return ConfigurationManager.AppSettings["RedisLocal"].ToString();
            }
        }
        public static int RedisLocaldb
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["RedisLocaldb"].ToString());
            }
        }
    }
}
