using Microsoft.AspNetCore.Mvc;

namespace SilverSpy.Controllers
{
    public class TransactionController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}