using Microsoft.AspNetCore.Mvc;

namespace TESNS.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
    }
}
