using HotelAndTours.DtoLayer.Dtos.HotelDto;
using HotelAndTours.DtoLayer.Dtos.RoomDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelAndTours.WebUI.Areas.Frontend.Controllers
{
    [Area("Frontend")]
    public class HotelDetails : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HotelDetails(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int hotelId)
        {
            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/Hotel/{hotelId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsData = await responseMessage.Content.ReadAsStringAsync();
                var hotelDto = JsonConvert.DeserializeObject<Hotel>(jsData);

                var responseMessageGetById = await client.GetAsync(_configuration["AppSettings:ApiEndpoint"]+  $"api/Room/GetRoomsByHotelId/{hotelId}");
                if (responseMessageGetById.IsSuccessStatusCode)
                {
                    var jsDataGetById = await responseMessageGetById.Content.ReadAsStringAsync();


                    // Deserialize as an array of RoomDto
                   var rooms = JsonConvert.DeserializeObject<List<Room>>(jsDataGetById);

                    var viewModel = new HotelDetailsViewModel()
                    {
                        HotelDetails = hotelDto,
                        RoomGroups = rooms.GroupBy(r => r.HotelId).ToList()
                    };

                    return View(viewModel);
                }
            }

            return View();
        }
    }
}