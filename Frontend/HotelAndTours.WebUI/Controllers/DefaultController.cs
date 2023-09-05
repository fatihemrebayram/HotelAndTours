using EntityLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.HotelComments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HotelAndTours.WebUI.Controllers;

public class DefaultController : Controller
{
    private readonly UserManager<AppUser> _appUser;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public DefaultController(UserManager<AppUser> appUser, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _appUser = appUser;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult AccessDenied()
    {
        return PartialView();
    }

    public async Task<PartialViewResult> AdminHeader()
    {
        var values = await _appUser.FindByNameAsync(User.Identity.Name);
        return PartialView(values);
    }

    public async Task<PartialViewResult> AdminSidebar()
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/HotelComments");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<HotelComment>>(jsData);

    
            ViewBag.CommentCount = values.Count(x => x.Status == false);
        }

        ViewBag.CommentCount = 0;

        return PartialView();
    }
}