using System.Text;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.RoomNumber;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace RoomNumberAndTours.WebUI.Controllers;

public class RoomNumberController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public RoomNumberController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index(int editId = 0)
    {
        if (editId != 0)
            ViewBag.EditStatus = "open";

        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/RoomNumber");

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<RoomNumbers>>(jsData);
            var responseMessagerooms = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Room");
            var jsDatarooms = await responseMessagerooms.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<List<Room>>(jsDatarooms);
            var roomDropdownList = rooms.Select(r => new SelectListItem
            {
                Text = r.Title + " (" + r.Hotel.HotelName + ") ", // Access the Title property of the Room object
                Value = r.RoomId.ToString() // Access the RoomId property of the Room object,
            }).ToList();

            ViewBag.DropDownRooms = roomDropdownList;


            var viewModel = new RoomNumberCRUDViewModel
            {
                _RoomNumberViewModel = values
            };

            if (editId != 0)
            {
                var responseMessageGetById =
                    await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/RoomNumber/{editId}");

                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                    var valueGetById = JsonConvert.DeserializeObject<RoomNumbers>(jsDataGetById);
                    viewModel._RoomNumberAddViewModel = valueGetById;
                }
            }

            return View(viewModel);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RoomNumberAddEdit(RoomNumbers p)
    {
        var client = _httpClientFactory.CreateClient();
        var jsdata = JsonConvert.SerializeObject(p);
        var content = new StringContent(jsdata, Encoding.UTF8, "application/json");
        if (p.RoomNumberId != 0)
        {
            var responseMessage =
                await client.PutAsync(_configuration["AppSettings:ApiEndpoint"] + "api/RoomNumber", content);
            if (responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
        }
        else
        {
            var responseMessage =
                await client.PostAsync(_configuration["AppSettings:ApiEndpoint"] + "api/RoomNumber", content);
            if (responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> RoomNumberDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage =
            await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/RoomNumber/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }
}