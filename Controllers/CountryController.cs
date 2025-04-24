using Product_Management_System.Helper;
using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Product_Management_System.Controllers
{
    public class CountryController : BaseController
    {
        #region CountryDBIConfiguration
        public CountryController(IConfiguration _configuration) : base(_configuration)
        { }
        #endregion

        #region CountryList
        public IActionResult CountryList()
        {
            SqlCommand command = Command("PR_LOC_Country_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region CountryDelete
        public IActionResult CountryDelete(string CountryID)
        {
            int decryptedCountryID = Convert.ToInt32(UrlEncryptor.Decrypt(CountryID.ToString()));
            try
            {
                SqlCommand command = Command("PR_LOC_Country_DeleteByPK");
                command.Parameters.AddWithValue("@CountryID", decryptedCountryID);
                command.ExecuteNonQuery();
                return RedirectToAction("Countrylist");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data";
                Console.WriteLine(e.ToString);
                return RedirectToAction("Countrylist");
            }

        }
        #endregion

        #region CountryForm
        public IActionResult CountryForm(string CountryID)
        {
            int? decryptedCountryID = null;
            DataTable table = new DataTable();

            if (!string.IsNullOrEmpty(CountryID))
            {

                string decryptedCountryIDString = UrlEncryptor.Decrypt(CountryID);
                decryptedCountryID = int.Parse(decryptedCountryIDString);

            }

            if (decryptedCountryID.HasValue)
            {

                SqlCommand command = Command("PR_LOC_Country_SelectByPK");
                command.Parameters.AddWithValue("@CountryID", decryptedCountryID);
                SqlDataReader reader = command.ExecuteReader();
                table.Load(reader);

            }

            CountryModel model = new CountryModel();

            if (table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.CountryName = dr["CountryName"].ToString();
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.CountryCode = dr["CountryCode"].ToString();
                }
                return View("CountryForm", model);
            }

            return View("CountryForm", model);
        }
        #endregion

        #region CountrySave
        [HttpPost]
        public IActionResult CountrySave([Bind("CountryName,CountryCode")] CountryModel CountryModel)
        {
            string DecryptedCountryID = UrlEncryptor.Decrypt(Request.Form["CountryID"]);

            if (!string.IsNullOrEmpty(DecryptedCountryID))
            {
                CountryModel.CountryID = Convert.ToInt32(DecryptedCountryID);
            }
            if (ModelState.IsValid)
            {
                using (SqlCommand command = Command(CountryModel.CountryID == 0 ? "PR_LOC_Country_Insert" : "PR_LOC_Country_UpdateByPK"))
                {
                    if (CountryModel.CountryID != 0)
                    {
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryModel.CountryID;
                    }

                    command.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = CountryModel.CountryName;
                    command.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = CountryModel.CountryCode;

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("CountryList");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                ModelState.AddModelError("", "There were errors in the form submission.");
            }
            return View("CountryForm", CountryModel);
        }
        #endregion
    }
}
