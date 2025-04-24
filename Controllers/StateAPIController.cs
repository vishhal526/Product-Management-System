using Product_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
//using Mono.TextTemplating;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace Product_Management_System.Controllers
{
    public class StateAPIController : BaseController
    {
        private readonly HttpClient _client;

        #region StateIConfiguration
        public StateAPIController(IConfiguration _configuration) : base(_configuration)
        {
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }
        #endregion

        #region GetStateBYApi
        [HttpGet]
        public async Task<IActionResult> StateList()
        {
            var response = await _client.GetAsync("State/all");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var states = JsonConvert.DeserializeObject<List<StateModel>>(data);
                return View(states);
            }
            return View(new List<StateModel>());
        }
        #endregion

        #region StateForm
        public async Task<IActionResult> StateForm(int? StateID)
        {
            Console.WriteLine($"StateID: {StateID}");
            await LoadCountryList();
            if (StateID.HasValue)
            {
                var response = await _client.GetAsync($"State/{StateID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var State = JsonConvert.DeserializeObject<StateModel>(data);
                    return View(State);
                }
            }
            return View(new StateModel());
        }
        #endregion

        #region StateSaveWithDebug
        //public async Task<IActionResult> StateSave(StateModel State)
        //{

        //    if (State == null)
        //    {
        //        ModelState.AddModelError("", "State data is missing.");
        //        return View("StateForm", State);
        //    }

        //    Console.WriteLine($"StateID: {State.StateID}");
        //    Console.WriteLine($"StateName: {State.StateName}");
        //    Console.WriteLine($"StateCode: {State.StateCode}");
        //    Console.WriteLine($"CountryID: {State.CountryID}");

        //    if (ModelState.IsValid)
        //    {
        //        var json = JsonConvert.SerializeObject(State);
        //        Console.WriteLine($"Payload to API: {json}"); // Log the JSON payload
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response;

        //        if (State.StateID == null) // Add
        //            response = await _client.PostAsync("State", content);
        //        else // Update
        //            response = await _client.PutAsync($"State/{State.StateID}", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("StateList");
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine($"API Error Response: {errorResponse}");
        //            ModelState.AddModelError("", "Error occurred while saving State data.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("ModelState is not valid!");
        //        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        //        Console.WriteLine("ModelState Errors: " + string.Join(", ", errors));
        //    }

        //    await LoadCountryList();
        //    return View("StateForm", State);
        //}

        #endregion

        #region StateSave
        [HttpPost]
        public async Task<IActionResult> StateSave(StateModel State)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(State);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (State.StateID == null)
                    response = await _client.PostAsync("State", content);
                else
                    response = await _client.PutAsync($"State/{State.StateID}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("StateList");
            }
            await LoadCountryList();
            return View("StateForm", State);
        }

        #endregion

        #region StateDelete
        public async Task<IActionResult> StateDelete(int StateID)
        {
                var response = await _client.DeleteAsync($"State/{StateID}");
            if (response.IsSuccessStatusCode) {
                TempData["Success"] = "Data Deletion is Completed";
            }

            else
            {
                TempData["ErrorMessage"] = "Data Could not be deleted as it is being used as a reference in other table";
            }
            return RedirectToAction("StateList");
        }
        #endregion

        #region CountryDropDown
        private async Task LoadCountryList()
        {
            var response = await _client.GetAsync("State/CountriesDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<List<CountryDropDownModel>>(data);
                ViewBag.CountryList = countries;
            }
        }
        #endregion

        #region DaataToExcel
        public IActionResult DataToExcel()
        {
            return ExportToExcel("PR_LOC_State_SelectAll", "StateData.xlsx", "State Data");
        }
        #endregion
    }
}
