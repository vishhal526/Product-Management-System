using ClosedXML.Excel;
using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Product_Management_System.Controllers
{
    public class BillController : BaseController
    {
        public BillController(IConfiguration _configuration) : base(_configuration)
        {}

        public void Dropdown(string STPROC, bool model)
        {
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
            else
            {
                List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
                foreach (DataRow data in table.Rows)
                {
                    OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                    orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                    orderList.Add(orderDropDownModel);
                }
                ViewBag.OrderList = orderList;
            }
        }

        public IActionResult BillList()
        {
            SqlCommand command = Command("PR_Bill_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult BillDelete(int BillID)
        {
            try
            {
                SqlCommand command = Command("PR_Bill_DeleteByPK");
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                command.ExecuteNonQuery();
                return RedirectToAction("BillList");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data ";
                Console.WriteLine(e.ToString());
                return RedirectToAction("BillList");
            }

        }

        public IActionResult BillForm(int BillID)
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

            #region Order Drop-down
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command2 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader2 = command1.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            connection2.Close();

            List<OrderDropDownModel> orders = new List<OrderDropDownModel>();

            foreach (DataRow dataRow in dataTable2.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(dataRow["OrderID"]);
                orders.Add(orderDropDownModel);
            }

            ViewBag.OrderList = orders;
            #endregion

            #region OrderByID

            SqlCommand command = Command("PR_Bill_SelectByPK");
            command.Parameters.AddWithValue("@BillID", BillID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            BillModel billmodel = new BillModel();

            foreach (DataRow dataRow in table.Rows)
            {
                billmodel.BillID = Convert.ToInt32(@dataRow["BillID"]);
                billmodel.BillNo = @dataRow["BillNumber"].ToString();
                billmodel.BillDate = Convert.ToDateTime(@dataRow["BillDate"]);
                billmodel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                billmodel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                billmodel.Discount = Convert.ToDecimal(@dataRow["Discount"]);
                billmodel.NetAmount = Convert.ToDecimal(@dataRow["NetAmount"]);
                billmodel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("BillForm", billmodel);
        }
        
        public IActionResult BillSave(BillModel billModel)
        {
            if (billModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(billModel.BillID == null ? "PR_Bill_Insert" : "PR_Bill_UpdateByPK"))
                {
                    if (billModel.BillID != null)
                    {
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value = billModel.BillID;
                    }

                    command.Parameters.Add("@BIllNumber", SqlDbType.VarChar).Value = billModel.BillNo;
                    command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billModel.BillDate;
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billModel.OrderID;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billModel.TotalAmount;
                    command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billModel.Discount;
                    command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billModel.NetAmount;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = billModel.UserID;

                    command.ExecuteNonQuery();
                }
                return RedirectToAction("BillList");
            }
            Dropdown("PR_Order_DropDown", false);
            Dropdown("PR_User_DropDown", true);
            return View("BillForm",billModel);

        }

        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_Bill_SelectAll", "BillData.xlsx", "Bill Data");
        }
    }
}
