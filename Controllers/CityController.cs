using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Product_Management_System.Helper;
using Product_Management_System.Models;

namespace Product_Management_System.Controllers
{
    public class CityController : BaseController
    {

        #region CityDBIConfiguration
        public CityController(IConfiguration _configuration) : base(_configuration)
        { }
        #endregion

        #region CityList
        public IActionResult CityList()
        {
            SqlCommand command = Command("PR_LOC_City_SelectAll");
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region CityDelete
        public IActionResult CityDelete(string CityID)
        {
            int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(CityID.ToString()));
            try
            {
                SqlCommand command = Command("PR_LOC_City_DeleteByPK");
                command.Parameters.AddWithValue("@CityID", decryptedCityID);
                command.ExecuteNonQuery();
                return RedirectToAction("Citylist");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Couldn't Delete the Data";
                Console.WriteLine(e.ToString);
                return RedirectToAction("Citylist");
            }

        }
        #endregion

        #region CityForm
        public IActionResult CityForm(string CityID)
        {
            LoadCountryList();
            int? decryptedCityID = null;
            DataTable table = new DataTable();

            if (!string.IsNullOrEmpty(CityID))
            {

                string decryptedCityIDString = UrlEncryptor.Decrypt(CityID);
                decryptedCityID = int.Parse(decryptedCityIDString);

            }

            if (decryptedCityID.HasValue)
            {

                SqlCommand command = Command("PR_LOC_City_SelectByPK");
                command.Parameters.AddWithValue("@CityID", decryptedCityID);
                SqlDataReader reader = command.ExecuteReader();
                table.Load(reader);

            }

            CityModel model = new CityModel();

            if (table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    model.CityID = Convert.ToInt32(dr["CityID"]);
                    model.CityName = dr["CityName"].ToString();
                    model.StateID = Convert.ToInt32(dr["StateID"]);
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.CityCode = dr["CityCode"].ToString();
                    ViewBag.StateList = GetStateByCountryID(model.CountryID);
                }
                GetStatesByCountry(model.CountryID);
                return View("CityForm", model);
            }

            return View("CityForm", model);
        }
        #endregion

        #region CitySave
        [HttpPost]
        public IActionResult CitySave([Bind("CityName,CityCode,CountryID,StateID")] CityModel CityModel)
        {
            string DecryptedCityID = UrlEncryptor.Decrypt(Request.Form["CityID"]);

            if (!string.IsNullOrEmpty(DecryptedCityID))
            {
                CityModel.CityID = Convert.ToInt32(DecryptedCityID);
            }

            if (ModelState.IsValid)
            {

                using (SqlCommand command = Command(CityModel.CityID == 0 ? "PR_LOC_City_Insert" : "PR_LOC_City_UpdateByPK"))
                {
                    if (CityModel.CityID != 0)
                    {
                        command.Parameters.Add("@CityID", SqlDbType.Int).Value = CityModel.CityID;
                    }

                    command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = CityModel.CityName;
                    command.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = CityModel.CityCode;
                    command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CityModel.CountryID;
                    command.Parameters.Add("@StateID", SqlDbType.Int).Value = CityModel.StateID;

                    command.ExecuteNonQuery();
                }
                return RedirectToAction("CityList");
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
            return View("CityForm", CityModel);
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

        #region Json States by Country
        [HttpPost]
        public JsonResult GetStatesByCountry(int CountryID)
        {
            List<StateDropDownModel> loc_State = GetStateByCountryID(CountryID);
            return Json(loc_State);
        }
        #endregion

        #region States by Country
        public List<StateDropDownModel> GetStateByCountryID(int CountryID)
        {

            List<StateDropDownModel> loc_State = new List<StateDropDownModel>();
            SqlCommand cmd = Command("PR_LOC_State_DropdownByCountryID");
            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            using (SqlDataReader objSDR = cmd.ExecuteReader())
            {
                if (objSDR.HasRows)
                {
                    while (objSDR.Read())
                    {
                        loc_State.Add(new StateDropDownModel
                        {
                            StateID = Convert.ToInt32(objSDR["StateID"]),
                            StateName = objSDR["StateName"].ToString()
                        });
                    }
                }
            }


            return loc_State;
        }
        #endregion

    }
}
