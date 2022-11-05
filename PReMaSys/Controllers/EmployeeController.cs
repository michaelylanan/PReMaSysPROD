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

        public IActionResult AddToCart(int? id)
        {
            Rewards reward = _context.Rewards.Where(r => r.RewardsInformationId == id).SingleOrDefault();
            return View(reward);

        }

        [HttpPost]
        public IActionResult AddToCart(string qty, int id)
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));

            /*// this retrieves the foreign key id from customer table
            var latestCId = _context.Customers.FirstOrDefault(c => c.ApplicationUser == user).CustomerId;
            Customer cust = _context.Customers.FirstOrDefault(c => c.CustomerId == latestCId);*/

            Rewards reward = _context.Rewards.Where(r => r.RewardsInformationId == id).SingleOrDefault();


            AddToCart cart = new AddToCart();

            cart.ApplicationUser = user;
            cart.Reward = reward;
            cart.RewardImage = reward.Picture;
            cart.RewardName = reward.RewardName;
            cart.Category = reward.Category;
            cart.RewardDescription = reward.Description;
            cart.RewardPrice = reward.PointsCost;
            cart.TotalCost = reward.PointsCost;

            _context.AddToCart.Add(cart);
            _context.SaveChanges();

            return RedirectToAction("DisplayView");
        }

    }
}
