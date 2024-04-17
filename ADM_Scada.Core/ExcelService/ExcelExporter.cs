
using ClosedXML.Excel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;

namespace ADM_Scada.Core.ExcelService
{
    public interface IExcelExporter
    {
        void ExportToExcel<T>(ObservableCollection<T> data, string fileName);
    }
    public class ExcelExporter : IExcelExporter
    {
        public void ExportToExcel<T>(ObservableCollection<T> data, string fileName)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add(typeof(T).Name);

                // Headers
                int column = 1;
                foreach (System.Reflection.PropertyInfo property in typeof(T).GetProperties())
                {
                    worksheet.Cell(1, column).Value = property.Name;
                    column++;
                }

                // Data
                int row = 2;
                foreach (T item in data)
                {
                    column = 1;
                    foreach (System.Reflection.PropertyInfo property in typeof(T).GetProperties())
                    {
                        worksheet.Cell(row, column).Value = property.GetValue(item)?.ToString();
                        column++;
                    }
                    row++;
                }
                try
                {
                    workbook.SaveAs(fileName);
                    _ = System.Windows.Forms.MessageBox.Show("Exported successfully!");
                }
                catch (Exception ex)
                {                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

    }

}
