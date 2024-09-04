using Microsoft.AspNetCore.Mvc;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
