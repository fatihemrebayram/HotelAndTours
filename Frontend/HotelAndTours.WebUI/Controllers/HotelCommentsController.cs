using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.HotelComments;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Controllers;

public class HotelCommentsController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HotelCommentsController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelComments");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<HotelComment>>(jsData);

            var viewModel = new HotelCommentCRUDViewModel
            {
                _HotelCommentsViewModel = values
            };


            return View(viewModel);
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> HotelCommentApprove(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/HotelComments/CommentApprove/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> HotelCommentDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/HotelComments/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }
}