using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;

namespace Product_Management_System.Controllers
{
    public class CityAPIController : BaseController
    {
        //Uri baseAddress = new Uri("https://localhost:7245/api");
        private readonly HttpClient _client;

        #region CityIConfiguration
        public CityAPIController(IConfiguration _configuration) : base(_configuration)
        {
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }
        #endregion

        #region GetCityBYApi
        [HttpGet]
        public async Task<IActionResult> CityList()
        {
            var response = await _client.GetAsync("City/all");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var cities = JsonConvert.DeserializeObject<List<CityModel>>(data);
                return View(cities);
            }
            return View(new List<CityModel>());
        }
        #endregion

        #region CityForm
        public async Task<IActionResult> CityForm(int? CityID)
        {
            await LoadCountryList();
            if (CityID.HasValue)
            {
                var response = await _client.GetAsync($"City/{CityID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var city = JsonConvert.DeserializeObject<CityModel>(data);
                    ViewBag.StateList = await GetStatesByCountryID(city.CountryID);
                    return View(city);
                }
            }
            return View(new CityModel());
        }
        #endregion

        #region CountryDropDown
        private async Task LoadCountryList()
        {
            var response = await _client.GetAsync("City/CountriesDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<List<CountryDropDownModel>>(data);
                ViewBag.CountryList = countries;
            }
        }
        #endregion

        #region JsonStateDropDown
        [HttpPost]
        public async Task<JsonResult> GetStatesByCountry(int CountryID)
        {
            var states = await GetStatesByCountryID(CountryID);
            return Json(states);
        }
        #endregion

        #region StateDropDown
        private async Task<List<StateDropDownModel>> GetStatesByCountryID(int CountryID)
        {
            var response = await _client.GetAsync($"City/StatesDropdown/{CountryID}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<StateDropDownModel>>(data);
            }
            return new List<StateDropDownModel>();
        }
        #endregion

        #region CitySave
        [HttpPost]

        #region CitySaveWithDebug
        //public async Task<IActionResult> CitySave(CityModel city)
        //{

        //    if (city == null)
        //    {
        //        ModelState.AddModelError("", "City data is missing.");
        //        return View("CityForm", city);
        //    }

        //    Console.WriteLine($"CityID: {city.CityID}");
        //    Console.WriteLine($"CityName: {city.CityName}");
        //    Console.WriteLine($"CityCode: {city.CityCode}");
        //    Console.WriteLine($"CountryID: {city.CountryID}");
        //    Console.WriteLine($"StateID: {city.StateID}");

        //    if (ModelState.IsValid)
        //    {
        //        var json = JsonConvert.SerializeObject(city);
        //        Console.WriteLine($"Payload to API: {json}"); // Log the JSON payload
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response;

        //        if (city.CityID == null) // Add
        //            response = await _client.PostAsync("City", content);
        //        else // Update
        //            response = await _client.PutAsync($"City/{city.CityID}", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("CityList");
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine($"API Error Response: {errorResponse}");
        //            ModelState.AddModelError("", "Error occurred while saving city data.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("ModelState is not valid!");
        //        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        //        Console.WriteLine("ModelState Errors: " + string.Join(", ", errors));
        //    }

        //    await LoadCountryList();
        //    return View("CityForm", city);
        //}
        #endregion

        public async Task<IActionResult> CitySave(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(city);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (city.CityID == null)
                    response = await _client.PostAsync("City", content);
                else
                    response = await _client.PutAsync($"City/{city.CityID}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("CityList");
            }
            await LoadCountryList();
            return View("CityForm", city);
        }

        #endregion

        #region CityDelete
        public async Task<IActionResult> CityDelete(int CityID)
        {
            var response = await _client.DeleteAsync($"City/{CityID}");
            return RedirectToAction("CityList");
        }
        #endregion

        #region DataToExcel
        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_LOC_City_SelectAll", "CityData.xlsx", "City Data");
        }
        #endregion

    }
}
