using System;
using System.Data;
using System.IO;
using GrapeCity.Documents.Excel;
using Microsoft.Data.SqlClient;

namespace HelloTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection =
                new SqlConnection(
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "localhost",
                        UserID = "sa",
                        Password = "P@ssw0rd!"
                    }.ToString());
            connection.Open();

            using var command =
                new SqlCommand(
                    File.ReadAllText("SelectInvoices.sql"),
                    connection);
            using var dataTable = new DataTable();
            dataTable.Load(command.ExecuteReader());

            var workbook = new Workbook();
            workbook.Open("Template.xlsx");
            workbook.AddDataSource("Invoice", dataTable);
            workbook.ProcessTemplate();

            workbook.Save("Invoice.pdf", SaveFileFormat.Pdf);
        }
    }
}
