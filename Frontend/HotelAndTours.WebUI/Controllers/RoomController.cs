using System.Net.Http.Headers;
using System.Text;
using HotelAndTours.BusinessLayer.ValiditionRules.RoomValidations;
using HotelAndTours.DtoLayer.Dtos.RoomDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.Room;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Common;

namespace HotelAndTours.WebUI.Controllers;

public class RoomController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public RoomController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index(int editId = 0)
    {


        if (editId != 0)
            ViewBag.EditStatus = "open";
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Authorization"]);

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Room");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Room>>(jsData);

            var responseMessageHotels = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Hotel");
            var jsDataHotels = await responseMessageHotels.Content.ReadAsStringAsync();
            var Hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsDataHotels);
            var HotelsDropdownList = Hotels.Select(r => new SelectListItem
            {
                Text = r.HotelName, // Access the Title property of the Room object
                Value = r.HotelId.ToString() // Access the RoomId property of the Room object
            }).ToList();

            ViewBag.DropDownList = HotelsDropdownList;


            var responseMessageroomSpecs =
                await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/RoomSpecs");
            var jsDataroomSpecs = await responseMessageroomSpecs.Content.ReadAsStringAsync();
            var roomSpecs = JsonConvert.DeserializeObject<List<RoomSpecs>>(jsDataroomSpecs);
            var roomSpecsDropdownList = roomSpecs.Select(r => new SelectListItem
            {
                Text = r.RoomSpec, // Access the Title property of the Room object
                Value = r.Id.ToString() // Access the RoomId property of the Room object
            }).ToList();

            ViewBag.roomSpecsDropDownList = roomSpecsDropdownList;


            var viewModel = new RoomCRUDViewModel
            {
                _RoomViewModel = values
            };

            if (editId != 0)
            {
                var responseMessageGetById =
                    await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Room/{editId}");
                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                    var valueGetById = JsonConvert.DeserializeObject<Room>(jsDataGetById);
                    viewModel._RoomAddViewModel = valueGetById;
                }
            }

            return View(viewModel);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RoomAddEdit(RoomDto p,IFormFile file)
    {
        var validator = new RoomValidator();
        var validationResult = await validator.ValidateAsync(p);

        if (validationResult.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
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
                    p.RoomCoverImage= _configuration["AppSettings:ApiEndpoint"] + "api/FileImage/images/" +
                                        generatedFileName;

                    // Use the generatedFileName as needed
                }
            }
            var jsdata = JsonConvert.SerializeObject(p);
            var content = new StringContent(jsdata, Encoding.UTF8, "application/json");
            if (p.RoomId != 0)
            {
                var responseMessage =
                    await client.PutAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Room", content);
                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Room") });
            }
            else
            {
                var responseMessage =
                    await client.PostAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Room", content);

                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Room") });
            }
        }

        foreach (var item in validationResult.Errors)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                Logger.LogMessage(
                    p.Title + " adlı otel için işlem yapılırken hata oluştu: " + item.PropertyName + " " +
                    item.ErrorMessage, User.Identity.Name, LogLevel.Error, HttpContext);
            }

            return Json(new { success = false, errors });
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> RoomDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Room/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }
}