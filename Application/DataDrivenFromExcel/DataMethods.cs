using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;
using static System.Configuration.ConfigurationManager;
using System.Data.OleDb;
using System.Data;

namespace Application.TestFramework
{
    public enum DataType
    {
        TestData,
        DataCreation
    }
    public enum Login
    {
        UserId,
        Password
    }
    public class DataMethods : WebDriverBase
    {
        public DataMethods(IWebDriver browserDriver) : base(browserDriver)
        {
        }

        private static string GetConnectionString(DataType type, string file)
        {
            string fileName;

            if(type.Equals(DataType.TestData))
            {
                fileName = string.Format(Path.Combine(AppSettings["TestDataFolder"], file));
            }
            else if(type.Equals(DataType.DataCreation))
            {
                fileName = string.Format(Path.Combine(AppSettings["DataCreationFolder"], file));
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find the file '{0}'", file));
            }
            var connectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml""", fileName);
            return connectionString;
        }
        public static DataTable ImportExcelSheetToDatatTable(DataType type, string file, string queryString)
        {
            var connectionString = GetConnectionString(type, file);
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(queryString, connectionString))
            using (DataSet dataSet = new DataSet())

            {
                try
                {
                    adapter.Fill(dataSet, "workSheet");
                    DataTable data = dataSet.Tables["workSheet"];
                    return data;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Format("Cannot import from '{0}', Exception: {1}", connectionString, ex.Message));
                }
            }
        }
        public static void ExportDataTableToExcelSheet(DataType type, string file, string queryString, Dictionary<string, string> parameterValues)
        {
            var connectionString = GetConnectionString(type, file);

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(queryString))
            {
                command.Connection = connection;

                try
                {
                    connection.Open();
                    foreach(KeyValuePair<string,string> item in parameterValues) 
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new ArgumentException(string.Format("Cannot import from '{0}', Exception: {1}", connectionString, ex.Message));
                }
            }
        }
    }
}
