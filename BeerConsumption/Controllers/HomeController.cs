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
    public class HomeController : Controller
    {
        private static readonly HttpClient _client;
        static HomeController()
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
    }
}