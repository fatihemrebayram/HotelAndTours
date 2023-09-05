using Microsoft.AspNetCore.Mvc;

namespace HotelAndTours.WebUI.Areas.Frontend.Controllers
{
    [Area("Frontend")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}