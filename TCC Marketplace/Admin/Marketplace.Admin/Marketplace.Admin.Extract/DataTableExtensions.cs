using System;
using System.Data;
using System.Text;

namespace Marketplace.Admin.Extract
{
    /// <summary>
    /// Extension functions handler for type DataTable.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Write to CSV file.
        /// </summary>
        /// <param name="dataTable"> Input DataTable. </param>
        /// <param name="filePath"> File Location. </param>
        /// <param name="delimiter"> Comma separator for csv file. </param>
        public static void WriteToCsvFile(this DataTable dataTable, string filePath, string delimiter = ",")
        {

            var fileContent = new StringBuilder();

            foreach (var col in dataTable.Columns)
            {
                fileContent.Append(col + delimiter);
            }

            fileContent.Replace(delimiter, Environment.NewLine, fileContent.Length - 1, 1);



            foreach (DataRow dr in dataTable.Rows)
            {

                foreach (var column in dr.ItemArray)
                {
                    if (column.GetType() == typeof(DateTime))
                    {
                        var dt = (DateTime)column;

                        fileContent.Append("\"" + dt.ToString("dd-MM-yyyy HH:mm:ss") + "\"" + delimiter);
                    }
                    else
                    {
                        fileContent.Append("\"" + column + "\"" + delimiter);
                    }
                }

                fileContent.Replace(delimiter, Environment.NewLine, fileContent.Length - 1, 1);
            }

            System.IO.File.WriteAllText(filePath, fileContent.ToString());
        }
    }
}
