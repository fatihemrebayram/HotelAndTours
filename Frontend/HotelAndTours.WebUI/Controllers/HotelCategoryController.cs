using System.Globalization;
using System.Text;
using HotelAndTours.BusinessLayer.ValiditionRules.BookingValidations;
using HotelAndTours.BusinessLayer.ValiditionRules.HotelCategoryValidations;
using HotelAndTours.DtoLayer.Dtos.HotelCategory;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.HotelCategory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Controllers;

public class HotelCategoryController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HotelCategoryController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index(int editId = 0)
    {
        if (editId != 0)
            ViewBag.EditStatus = "open";

        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelCategory");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<HotelCategory>>(jsData);

            var viewModel = new HotelCategoryCRUDViewModel
            {
                _HotelCategoriesViewModel = values
            };

            if (editId != 0)
            {
                var responseMessageGetById =
                    await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/HotelCategory/{editId}");
                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                    var valueGetById = JsonConvert.DeserializeObject<HotelCategory>(jsDataGetById);
                    viewModel._HotelCategoryAddViewModel = valueGetById;
                }
            }

            return View(viewModel);
        }

        return NoContent();
    }


    [HttpPost]
    public async Task<IActionResult> HotelCategoryAddEdit(HotelCategoryDto p)
    {
        var validator = new HotelCategoryValidator();
        var validationResult = await validator.ValidateAsync(p);

      
        if (validationResult.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
            var jsdata = JsonConvert.SerializeObject(p);
            var content = new StringContent(jsdata, Encoding.UTF8, "application/json");
            if (p.HotelCategoryId != 0)
            {
                var responseMessage =
                    await client.PutAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelCategory", content);
                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "HotelCategory") });
            }
            else
            {
                var responseMessage =
                    await client.PostAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelCategory", content);
                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "HotelCategory") });
            }

        }
        foreach (var item in validationResult.Errors)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                Logger.LogMessage(
                    p.HotelCategoryName + " işlem yapılırken hata oluştu: " + item.PropertyName + " " +
                    item.ErrorMessage, User.Identity.Name, LogLevel.Error, HttpContext);
            }

            return Json(new { success = false, errors });
        }


        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> HotelCategoryDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/HotelCategory/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }
}