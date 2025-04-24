using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Product_Management_System.Controllers
{
    public class OrderDetailController : BaseController
    {
        public OrderDetailController(IConfiguration _configuration) : base(_configuration)
        { }
        public IActionResult OrderDetailList()
        {
            SqlCommand command = Command("PR_OrderDetail_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public void DropDown(string STPROC, int model)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            if (model == 1)
            {
                command1.CommandText = "PR_Product_DropDown";
            }
            else if (model == 2)
            {
                command1.CommandText = "PR_Order_DropDown";
            }
            else {
                command1.CommandText = "PR_User_DropDown";
            }
            
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader1);
            connection1.Close();
            switch (model)
            {
                case 1:
                    List<ProductDropDownModel> productList = new List<ProductDropDownModel>();
                    foreach (DataRow data in dataTable.Rows)
                    {
                        ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                        productDropDownModel.ProductID = Convert.ToInt32(data["ProductID"]);
                        productDropDownModel.ProductName = data["ProductName"].ToString();
                        productList.Add(productDropDownModel);
                    }
                    ViewBag.ProductList = productList;
                    break;
                case 2:
                    List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
                    foreach (DataRow data in dataTable.Rows)
                    {
                        OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                        orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                        orderList.Add(orderDropDownModel);
                    }
                    ViewBag.OrderList = orderList;
                    break;
                    default:
                    List<UserDropDownModel> userList = new List<UserDropDownModel>();
                    foreach (DataRow data in dataTable.Rows)
                    {
                        UserDropDownModel userDropDownModel = new UserDropDownModel();
                        userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                        userDropDownModel.UserName = data["UserName"].ToString();
                        userList.Add(userDropDownModel);
                    }
                    ViewBag.UserList = userList;
                    break;
            }

        }

        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
            try
            {
                SqlCommand command = Command("PR_OrderDetail_DeleteByPK");
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderDetailList");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data";
                Console.WriteLine(e.ToString);
                return RedirectToAction("OrderDetailList");
            }
        }
        public IActionResult OrderDetailForm(int OrderDetailID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            DropDown("PR_User_DropDown", 3);
            #region User Drop-Down
            //SqlConnection connection1 = new SqlConnection(connectionString);
            //connection1.Open();
            //SqlCommand command1 = connection1.CreateCommand();
            //command1.CommandType = System.Data.CommandType.StoredProcedure;
            //command1.CommandText = "PR_User_DropDown";
            //SqlDataReader reader1 = command1.ExecuteReader();
            //DataTable dataTable1 = new DataTable();
            //dataTable1.Load(reader1);
            //connection1.Close();

            //List<UserDropDownModel> users = new List<UserDropDownModel>();

            //foreach (DataRow dataRow in dataTable1.Rows)
            //{
            //    UserDropDownModel userDropDownModel = new UserDropDownModel();
            //    userDropDownModel.UserID = Convert.ToInt32(dataRow["UserID"]);
            //    userDropDownModel.UserName = dataRow["UserName"].ToString();
            //    users.Add(userDropDownModel);
            //}

            //ViewBag.UserList = users;
            #endregion

            DropDown("PR_Product_DropDown", 1);
            #region Product Dropdown
            //SqlConnection connection2 = new SqlConnection(connectionString);
            //connection2.Open();
            //SqlCommand command2 = connection2.CreateCommand();
            //command2.CommandType = CommandType.StoredProcedure;
            //command2.CommandText = "PR_Product_DropDown";
            //SqlDataReader reader2 = command2.ExecuteReader();
            //DataTable dataTable2 = new DataTable();
            //dataTable2.Load(reader2);
            //connection2.Close();

            //List<ProductDropDownModel> products = new List<ProductDropDownModel>();

            //foreach (DataRow i in dataTable2.Rows)
            //{
            //    ProductDropDownModel productDropDownModel = new ProductDropDownModel();
            //    productDropDownModel.ProductID = Convert.ToInt32(i["ProductID"]);
            //    productDropDownModel.ProductName = i["ProductName"].ToString();
            //    products.Add(productDropDownModel);
            //}

            //ViewBag.ProductList = products;

            #endregion

            DropDown("PR_Order_DropDown", 2);
            #region Order Dropdown
            //SqlConnection connection3 = new SqlConnection(connectionString);
            //connection3.Open();
            //SqlCommand command3 = connection3.CreateCommand();
            //command3.CommandType = CommandType.StoredProcedure;
            //command3.CommandText = "PR_Order_DropDown";
            //SqlDataReader reader3 = command3.ExecuteReader();
            //DataTable dataTable3 = new DataTable();
            //dataTable3.Load(reader3);
            //connection3.Close();

            //List<OrderDropDownModel> orders = new List<OrderDropDownModel>();

            //foreach (DataRow i in dataTable3.Rows)
            //{ 
            //    OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
            //    orderDropDownModel.OrderID = Convert.ToInt32(i["OrderID"]);
            //    orderDropDownModel.OrderNo = i["OrderNo"].ToString();
            //    orders.Add(orderDropDownModel);
            //}

            //ViewBag.OrderList = orders;

            #endregion

            #region OrderDetailByID

            SqlCommand command = Command("PR_OrderDetail_SelectByPK");
            command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderDetailModel odModel = new OrderDetailModel();

            foreach (DataRow @row in table.Rows)
            {
                odModel.OrderDetailID = Convert.ToInt32(@row["OrderDetailID"]);
                odModel.OrderID = Convert.ToInt32(@row["OrderID"]);
                odModel.ProductID = Convert.ToInt32(@row["ProductID"]);
                odModel.Quantity = Convert.ToInt32(@row["Quantity"]);
                odModel.Amount = Convert.ToDecimal(@row["Amount"]);
                odModel.TotalAmount = Convert.ToDecimal(@row["TotalAmount"]);
                odModel.UserID = Convert.ToInt32(@row["UserID"]);
            }

            #endregion


            return View("OrderDetailForm", odModel);
        }
        public IActionResult OrderDetailSave(OrderDetailModel odmodel)
        {
            if (odmodel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }
            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(odmodel.OrderDetailID == null ? "PR_OrderDetail_Insert" : "PR_OrderDetail_UpdateByPK")) 
                {
                    if (odmodel.OrderDetailID != null) 
                    {
                        command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = odmodel.OrderDetailID;
                    }
                    
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = odmodel.OrderID;
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = odmodel.ProductID;
                        command.Parameters.Add("@Quantity", SqlDbType.Int).Value = odmodel.Quantity;
                        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = odmodel.Amount;
                        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = odmodel.TotalAmount;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = odmodel.UserID;
                    
                    command.ExecuteNonQuery();
                }
                    return RedirectToAction("OrderDetailList");
            }
            DropDown("PR_Product_DropDown", 1);
            DropDown("PR_Order_DropDown", 2);
            DropDown("PR_User_DropDown", 3);
            return View("OrderDetailForm",odmodel);
        }

        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_OrderDetail_SelectAll", "OrderDetailData.xlsx", "OrderDetail Data");
        }
    }
}
