using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Data
{
    public class SqlClientService : ISqlClientService
    {
        public DataSet Execute(string spName, string connectionString, IList<SqlParameter> sqlParameter)
        {
            using (var connaction = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connaction;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameter.Any())
                    {
                        foreach (var item in sqlParameter)
                        {
                            command.Parameters.Add(item);
                        }
                    }
                    using (var da = new SqlDataAdapter())
                    {
                        da.SelectCommand = command;

                        var ds = new DataSet();

                        try
                        {
                            da.Fill(ds);
                        }
                        catch (System.Exception ex)
                        {
                            WriteLogAll(ex.ToString()+"ds"+ "connectionString" + JsonConvert.SerializeObject(ds));
                        }
                        finally
                        {
                            connaction.Close();
                            connaction.Dispose();
                            command.Dispose();
                            da.Dispose();
                        }

                        return ds;
                    }
                }
            }
        }
        public static void WriteLogAll(String str)
        {
            try
            {
                if (ConfigItems.AllLog)
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
                        //sw.Write(DateTime.Now);.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                        sw.WriteLine(Environment.NewLine + str );
                    }
                }
            }
            catch (Exception)
            { }
        }
    }
}
