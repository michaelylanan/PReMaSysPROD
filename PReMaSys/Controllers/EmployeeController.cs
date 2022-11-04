using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PReMaSys.Data;

using PReMaSys.Models;


namespace PReMaSys.Controllers
{
    /*[Authorize(Roles = "Employee")]*/
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult EmployeeRank()
        {
            return View();
        }

        public IActionResult TransactionHistory()
        {
            return View();
        }
        public IActionResult RewardStatus()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult EmployeeHomePage(String searchby, String search)
        {
            var list = _context.Rewards.ToList();
            if (searchby == "RewardName" && search != null)
            {
                return View(list.Where(x => x.RewardName.Contains(search)).ToList());
            }
            else
            {
                return View(list);
            }
        }

    }
}
