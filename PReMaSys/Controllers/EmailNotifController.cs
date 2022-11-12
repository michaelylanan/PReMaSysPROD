using Microsoft.AspNetCore.Mvc;

namespace PReMaSys.Controllers
{
    public class EmailNotifController : Controller
    {

        public IActionResult email()
        {
            return View();
        }
    }
}
