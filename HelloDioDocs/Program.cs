using System.Data;
using System.IO;
using GrapeCity.Documents.Excel;
using Microsoft.Data.SqlClient;

namespace HelloDioDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbook = new Workbook();
            workbook.Open("Template.xlsx");

            //workbook.Worksheets[0].Range["B5"].Value = "Hello, DioDocs!";
            //workbook.AddDataSource("CompanyName", "Hello, DioDocs!");
            //var invoice = new Invoice {CompanyName = "Hello, DioDocs!"};
            //workbook.AddDataSource("Invoice", invoice);

            var connectionStringBuilder =
                new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    UserID = "sa",
                    Password = "P@ssw0rd!"
                };
            using var connection = new SqlConnection(connectionStringBuilder.ToString());
            connection.Open();

            using var command =
                new SqlCommand(
                    File.ReadAllText("SelectInvoices.sql"),
                    connection);
            using var dataTable = new DataTable();
            dataTable.Load(command.ExecuteReader());

            workbook.AddDataSource("Invoice", dataTable);

            //Init template global settings
            workbook.Names.Add("TemplateOptions.InsertMode", "EntireRowColumn");
            workbook.Names.Add("TemplateOptions.KeepLineSize", "true");

            workbook.ProcessTemplate();

            workbook.Save("Invoice.pdf", SaveFileFormat.Pdf);
            workbook.Save("Invoice.xlsx", SaveFileFormat.Xlsx);
        }
    }
}
