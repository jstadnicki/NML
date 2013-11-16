using System.Collections.Generic;
using System.Data.OleDb;

namespace NML.Search.FilesSystem
{
    internal class FileIndexSearch
    {
        // Connection string for Windows Search
        private const string connectionString = "Provider=Search.CollatorDSO;Extended Properties=\"Application=Windows\"";

        // Execute a query and display the rowset up to chapterDepth deep
        internal static IEnumerable<FileSearchResult> ExecuteQuery(string query)
        {
            var result = new List<FileSearchResult>();
            var fileQuery = string.Format("SELECT TOP 10 System.ItemName, System.ItemUrl FROM SystemIndex WHERE System.FileName LIKE '{0}%'", query);

            OleDbDataReader myDataReader = null;
            var myOleDbConnection = new OleDbConnection(connectionString);
            var myOleDbCommand = new OleDbCommand(fileQuery, myOleDbConnection);

            try
            {
                myOleDbConnection.Open();
                myDataReader = myOleDbCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        result.Add(new FileSearchResult { Name = myDataReader.GetFieldValue<string>(0), Path = myDataReader.GetFieldValue<string>(1) });
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (myDataReader != null)
                {
                    myDataReader.Close();
                    myDataReader.Dispose();
                }
                if (myOleDbConnection.State == System.Data.ConnectionState.Open)
                {
                    myOleDbConnection.Close();
                }
            }

            return result;
        }
    }
}
