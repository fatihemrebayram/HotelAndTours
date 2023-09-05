using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Areas.Frontend.Controllers;

[Area("Frontend")]
public class HotelsGridSidebarController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HotelsGridSidebarController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index(string search="Empty",string location="Empty",string category="Empty")
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Hotel/GetHotelsFiltered/{search}/{location}/{category}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseMessageHotelCateogry = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelCategory");
            if (responseMessageHotelCateogry.IsSuccessStatusCode)
            {
                var jsDataHotelCateogry = await responseMessageHotelCateogry.Content.ReadAsStringAsync();
                var valuesHotelCateogry = JsonConvert.DeserializeObject<List<HotelCategory>>(jsDataHotelCateogry);
                ViewBag.HotelCategory=valuesHotelCateogry;
            }

            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Hotel>>(jsData);

            var username = "fatihemrebayram"; // Replace with your GeoNames username

            // Make a request to the GeoNames API to retrieve all cities in Turkey
            var apiUrl = $"http://api.geonames.org/searchJSON?country=TR&maxRows=1000&username={username}";

            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Log or print the content to inspect the JSON response

            // Parse the JSON response
            dynamic result = JsonConvert.DeserializeObject(content);
            var capitalCities = new List<string>();

            foreach (var item in result.geonames)
            {
                string featureCode = item.fcode;
                if (featureCode == "PPLA")
                {
                    string capitalCity = item.name;
                    capitalCities.Add(capitalCity);
                }
            }


            ViewBag.CapitalCities = capitalCities;
            ViewBag.City = location;
            ViewBag.Category= category;

            return View(values);
        }

        return View();
    }
}