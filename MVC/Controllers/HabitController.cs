using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class HabitController : Controller
    {
        // The Definition of Base URL
        public const string baseUrl = "https://localhost:44378/";
        readonly Uri ClientBaseAddress = new(baseUrl);
        readonly HttpClient clnt;

        // Constructor for initiating request to the given base URL publicly
        public HabitController()
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


        // GET: Habit
        public async Task<ActionResult> Index()
        {
            // Creating the list of new Habits list
            List<Habit> HabitInfo = new();

            HeaderClearing();

            // Sending Request to the find web api Rest service resources using HTTPClient
            HttpResponseMessage httpResponseMessage = await clnt.GetAsync("api/Habit");

            // If the request is success
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // storing the web api data into model that was predefined prior
                var responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;

                HabitInfo = JsonConvert.DeserializeObject<List<Habit>>(responseMessage);
            }
            return View(HabitInfo);
        }




        // POST: HabitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Habit habit)
        {
            if (ModelState.IsValid)
            {
                string createHabitInfo = JsonConvert.SerializeObject(habit);

                // creating string content to pass as Http content later
                StringContent stringContentInfo = new(createHabitInfo, Encoding.UTF8, "application/json");

                // Making a Post request
                HttpResponseMessage createHttpResponseMessage = clnt.PostAsync(clnt.BaseAddress + "api/Habit/", stringContentInfo).Result;
                if (createHttpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(habit);
        }

        // GET: Habit/Create
        public ActionResult Create()
        {
            return View();
        }


        // GET: HabitController/Details/
        public ActionResult Details(int id)
        {
            //Creating a Get Request to get single Habit
            Habit habitDetails = new();

            HeaderClearing();

            HttpResponseMessage httpResponseMessageDetails = clnt.GetAsync(clnt.BaseAddress + "api/Habit/" + id).Result;

            // Checking for response state
            if (httpResponseMessageDetails.IsSuccessStatusCode)
            {
                string detailsInfo = httpResponseMessageDetails.Content.ReadAsStringAsync().Result;
                habitDetails = JsonConvert.DeserializeObject<Habit>(detailsInfo);
            }
            return View(habitDetails);
        }


        // GET: HabitController/Edit/
        public async Task<ActionResult> Edit(int id)
        {
            //Creating a Get Request to get single Habit
            Habit habitDetails = new();

            HeaderClearing();


            HttpResponseMessage httpResponseMessageDetails = await clnt.GetAsync(clnt.BaseAddress + "api/Habit/" + id);

            // Checking for response state
            if (httpResponseMessageDetails.IsSuccessStatusCode)
            {
                // storing the response details received from web api 
                string detailsInfo = httpResponseMessageDetails.Content.ReadAsStringAsync().Result;
                habitDetails = JsonConvert.DeserializeObject<Habit>(detailsInfo);
                // Create a SelectList object containing the repeat options
                SelectList repeatOptions = new (new[] { "Daily", "Weekly", "Monthly" });

                // Pass the SelectList object and the selected value to the view
                ViewData["Repeat"] = new SelectList(repeatOptions.Items, habitDetails.Repeat);
            }
            return View(habitDetails);
        }


        // POST: HabitController/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Habit habit)
        {
            if (ModelState.IsValid)
            {
                // serializing habit object into json format to send
                string editHabitInfo = JsonConvert.SerializeObject(habit);

                // creating string content to pass as Http content later
                StringContent stringContentInfo = new(editHabitInfo, Encoding.UTF8, "application/json");

                // Making a Put request
                HttpResponseMessage editHttpResponseMessage = await clnt.PutAsync(clnt.BaseAddress + "api/Habit/" + id, stringContentInfo);
                if (editHttpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(habit);
        }

        // GET: HabitController/
        public async Task<ActionResult> Delete(int id)
        {
            //Creating a Get Request to get single Habit
            Habit habitDetails = new();

            HeaderClearing();


            HttpResponseMessage httpResponseMessageDetails = await clnt.GetAsync(clnt.BaseAddress + "api/Habit/" + id);

            // Checking for response state
            if (httpResponseMessageDetails.IsSuccessStatusCode)
            {
                // storing the response details received from web api 
                string detailsInfo = httpResponseMessageDetails.Content.ReadAsStringAsync().Result;

                // deserializing the response
                habitDetails = JsonConvert.DeserializeObject<Habit>(detailsInfo);
            }
            return View(habitDetails);
        }

        // POST: HabitController/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage httpResponseMessage = await clnt.DeleteAsync(clnt.BaseAddress + "api/Habit/" + id);

            //Checking the response is successful or not which is sent using HttpClient  
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
