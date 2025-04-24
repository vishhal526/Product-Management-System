using Product_Management_System.Helper;
using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.InkML;

namespace Product_Management_System.Controllers
{
    public class StateController : BaseController
    {
        #region StateDBIConfiguration
        public StateController(IConfiguration _configuration) : base(_configuration)
        { }
        #endregion

        #region StateList
        public IActionResult StateList()
        {
            SqlCommand command = Command("PR_LOC_State_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region StateDelete
        public IActionResult StateDelete(string StateID)
        {
            int decryptedStateID = Convert.ToInt32(UrlEncryptor.Decrypt(StateID.ToString()));
            try
            {
                SqlCommand command = Command("PR_LOC_State_DeleteByPK");
                command.Parameters.AddWithValue("@StateID", decryptedStateID);
                command.ExecuteNonQuery();
                return RedirectToAction("Statelist");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data";
                Console.WriteLine(e.ToString);
                return RedirectToAction("Statelist");
            }

        }
        #endregion

        #region StateForm
        public IActionResult StateForm(string StateID)
        {
            LoadCountryList();
            int? decryptedStateID = null;
            DataTable table = new DataTable();

            if (!string.IsNullOrEmpty(StateID))
            {

                string decryptedStateIDString = UrlEncryptor.Decrypt(StateID);
                decryptedStateID = int.Parse(decryptedStateIDString);

            }

            if (decryptedStateID.HasValue)
            {

                SqlCommand command = Command("PR_LOC_State_SelectByPK");
                command.Parameters.AddWithValue("@StateID", decryptedStateID);
                SqlDataReader reader = command.ExecuteReader();
                table.Load(reader);

            }

            StateModel model = new StateModel();

            if (table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    model.StateID = Convert.ToInt32(dr["StateID"]);
                    model.StateName = dr["StateName"].ToString();
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.StateCode = dr["StateCode"].ToString();
                }
                return View("StateForm", model);
            }

            return View("StateForm", model);
        }
        #endregion

        #region StateSave
        [HttpPost]
        public IActionResult StateSave([Bind("StateName,StateCode,CountryID")] StateModel StateModel)
        {
            string DecryptedStateID = UrlEncryptor.Decrypt(Request.Form["StateID"]);

            if (!string.IsNullOrEmpty(DecryptedStateID))
            {
                StateModel.StateID = Convert.ToInt32(DecryptedStateID);
            }
            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(StateModel.StateID == 0 ? "PR_LOC_State_Insert" : "PR_LOC_State_UpdateByPK"))
                {
                    if (StateModel.StateID != 0)
                    {
                        command.Parameters.Add("@StateID", SqlDbType.Int).Value = StateModel.StateID;
                    }

                    command.Parameters.Add("@StateName", SqlDbType.VarChar).Value = StateModel.StateName;
                    command.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = StateModel.StateCode;
                    command.Parameters.Add("@CountryID", SqlDbType.Int).Value = StateModel.CountryID;

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("StateList");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                ModelState.AddModelError("", "There were errors in the form submission.");
            }
            LoadCountryList();
            return View("StateForm", StateModel);
        }
        #endregion

        #region Load Country List
        private void LoadCountryList()
        {
            SqlCommand command = Command("PR_LOC_Country_Dropdown");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);


            List<CountryDropDownModel> countryList = new List<CountryDropDownModel>();
            foreach (DataRow dr in table.Rows)
            {
                countryList.Add(new CountryDropDownModel
                {
                    CountryID = Convert.ToInt32(dr["CountryID"]),
                    CountryName = dr["CountryName"].ToString()
                });
            }
            ViewBag.CountryList = countryList;
        }
        #endregion

    }
}
