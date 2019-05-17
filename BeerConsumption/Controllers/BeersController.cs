using BeerConsumption.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BeerConsumption.Controllers
{
    public class BeersController : Controller
    {
        private static readonly HttpClient _client;
        static BeersController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:8000/api/");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));
        }

        // GET: Beers
        public async Task<ActionResult> Index()
        {
            try
            {
                var jsonAsString = await _client.GetStringAsync("beers");
                var beers = JsonConvert.DeserializeObject<List<Beer>>(jsonAsString);
                return View(beers);
            }
            catch (Exception ex)
            {
                return View(new List<Beer>());
            }
        }

        // GET: Beers/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var beer = new BeerCreation();
            return View(beer);
        }

        // POST: Beers/Create
        [HttpPost]
        public async Task<ActionResult> Create(BeerCreation beer)
        {
            try
            {
                var json = JsonConvert.SerializeObject(beer);
                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _client.PostAsync("beers", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var success = JsonConvert.DeserializeObject<bool>(result);

                    if (success)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(beer);
            }
            catch
            {
                return View(beer);
            }
        }

        // GET: Beers/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var jsonAsString = await _client.GetStringAsync($"beers/{id}");
                var beer = JsonConvert.DeserializeObject<Beer>(jsonAsString);
                return View(beer);
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

        // POST: Beers/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string id, BeerCreation beer)
        {
            try
            {
                var json = JsonConvert.SerializeObject(beer);
                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _client.PutAsync($"beers/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var success = JsonConvert.DeserializeObject<bool>(result);

                    if (success)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(beer);
            }
            catch
            {
                return View(beer);
            }
        }

        // GET: Beers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                await _client.DeleteAsync($"beers/{id}");
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
