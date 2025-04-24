using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OfficeOpenXml;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Product_Management_System.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(IConfiguration _configuration) : base(_configuration)
        { }

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

        public IActionResult CustomerList()
        {
            SqlCommand command = Command("PR_Customer_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult CustomerDelete(int CustomerID)
        {
            try
            {
                SqlCommand command = Command("PR_Customer_DeleteByPK");
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                command.ExecuteNonQuery();
                return RedirectToAction("CustomerList");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data ";
                Console.WriteLine(e.ToString);
                return RedirectToAction("CustomerList");
            }
        }

        public IActionResult CustomerForm(int CustomerID) {

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

            #region CustomerByID

            SqlCommand command = Command("PR_Customer_SelectByPK");
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CustomerModel customerModel = new CustomerModel();

            foreach (DataRow @row in table.Rows)
            {
                customerModel.CustomerID = Convert.ToInt32(@row["CustomerID"]);
                customerModel.CustomerName = @row["CustomerName"].ToString();
                customerModel.HomeAddress = @row["HomeAddress"].ToString();
                customerModel.Email = @row["Email"].ToString();
                customerModel.MobileNo = @row["MobileNo"].ToString();
                customerModel.GSTNO = @row["GSTNO"].ToString();
                customerModel.CityName = @row["CityName"].ToString();
                customerModel.PinCode = @row["PinCode"].ToString();
                customerModel.NetAmount = Convert.ToDecimal(@row["NetAmount"]);
                customerModel.UserID = Convert.ToInt32(@row["UserID"]);
            }
            #endregion

            Dropdown("PR_User_DropDown");
            return View("CustomerForm",customerModel); 
        }

        public IActionResult CustomerSave(CustomerModel customermodel)
        {
            if (customermodel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }
            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(customermodel.CustomerID == null ? "PR_Customer_Insert" : "PR_Customer_UpdateByPK"))
                {
                    if (customermodel.CustomerID != null) 
                    {
                        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customermodel.CustomerID;
                    }
                    command.Parameters.Add("CustomerName", SqlDbType.VarChar).Value = customermodel.CustomerName;
                    command.Parameters.Add("HomeAddress", SqlDbType.VarChar).Value = customermodel.HomeAddress;
                    command.Parameters.Add("EmailID", SqlDbType.VarChar).Value = customermodel.Email;
                    command.Parameters.Add("MobileNo", SqlDbType.VarChar).Value = customermodel.MobileNo;
                    command.Parameters.Add("GSTNO", SqlDbType.VarChar).Value = customermodel.GSTNO;
                    command.Parameters.Add("CityName", SqlDbType.VarChar).Value = customermodel.CityName;
                    command.Parameters.Add("PinCode", SqlDbType.VarChar).Value = customermodel.PinCode;
                    command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customermodel.NetAmount;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = customermodel.UserID;
                    command.ExecuteNonQuery();
                }
                    return RedirectToAction("CustomerList");
            }
            Dropdown("PR_User_DropDown");
            return View("CustomerForm",customermodel);
        }

        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_Customer_SelectAll", "CustomerData.xlsx", "Customer Data");
        }
    }
}
