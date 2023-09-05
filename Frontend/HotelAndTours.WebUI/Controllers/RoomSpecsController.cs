using System.Text;
using HotelAndTours.DtoLayer.Dtos.RoomDto;
using HotelAndTours.DtoLayer.Dtos.RoomSpecsDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.Room;
using HotelAndTours.ModelsLayer.Models.RoomSpecs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Controllers;

public class RoomSpecsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public RoomSpecsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }


    public async Task<IActionResult> Index(int editId = 0)
    {
        if (editId != 0)
            ViewBag.EditStatus = "open";

        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/RoomSpecs");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<RoomSpecs>>(jsData);

            var viewModel = new RoomSpecsCRUDViewModel
            {
                _RoomSpecsViewModel = values
            };

            if (editId != 0)
            {
                var responseMessageGetById = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/RoomSpecs/{editId}");
                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                    var valueGetById = JsonConvert.DeserializeObject<RoomSpecs>(jsDataGetById);
                    viewModel._RoomSpecsAddViewModel = valueGetById;
                }
            }

            return View(viewModel);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(RoomSpecsDto p)
    {
        var client = _httpClientFactory.CreateClient();
        var jsdata = JsonConvert.SerializeObject(p);
        var content = new StringContent(jsdata, Encoding.UTF8, "application/json");
        if (p.Id != 0)
        {
            var responseMessage = await client.PutAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/RoomSpecs", content);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
        }
        else
        {
            var responseMessage = await client.PostAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/RoomSpecs", content);

            if (responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> RoomSpecsDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/RoomSpecs/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }
}