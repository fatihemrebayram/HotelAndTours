using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using HotelAndTours.BusinessLayer.ValiditionRules;
using HotelAndTours.BusinessLayer.ValiditionRules.HotelValiditons;
using HotelAndTours.DtoLayer.Dtos.HotelDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Controllers;

public class HotelController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HotelController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> HotelAddEdit(HotelDto p, IFormFile file)
    {
        var validator = new HotelValidator();
        var validationResult = await validator.ValidateAsync(p);

        if (validationResult.IsValid)
        {
            var client = _httpClientFactory.CreateClient();

            var selectedValues = Request.Form["_hotelCategories"]; // Retrieve the selected values as a string array

            var responseMessageHotelCategory = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/HotelCategory/GetHotelCategoriesByIds/{selectedValues}");
            p.HotelCategories = new List<RSHotelCategory>(); // Initialize the HotelCategories collection

            if (responseMessageHotelCategory.IsSuccessStatusCode)
            {
                var jsDataHotelCategory = await responseMessageHotelCategory.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<HotelCategory>>(jsDataHotelCategory);
                foreach (var item in values)
                {
                    var RRSpecs = new RSHotelCategory()
                    {
                        HotelId = p.HotelId,
                        HotelCategoryId = item.HotelCategoryId
                    };
                    p.HotelCategories.Add(RRSpecs);
                }
            }


            if (file != null)
            {
                var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var bytes = stream.ToArray();
                var btArrayContent = new ByteArrayContent(bytes);
                btArrayContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                var multipartFormData = new MultipartFormDataContent();
                multipartFormData.Add(btArrayContent, "file", file.FileName);
                var responseMessageFile =
                    await client.PostAsync(_configuration["AppSettings:ApiEndpoint"] + "api/FileImage",
                        multipartFormData);


                if (responseMessageFile.IsSuccessStatusCode)
                {
                    // Get the generated filename from the response
                    var responseContent = await responseMessageFile.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeAnonymousType(responseContent, new { FileName = "" });
                    var generatedFileName = result.FileName;
                    p.HotelCoverImage = _configuration["AppSettings:ApiEndpoint"] + "api/FileImage/images/" +
                                        generatedFileName;

                    // Use the generatedFileName as needed
                }
            }
          


            var jsData = JsonConvert.SerializeObject(p);
            var content = new StringContent(jsData, Encoding.UTF8, "application/json");
            if (p.HotelId != 0)
            {
                
                var responseMessage =
                    await client.PutAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Hotel", content);
                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Hotel") });
            }
            else
            {
                var responseMessage =
                    await client.PostAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Hotel", content);

                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Hotel") });
            }
        }

        foreach (var item in validationResult.Errors)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                Logger.LogMessage(
                    p.HotelName + " adlı otel için işlem yapılırken hata oluştu: " + item.PropertyName + " " +
                    item.ErrorMessage, User.Identity.Name, LogLevel.Error, HttpContext);
            }

            return Json(new { success = false, errors });
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> HotelDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Hotel/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }

    public async Task<IActionResult> Index(int editId = 0)
    {
        if (editId != 0)
            ViewBag.EditStatus = "open";

        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Hotel");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseMessagehotelCategory =
                await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelCategory");
            var jsDatahotelCategory = await responseMessagehotelCategory.Content.ReadAsStringAsync();
            var hotelCategory = JsonConvert.DeserializeObject<List<HotelCategory>>(jsDatahotelCategory);
            var hotelCategoryDropdownList = hotelCategory.Select(r => new SelectListItem
            {
                Text = r.HotelCategoryName, // Access the Title property of the Room object
                Value = r.HotelCategoryId.ToString() // Access the RoomId property of the Room object
            }).ToList();

            ViewBag.hotelCategoryDropDownList = hotelCategoryDropdownList;

            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Hotel>>(jsData);

            var viewModel = new HotelCRUDViewModel
            {
                _HotelViewModel = values
            };

            if (editId != 0)
            {
                var responseMessageGetById =
                    await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Hotel/{editId}");
                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                    var valueGetById = JsonConvert.DeserializeObject<Hotel>(jsDataGetById);
                    viewModel._HotelAddViewModel = valueGetById;
                }
            }


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

            return View(viewModel);
        }

        return View();
    }
}