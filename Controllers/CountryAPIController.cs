using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Product_Management_System.Controllers
{
    public class CountryAPIController : BaseController
    {

        private readonly HttpClient _client;

        #region CountryIConfiguration
        public CountryAPIController(IConfiguration _configuration) : base(_configuration)
        {
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }
        #endregion

        #region GetCountryBYApi
        [HttpGet]
        public async Task<IActionResult> CountryList()
        {
            var response = await _client.GetAsync("Country/all");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<List<CountryModel>>(data);
                return View(countries);
            }
            return View(new List<CountryModel>());
        }
        #endregion

        #region CountryForm
        public async Task<IActionResult> CountryForm(int? CountryID)
        {
            Console.WriteLine($"CountryID: {CountryID}");
            if (CountryID.HasValue)
            {
                var response = await _client.GetAsync($"Country/{CountryID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var Country = JsonConvert.DeserializeObject<CountryModel>(data);
                    return View(Country);
                }
            }
            return View(new CountryModel());
        }
        #endregion

        #region CountrySaveWithDebug
        //public async Task<IActionResult> CountrySave(CountryModel Country)
        //{

        //    if (Country == null)
        //    {
        //        ModelCountry.AddModelError("", "Country data is missing.");
        //        return View("CountryForm", Country);
        //    }

        //    Console.WriteLine($"CountryID: {Country.CountryID}");
        //    Console.WriteLine($"CountryName: {Country.CountryName}");
        //    Console.WriteLine($"CountryCode: {Country.CountryCode}");
        //    Console.WriteLine($"CountryID: {Country.CountryID}");

        //    if (ModelCountry.IsValid)
        //    {
        //        var json = JsonConvert.SerializeObject(Country);
        //        Console.WriteLine($"Payload to API: {json}"); // Log the JSON payload
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response;

        //        if (Country.CountryID == null) // Add
        //            response = await _client.PostAsync("Country", content);
        //        else // Update
        //            response = await _client.PutAsync($"Country/{Country.CountryID}", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("CountryList");
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine($"API Error Response: {errorResponse}");
        //            ModelCountry.AddModelError("", "Error occurred while saving Country data.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("ModelCountry is not valid!");
        //        var errors = ModelCountry.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        //        Console.WriteLine("ModelCountry Errors: " + string.Join(", ", errors));
        //    }

        //    await LoadCountryList();
        //    return View("CountryForm", Country);
        //}

        #endregion

        #region CountrySave
        [HttpPost]
        public async Task<IActionResult> CountrySave(CountryModel Country)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Country);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (Country.CountryID == null)
                    response = await _client.PostAsync("Country", content);
                else
                    response = await _client.PutAsync($"Country/{Country.CountryID}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("CountryList");
            }

            return View("CountryForm", Country);
        }

        #endregion

        #region CountryDelete
        public async Task<IActionResult> CountryDelete(int CountryID)
        {
            var response = await _client.DeleteAsync($"Country/{CountryID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Data Deletion is Completed";
            }

            else
            {
                TempData["ErrorMessage"] = "Data Could not be deleted as it is being used as a reference in other table";
            }
            return RedirectToAction("CountryList");
        }
        #endregion

    }
}
