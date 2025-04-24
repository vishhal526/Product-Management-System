using Microsoft.AspNetCore.Mvc;
using Product_Management_System.Models;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Product_Management_System.Controllers
{
    public class ProductController : BaseController
    {

        public ProductController(IConfiguration _configuration) : base(_configuration)
        { }

        public IActionResult ProductList()
        {
            SqlCommand command = Command("PR_Product_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public void Dropdown(string STPROC)
        {
            SqlCommand command = Command(STPROC);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            List<UserDropDownModel> UserList = new List<UserDropDownModel>();
            foreach (DataRow data in table.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                UserList.Add(userDropDownModel);
            }
            ViewBag.UserList = UserList;
        }

        public IActionResult ProductDelete(int ProductID)
        {
            try
            {
                SqlCommand command = Command("PR_Product_DeleteByPK");
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
                command.ExecuteNonQuery();
                return RedirectToAction("ProductList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data ";
                Console.WriteLine(ex.ToString());
                return RedirectToAction("ProductList");
            }
        }

        public IActionResult ProductSave(ProductModel productModel)
        {
            if (productModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }
            if (ModelState.IsValid)
            {
                    using (SqlCommand command = Command(productModel.ProductID == null ? "PR_Product_Insert" : "PR_Product_UpdateByPK"))
                    {
                        if (productModel.ProductID != null)
                        {
                            command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
                        }

                        command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                        command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                        command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;

                        command.ExecuteNonQuery();
                    }
                    return RedirectToAction("ProductList");      
            }
            Dropdown("PR_User_DropDown");
            return View("ProductForm", productModel);
        }

        public IActionResult ProductForm(int ProductID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region User Drop-Down
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            connection1.Close();

            List<UserDropDownModel> users = new List<UserDropDownModel>();

            foreach (DataRow dataRow in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(dataRow["UserID"]);
                userDropDownModel.UserName = dataRow["UserName"].ToString();
                users.Add(userDropDownModel);
            }

            ViewBag.UserList = users;
            #endregion

            #region ProductByID

            SqlCommand command = Command("PR_Product_SelectByPK");
            command.Parameters.AddWithValue("@ProductID", ProductID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            ProductModel productModel = new ProductModel();

            foreach (DataRow @row in table.Rows)
            {
                productModel.ProductID = Convert.ToInt32(@row["ProductID"]);
                productModel.ProductName = @row["ProductName"].ToString();
                productModel.ProductCode = @row["ProductCode"].ToString();
                productModel.ProductPrice = Convert.ToDouble(@row["ProductPrice"]);
                productModel.Description = @row["Description"].ToString();
                productModel.UserID = Convert.ToInt32(@row["UserID"]);
            }

            #endregion

            return View("ProductForm", productModel);
        }

        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_Product_SelectAll", "ProductData.xlsx", "Product Data");
        }
    }
}
