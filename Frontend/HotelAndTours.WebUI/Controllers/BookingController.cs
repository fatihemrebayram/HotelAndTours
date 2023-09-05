using System.Globalization;
using System.Text;
using HotelAndTours.BusinessLayer.ValiditionRules.BookingValidations;
using HotelAndTours.DtoLayer.Dtos.BookingDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Controllers;

public class BookingController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public BookingController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> BookingAddEdit(BookingDto p, string checkIn, string checkOut, int hotelId = 0,
        int roomId = 0)
    {
        var validator = new BookingValidator();
        var validationResult = await validator.ValidateAsync(p);

        var dateFormat = "yyyy-MM-dd";
        var _checkInConv = DateTime.ParseExact(checkIn, dateFormat, CultureInfo.InvariantCulture)
            .ToString("dd/MM/yyyy");
        var _checkOutConv = DateTime.ParseExact(checkOut, dateFormat, CultureInfo.InvariantCulture)
            .ToString("dd/MM/yyyy");

        if (validationResult.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessageGetById =
                await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/RoomNumber/{p.RoomNumberId}");

            if (responseMessageGetById.IsSuccessStatusCode)
            {
                var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                var valueGetById = JsonConvert.DeserializeObject<RoomNumbers>(jsDataGetById);
                roomId = valueGetById.Room.RoomId;
                hotelId = valueGetById.Room.HotelId;
            }

            var responseMessageAvilableRooms =
                await client.GetAsync(
                    _configuration["AppSettings:ApiEndpoint"] +
                    $"api/Room/hotels/{hotelId}/rooms/{roomId}/available?checkIn={_checkInConv}&checkOut={_checkOutConv}");

            if (responseMessageAvilableRooms.IsSuccessStatusCode)
            {
                var jsData = await responseMessageAvilableRooms.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<RoomNumbers>>(jsData);

                // Exclude the current booking's RoomNumberId from the check
                var availableRoomIds = values.Select(x => x.RoomNumberId);

                if (!availableRoomIds.Contains(p.RoomNumberId))
                {
                    ModelState.AddModelError("Oda Dolu", "Seçilen tarihler arası oda dolu");
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Where(e => !string.IsNullOrEmpty(e.ErrorMessage) &&
                                    e.ErrorMessage != "The value '' is invalid.")
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var errorResponse = errors.Select(errorMessage => new { errorMessage });

                    return Json(new { success = false, errors = errorResponse });
                }
            }
            else
            {
                ModelState.AddModelError("Oda Dolu", "Seçilen tarihler arası oda dolu");
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) // Extract error messages
                    .ToList();

                var errorResponse = errors.Select(errorMessage => new { errorMessage });

                return Json(new { success = false, errors = errorResponse });
            }

            var jsdata = JsonConvert.SerializeObject(p);
            var content = new StringContent(jsdata, Encoding.UTF8, "application/json");
            if (p.BookingId != 0)
            {
                var responseMessage =
                    await client.PutAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Booking", content);
                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Booking") });
            }
            else
            {
                var responseMessage =
                    await client.PostAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Booking", content);

                if (responseMessage.IsSuccessStatusCode)
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Booking") });
            }
        }

        foreach (var item in validationResult.Errors)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                Logger.LogMessage(
                    p.Name + " adlı kişi için rezervasyon yapılırken hata oluştu: " + item.PropertyName + " " +
                    item.ErrorMessage, User.Identity.Name, LogLevel.Error, HttpContext);
            }

            return Json(new { success = false, errors });
        }


        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> BookingDelete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.DeleteAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Booking/{id}");
        if (responseMessage.IsSuccessStatusCode)
            return RedirectToAction("Index");
        return NotFound();
    }

    public async Task<IActionResult> Index(int editId = 0)
    {
        if (editId != 0)
            ViewBag.EditStatus = "open";

        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/Booking");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Booking>>(jsData);

            var responseMessagerooms =
                await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + "api/RoomNumber");
            var jsDatarooms = await responseMessagerooms.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<List<RoomNumbers>>(jsDatarooms);

            // Retrieve the list of already used room number names from bookings
            /*    var usedRoomNumbers = values.Select(b => b.RoomNumber.RoomName).ToList();

                // Filter the rooms to exclude the used room numbers
                var availableRooms = rooms.Where(r => !usedRoomNumbers.Contains(r.RoomName)).ToList();*/

            var roomDropdownList = rooms.Select(r => new SelectListItem
            {
                Text = r.RoomName + " (" + r.Room.Title + " - " + r.Room.Hotel.HotelName + ")",
                Value = r.RoomNumberId.ToString()
            }).ToList();

            ViewBag.DropDownList = roomDropdownList;
            var viewModel = new BookingCRUDViewModel
            {
                _BookingViewModel = values
            };

            if (editId != 0)
            {
                var responseMessageGetById =
                    await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/Booking/{editId}");
                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
                    var valueGetById = JsonConvert.DeserializeObject<Booking>(jsDataGetById);
                    viewModel._BookingAddViewModel = valueGetById;
                }
            }

            return View(viewModel);
        }

        return View();
    }
}