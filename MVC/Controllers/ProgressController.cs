using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace MVC.Controllers
{
    public class ProgressController : Controller
    {
        // The Definition of Base URL
        public const string baseUrl = "https://localhost:44378/";
        readonly Uri ClientBaseAddress = new Uri(baseUrl);
        readonly HttpClient clnt;

        // Constructor for initiating request to the given base URL publicly
        public ProgressController()
        {
            clnt = new HttpClient
            {
                BaseAddress = ClientBaseAddress
            };
        }

        public void HeaderClearing()
        {
            // Clearing default headers
            clnt.DefaultRequestHeaders.Clear();

            // Define the request type of the data
            clnt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Progress
        public async Task<ActionResult> Index()
        {
            // Creating the list of new Progress list
            List<Progress> ProgressInfo = new List<Progress>();

            HeaderClearing();

            // Sending Request to the find web api Rest service resources using HTTPClient
            HttpResponseMessage httpResponseMessage = await clnt.GetAsync("api/Progress");

            // If the request is success
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // storing the web api data into model that was predefined prior
                var responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;

                ProgressInfo = JsonConvert.DeserializeObject<List<Progress>>(responseMessage);
            }
            return View(ProgressInfo);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Make a GET request to the Habit API endpoint to get the habit names data
            HttpResponseMessage response = await clnt.GetAsync("api/Habit");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string content = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string to a list of Habit objects
                var habits = JsonConvert.DeserializeObject<List<Habit>>(content);

                // Create a list of SelectListItem to hold the habit names
                var habitNames = new List<SelectListItem>();

                // Add each habit name to the list
                foreach (var habit in habits)
                {
                    habitNames.Add(new SelectListItem { Value = habit.ID.ToString(), Text = habit.Name });
                }

                // Pass the list of habit names to the view using the ViewData dictionary
                ViewData["HabitNames"] = habitNames;
            }
            else
            {
                ViewData["HabitNames"] = new List<SelectListItem>();
            }

            return View();
        }


        // POST: ProgressController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Progress progress)
        {
            //  progress.Habit = new Habit { ID = progress.Habit.ID };
            if (ModelState.IsValid)
            {
                string createProgressInfo = JsonConvert.SerializeObject(progress);

                // creating string content to pass as Http content later
                StringContent stringContentInfo = new StringContent(createProgressInfo, Encoding.UTF8, "application/json");

                // Making a Post request
                HttpResponseMessage createHttpResponseMessage = clnt.PostAsync(clnt.BaseAddress + "api/Progress/", stringContentInfo).Result;
                if (createHttpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(progress);
        }

        // GET: ProgressController/Details/5
        public ActionResult Details(int id)
        {
            //Creating a Get Request to get single Progress
            Progress progressDetails = new Progress();

            HeaderClearing();


            HttpResponseMessage httpResponseMessageDetails = clnt.GetAsync(clnt.BaseAddress + "api/Progress/" + id).Result;

            if (httpResponseMessageDetails.IsSuccessStatusCode)
            {
                string detailsInfo = httpResponseMessageDetails.Content.ReadAsStringAsync().Result;

                progressDetails = JsonConvert.DeserializeObject<Progress>(detailsInfo);
            }
            return View(progressDetails);
        }


        // GET: ProgressController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HeaderClearing();

            HttpResponseMessage responseMessageDetails = await clnt.GetAsync($"api/Progress/{id}");
            if (!responseMessageDetails.IsSuccessStatusCode)
            {
                return View(new Progress());
            }

            var progressDetails = JsonConvert.DeserializeObject<Progress>(await responseMessageDetails.Content.ReadAsStringAsync());

            HttpResponseMessage response = await clnt.GetAsync("api/Habit");
            if (response.IsSuccessStatusCode)
            {
                var habits = JsonConvert.DeserializeObject<List<Habit>>(await response.Content.ReadAsStringAsync());

                var habitNames = habits.Select(h => new SelectListItem { Value = h.ID.ToString(), Text = h.Name }).ToList();

                var selectedHabit = habitNames.FirstOrDefault(x => x.Value == progressDetails.Habit.ID.ToString());
                if (selectedHabit != null)
                {
                    selectedHabit.Selected = true;
                }

                ViewData["HabitNames"] = habitNames;
            }
            else
            {
                ViewData["HabitNames"] = new List<SelectListItem>();
            }

            return View(progressDetails);
        }



        // POST: ProgressController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Progress progress)
        {

            if (ModelState.IsValid)
            {
                // serializing habit object into json format to send
                string editProgressInfo = JsonConvert.SerializeObject(progress);

                // creating string content to pass as Http content later
                StringContent stringContentInfo = new StringContent(editProgressInfo, Encoding.UTF8, "application/json");

                // Making a Put request
                HttpResponseMessage editHttpResponseMessage = await clnt.PutAsync(clnt.BaseAddress + "api/Progress/" + id, stringContentInfo);
                if (editHttpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(progress);
        }



        // GET: ProgressController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Progress progressDetails = new Progress();
            HeaderClearing();
            HttpResponseMessage response = await clnt.GetAsync(clnt.BaseAddress + "api/Progress/" + id);

            if (response.IsSuccessStatusCode)
            {
                string detailsInfo = response.Content.ReadAsStringAsync().Result;

                progressDetails = JsonConvert.DeserializeObject<Progress>(detailsInfo);
            }
            return View(progressDetails);
        }

        // POST: ProgressController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await clnt.DeleteAsync(clnt.BaseAddress + "api/Progress/" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
