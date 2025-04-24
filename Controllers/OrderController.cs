using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Product_Management_System.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IConfiguration _configuration) : base(_configuration)
        { }

        public IActionResult OrderList()
        {
            SqlCommand command = Command("PR_Order_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult OrderDelete(int OrderID)
        {
            try {
                SqlCommand command = Command("PR_Order_DeleteByPK");
                command.Parameters.Add("@OrderID",SqlDbType.Int).Value = OrderID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderList");
            }
            catch(Exception e) {
                TempData["ErrorMessage"] = "Couldn't Delete the Data";
                Console.WriteLine(e.ToString);
                return RedirectToAction("OrderList");
            }
        }

        public void Dropdown(string STPROC ,bool model) {
            SqlCommand command = Command(STPROC);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            if (model)
            {
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
            else {
                List<CustomerDropDownModel> customerList = new List<CustomerDropDownModel>();
                foreach (DataRow data in table.Rows)
                {
                    CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                    customerDropDownModel.CustomerID = Convert.ToInt32(data["CustomerID"]);
                    customerDropDownModel.CustomerName = data["CustomerName"].ToString();
                    customerList.Add(customerDropDownModel);
                }
                ViewBag.CustomerList = customerList;
            }
        }
        public IActionResult OrderForm(int OrderID)
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

            #region Customer Drop-Down
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_Customer_DropDown";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            connection2.Close();

            List<CustomerDropDownModel> customers = new List<CustomerDropDownModel>();

            foreach (DataRow dataRow in dataTable2.Rows)
            {
                CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                customerDropDownModel.CustomerID = Convert.ToInt32(dataRow["CustomerID"]);
                customerDropDownModel.CustomerName = dataRow["CustomerName"].ToString();
                customers.Add(customerDropDownModel);
            }

            ViewBag.UserList = customers;
            #endregion

            #region OrderByID

            SqlCommand command = Command("PR_Order_SelectByPK");
            command.Parameters.AddWithValue("@OrderID", OrderID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrdersModel orderModel = new OrdersModel();

            foreach (DataRow @row in table.Rows)
            {
                orderModel.OrderID = Convert.ToInt32(@row["OrderID"]);
                orderModel.OrderDate = Convert.ToDateTime(@row["OrderDate"]);
                orderModel.CustomerID = Convert.ToInt32(@row["CustomerID"]);
                orderModel.PaymentMode = @row["PaymentMode"].ToString();
                orderModel.TotalAmount = Convert.ToDecimal(@row["TotalAmount"]);
                orderModel.ShippingAddress = @row["ShippingAddress"].ToString();
                orderModel.UserID = Convert.ToInt32(@row["UserID"]);
            }

            #endregion

            Dropdown("PR_Customer_DropDown",false);
            Dropdown("PR_User_DropDown", true);
            return View("OrderForm",orderModel);
        }
        public IActionResult OrderSave(OrdersModel ordermodel)
        {
            if (ordermodel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }
            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(ordermodel.OrderID == null ? "PR_Order_Insert" : "PR_Order_UpdateByPK"))
                {
                    if (ordermodel.OrderID != null)
                    {
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = ordermodel.OrderID;
                    }

                    command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = ordermodel.OrderDate;
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = ordermodel.CustomerID;
                    command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = ordermodel.PaymentMode;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = ordermodel.TotalAmount;
                    command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = ordermodel.ShippingAddress;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = ordermodel.UserID;

                    command.ExecuteNonQuery();
                }
                return RedirectToAction("OrderList");
            }
            Dropdown("PR_Customer_DropDown", false);
            Dropdown("PR_User_DropDown", true);
            return View("OrderForm",ordermodel);
        }

        public IActionResult DataToExcel() {
            return ExportToExcel("PR_Order_SelectAll", "OrderData.xlsx", "Order Data");
        }
    }
}
