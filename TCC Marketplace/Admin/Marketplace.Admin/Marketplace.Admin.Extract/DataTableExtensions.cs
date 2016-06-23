using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Admin.Extract
{
    public static class DataTableExtensions
    {
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
