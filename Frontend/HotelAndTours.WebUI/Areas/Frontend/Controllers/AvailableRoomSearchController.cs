using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelAndTours.WebUI.Areas.Frontend.Controllers;

[Area("Frontend")]
public class AvailableRoomSearchController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AvailableRoomSearchController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }


    public async Task<IActionResult> Index(int qtyInputAdult, int qtyInputChild,int hotelId, string dates, int hotelSearchId = 0, int roomId = 0)
    {
        if (hotelSearchId == 0)
            hotelSearchId = hotelId;
        var client = _httpClientFactory.CreateClient();
        var datesArray = dates.Split('>');
        var dateFormat = "dd-MM-yy";
        var _checkInConv = DateTime.ParseExact(datesArray[0].Trim(), dateFormat, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
        var _checkOutConv = DateTime.ParseExact(datesArray[1].Trim(), dateFormat, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
        var responseMessage =
            await client.GetAsync(
              _configuration["AppSettings:ApiEndpoint"] + $"api/Room/hotels/{hotelId}/rooms/{roomId}/available?checkIn={_checkInConv}&checkOut={_checkOutConv}");

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<RoomNumbers>>(jsData);

            var responseMessageHotels = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"]+"api/Hotel");
            var jsDataHotels = await responseMessageHotels.Content.ReadAsStringAsync();
            var Hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsDataHotels);
            List<SelectListItem> HotelsDropdownList = Hotels.Select(r => new SelectListItem
            {
                Text = r.HotelName,  // Access the Title property of the Room object
                Value = r.HotelId.ToString()  // Access the RoomId property of the Room object
            }).ToList();
            ViewBag.DropDownList = HotelsDropdownList;
            var responseMessagerooms = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Room/GetRoomsByHotelId/{hotelSearchId}");
            var jsDatarooms = await responseMessagerooms.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<List<Room>>(jsDatarooms);
            List<SelectListItem> roomDropdownList = rooms.Select(r => new SelectListItem
            {
                Text = r.Title,  // Access the Title property of the Room object
                Value = r.RoomId.ToString()  // Access the RoomId property of the Room object
            }).ToList();


            DateTime checkInDate = DateTime.ParseExact(_checkInConv, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime checkOutDate = DateTime.ParseExact(_checkOutConv, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            TimeSpan duration = checkOutDate - checkInDate;
            int numberOfDays = (int)duration.TotalDays;

          

            ViewBag.RoomsDropDownList = roomDropdownList;
            ViewBag.HotelSearchId = hotelSearchId;
            ViewBag.RoomId = roomId;
            ViewBag.Dates = dates;
            ViewBag.Days = numberOfDays;
            ViewBag.qtyInputAdult = qtyInputAdult;
            ViewBag.qtyInputChild = qtyInputChild;
            return View(values);
        }

        return View();
    }
}