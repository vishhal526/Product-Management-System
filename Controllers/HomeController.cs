using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Product_Management_System.Controllers
{
    [CheckAccess]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration _configuration) : base(_configuration)
        { }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public int GetCount(string tablename) {

            int count;
            SqlCommand command = Command("PR_AllTable_Count");
            command.Parameters.AddWithValue("@TableName", tablename);
            count = (int)command.ExecuteScalar();
            return count;

        }

        public IActionResult Index()
        {
            ViewBag.CustomerCount = GetCount("Customer");
            ViewBag.UserCount = GetCount("User");
            ViewBag.ProductCount = GetCount("Product");
            ViewBag.OrderCount = GetCount("Orders");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
