using System.Globalization;
using System.Text;
using HotelAndTours.BusinessLayer.ValiditionRules.BookingValidations;
using HotelAndTours.DtoLayer.Dtos.BookingDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.Purchase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Areas.Frontend.Controllers;

[Area("Frontend")]
public class PurchaseController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public PurchaseController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string dates, int qtyInputAdult, int qtyInputChild, int roomNumberId = 0)
    {
        var datesArray = dates.Split('>');
        var dateFormat = "dd-MM-yy";
        var _checkInConv = DateTime.ParseExact(datesArray[0].Trim(), dateFormat, CultureInfo.InvariantCulture)
            .ToString("dd/MM/yyyy");
        var _checkOutConv = DateTime.ParseExact(datesArray[1].Trim(), dateFormat, CultureInfo.InvariantCulture)
            .ToString("dd/MM/yyyy");


        var username = "fatihemrebayram"; // Replace with your GeoNames username

        // Make a request to the GeoNames API to retrieve all cities in Turkey
        var apiUrl = $"http://api.geonames.org/searchJSON?country=TR&maxRows=1000&username={username}";

        using var client = new HttpClient();
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

        var responseMessageGetById =
            await client.GetAsync(_configuration["AppSettings:ApiEndpoint"] + $"api/RoomNumber/{roomNumberId}");

        if (responseMessageGetById.IsSuccessStatusCode)
        {
            var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();
            var valueGetById = JsonConvert.DeserializeObject<RoomNumbers>(jsDataGetById);

            var checkInDate = DateTime.ParseExact(_checkInConv, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var checkOutDate = DateTime.ParseExact(_checkOutConv, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            var duration = checkOutDate - checkInDate;
            var numberOfDays = (int)duration.TotalDays;


            var bookingDetails = new BookingDetailsViewModel
            {
                CheckIn = _checkInConv,
                CheckOut = _checkOutConv,
                Adults = qtyInputAdult,
                Childs = qtyInputChild,
                RoomNumber = valueGetById,
                Price = (valueGetById.Room.PriceForNightAdult * qtyInputAdult +
                         valueGetById.Room.PriceForNightChildren * qtyInputChild) * numberOfDays
            };
            ViewBag.CapitalCities = capitalCities;

            return View(bookingDetails);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Index(BookingDetailsViewModel b, string dates,int roomNumberId, int qtyInputAdult,int qtyInputChild, string street_1,string city,string district,string postal_code, int hotelId = 0, int roomId = 0)
    {


        var datesArray = dates.Split('>');
        var dateFormat = "dd-MM-yy";
        var _checkInConv = DateTime.ParseExact(datesArray[0].Trim(), dateFormat, CultureInfo.InvariantCulture)
            .ToString("dd/MM/yyyy");
        var _checkOutConv = DateTime.ParseExact(datesArray[1].Trim(), dateFormat, CultureInfo.InvariantCulture)
            .ToString("dd/MM/yyyy");


        var checkInDate = DateTime.ParseExact(_checkInConv, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        var checkOutDate = DateTime.ParseExact(_checkOutConv, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        var billingAdress = city + " " + district + " " + postal_code + " " + street_1;
        b.BillingAddress = billingAdress;
        var p = new BookingDto()
        {
            CheckIn = checkInDate,
            CheckOut = checkOutDate,
            ChildCount = qtyInputChild,
            AdultCount = qtyInputAdult,
            Mail = b.Mail,
            Name = b.Name,
            PaidPrice = b.Price,
            RoomNumberId = roomNumberId,
            BillingAddress = b.BillingAddress,
            Status = "Onay Bekleniyor",
            PhoneNumber = b.PhoneNumber,
            AddedDate = DateTime.Now,
        };
        var validator = new BookingValidator();
        var validationResult = await validator.ValidateAsync(p);

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
    /*	public async Task<IActionResult> GetDistricts(string city)
		{
			string username = "fatihemrebayram"; // Replace with your GeoNames username

			// Make a request to the GeoNames API to retrieve the districts of the selected city
			string apiUrl = $"http://api.geonames.org/searchJSON?country=TR&maxRows=1000&username={username}&name={Uri.EscapeDataString(city)}&featureCode=ADM2";

			using HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync(apiUrl);
			response.EnsureSuccessStatusCode();

			string content = await response.Content.ReadAsStringAsync();

			// Parse the JSON response for districts
			dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
			List<string> districts = new List<string>();

			foreach (var item in result.geonames)
			{
				string districtName = item.name;
				districts.Add(districtName);
			}

			// Return the districts as a JSON response
			return Json(districts);
		}
	*/
}