using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Product_Management_System.Controllers
{ 
    public class UserController : BaseController
    {
        public IActionResult Login()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = userName;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public UserController(IConfiguration _configuration) : base(_configuration)
        { }

        public IActionResult Userlist()
        {
            SqlCommand command = Command("PR_User_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult UserDelete(int UserID)
        {
            try {
                SqlCommand command = Command("PR_User_DeleteByPK");
                command.Parameters.Add("@UserID" ,SqlDbType.Int).Value = UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Userlist");
            }
            catch (Exception e) {
                TempData["ErrorMessage"] = "Couldn't Delete the Data";
                Console.WriteLine(e.ToString);
                return RedirectToAction("Userlist");
            }

        }

        public IActionResult UserSave(UserModel usermodel)
        {
            if (usermodel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }
            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(usermodel.UserID == null ? "PR_User_Insert" : "PR_User_UpdateByPK"))
                {
                    if (usermodel.UserID != null)
                    {
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = usermodel.UserID;
                    }
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = usermodel.UserName;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = usermodel.Email;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = usermodel.Password;
                    command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = usermodel.MobileNo;
                    command.Parameters.Add("@Address", SqlDbType.VarChar).Value = usermodel.Address;
                    command.Parameters.Add("@isActive", SqlDbType.Bit).Value = true;
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("UserList");
            }
            return View("UserForm", usermodel);
        }

        public IActionResult UserForm(int UserID)
        {
            #region UserByID
            SqlCommand command = Command("PR_User_SelectByPK");
            command.Parameters.AddWithValue("@UserID", UserID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            UserModel userModel = new UserModel();

            foreach (DataRow dataRow in table.Rows)
            {
                userModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                userModel.UserName = @dataRow["UserName"].ToString();
                userModel.Email = @dataRow["Email"].ToString();
                userModel.Password = @dataRow["Password"].ToString();
                userModel.MobileNo = @dataRow["MobileNo"].ToString();
                userModel.Address = @dataRow["Address"].ToString();
                userModel.isActive = Convert.ToBoolean(@dataRow["IsActive"]);
            }
            #endregion

            return View("UserForm", userModel);
        }

        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_User_SelectAll", "UserData.xlsx", "User Data");
        }

        [HttpPost]
        public IActionResult UserLogin(UserLoginModel ulm) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SqlCommand command = Command("PR_User_Login");
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = ulm.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = ulm.Password;

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow value in table.Rows)
                        {
                            HttpContext.Session.SetString("UserID", value["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", value["UserName"].ToString());
                        }
                        return RedirectToAction("ProductList", "Product");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "UserName or Password is Incorrect. Please Try Again.";
                        Console.WriteLine("There an Error in Username or Password");
                        return RedirectToAction("Login", "User");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
                Console.WriteLine("The error is something else like this = "+e.ToString());
                return View("Login", ulm); // Return to login view with error message
            }

            TempData["ErrorMessage"] = "Model is invalid. Please check the input.";
            return View("Login", ulm);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public IActionResult UserRegister(UserRegisterModel urm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SqlCommand command = Command("PR_User_Register");
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = urm.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = urm.Password;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = urm.Email;
                    command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = urm.MobileNo;
                    command.Parameters.Add("@Address", SqlDbType.VarChar).Value = urm.Address;
                    bool isActive = urm.isActive ?? true;
                    command.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                    command.ExecuteNonQuery();

                    HttpContext.Session.SetString("UserName", urm.UserName);
                    return RedirectToAction("ProductList", "Product");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }
            return RedirectToAction("Register");
        }
    }
}
