using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using SMSControlPanel.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SMSControlPanel.DataLayer
{
    public class CommonClass
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && !(dr[column.ColumnName] is DBNull))
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static ExcelPackage ListToExcel<T>(IEnumerable<T> result, string sheetname)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage package = new();

            var worksheet = package.Workbook.Worksheets.Add(sheetname);

            Type resultType = typeof(T);

            int columnIndex = 1;
            int rowIndex = 1;

            foreach (var prop in resultType.GetProperties())
            {
                worksheet.Cells[rowIndex, columnIndex].Value = prop.Name;
                columnIndex++;
            }
            rowIndex++;

            foreach (var row in result)
            {
                columnIndex = 1;
                foreach (var prop in resultType.GetProperties())
                {
                    worksheet.Cells[rowIndex, columnIndex].Value = row.GetType().GetProperty(prop.Name).GetValue(row, null);
                    if (row.GetType().GetProperty(prop.Name).ToString().Contains("DateTime"))  //To format date column
                        worksheet.Cells[rowIndex, columnIndex].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    columnIndex++;
                }
                rowIndex++;
            }

            worksheet.Cells.AutoFitColumns();

            return package;
        }

        public static DataTable ConvertJsonToDataTable(JsonResult jsonResult)
        {
            // Convert the JsonResult to a JSON string
            string jsonString = JsonConvert.SerializeObject(jsonResult.Value);

            // Parse the JSON string to a JArray
            JArray jsonArray = JArray.Parse(jsonString);

            // Create a new DataTable
            DataTable dataTable = new DataTable();

            // Iterate through the JSON array to extract column names and data
            foreach (JObject jsonObject in jsonArray)
            {
                // Add columns to the DataTable based on the JSON object properties
                foreach (JProperty property in jsonObject.Properties())
                {
                    if (!dataTable.Columns.Contains(property.Name))
                    {
                        dataTable.Columns.Add(property.Name);
                    }
                }

                // Add a new row to the DataTable with data from the JSON object
                DataRow dataRow = dataTable.NewRow();
                foreach (JProperty property in jsonObject.Properties())
                {
                    dataRow[property.Name] = property.Value.ToString();
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public static ExcelPackage DatatableToExcel(DataTable dataTable, string sheetName)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage package = new();

            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Set the column headers in the worksheet
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
            }

            // Populate the data rows in the worksheet
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1].Value = dataTable.Rows[i][j];
                }
            }

            return package;
        }

    }
}
