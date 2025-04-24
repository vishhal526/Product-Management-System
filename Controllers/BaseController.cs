using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using OfficeOpenXml;

namespace Product_Management_System.Controllers
{
    public class BaseController : Controller
    {
        //public BaseController()
        //{
        //    ViewBag.UserName = HttpContext.Session.GetString("UserName");
        //}

        protected readonly IConfiguration configuration;

        public BaseController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public SqlCommand Command(string StoredProcedure)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = StoredProcedure;
            return command;
        }

        public IActionResult ExportToExcel(string STPROC, string filename, string sheetname)
        {

            SqlCommand command = Command(STPROC);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetname);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataTable.Rows[i][j];
                    }
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                byte[] fileContents = stream.ToArray();

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
        }
    }
}
