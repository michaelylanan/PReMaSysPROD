using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Testertest.Data;
using Testertest.Models;
using Testertest.ViewModel;

namespace Testertest.Controllers
{
    //[Authorize(Roles = "Support")]
    public class SupportAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupportAdminController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult SupportPage()
        {
            return View();
        }
        public IActionResult NotificationPage()
        {
            return View();
        }

        /*New Methods*/
        public IActionResult EmployeeRole()
        {
            var roles = _roleManager.Roles;

            return View(roles);
        }

        [HttpGet]
        public IActionResult ListSalesEmployee()
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var list = _context.Users.Where(c => c.user == user).ToList();
            return View(list);
        }

        /*EDIT ADMIN ROLES Account------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> EditSalesLC(string id)
        {
            var se = await _userManager.FindByIdAsync(id);

            if (se == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(se);
            var userRoles = await _userManager.GetRolesAsync(se);

            var model = new EditSalesUserViewModel
            {
                Id = se.Id,
                Email = se.Email,
                UserName = se.UserName,
                /*Claims = User.Claims.Select(c => c.Value).ToList(),*/
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSalesLC(EditSalesUserViewModel model)
        {
            var se = await _userManager.FindByIdAsync(model.Id);

            if (se == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                se.UserName = model.Email;
                se.Email = model.Email;
                var result = await _userManager.UpdateAsync(se);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListSalesEmployee");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //DELETE ADMIN ACCOUNT
        public async Task<IActionResult> DeleteSLC(string id)
        {
            var se = await _userManager.FindByIdAsync(id);
            if (User == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(se);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListSalesEmployee");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListAdminRoles");
            }
        }



        /*CREATE NEW ADMIN ROLE*/
        public IActionResult SERecord()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SERecord(SalesUser se)
        {
            ApplicationUser userz = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));

            var user = new ApplicationUser
            {
                UserName = se.Email,
                Email = se.Email,
                EmailConfirmed = true,
                user = userz
            };
            var insertrec = await _userManager.CreateAsync(user, se.Password);
            if (insertrec.Succeeded)
            {
                ViewBag.message = "The User " + se.Email + "Is Saved Succesfully..!!";
            }
            else
            {
                foreach (var error in insertrec.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }


        /*OLD METHODS*/
        /*SEMPLOYEES CRUD--------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public IActionResult IndexSE()
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var list = _context.SalesEmployeeRecords.Where(c => c.ApplicationUser == user).ToList();
            return View(list);
        }

        public IActionResult CreateSE()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSE(SalesEmployeeRecord record, IFormFile EmployeePicture)
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));

            var SEmployees = new SalesEmployeeRecord()
            {
                ApplicationUser = user,
                EmployeeNo = record.EmployeeNo,
                EmployeeUsername = record.EmployeeUsername,
                EmployeeFirstname = record.EmployeeFirstname,
                EmployeeLastname = record.EmployeeLastname,
                Gender = record.Gender,
                EmployeeAddress = record.EmployeeAddress,
                EmployeeBirthdate = record.EmployeeBirthdate,
                EmployeeEmail = record.EmployeeEmail,
                ContactNo = record.ContactNo,
                Password = record.Password,
                EmployeePoints = record.EmployeePoints,
                DateAdded = DateTime.Now,
            };
            if (EmployeePicture != null)
            {
                if (EmployeePicture.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/SEmployees", EmployeePicture.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        EmployeePicture.CopyTo(stream);
                    }
                    SEmployees.EmployeePicture = "~/img/SEmployees/" + EmployeePicture.FileName;
                }
            }

            _context.SalesEmployeeRecords.Add(SEmployees);
            _context.SaveChanges();

            return RedirectToAction("IndexSE");

        }

        public IActionResult EditSE(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("IndexSE");
            }

            //variable product that retrieves the existing record from the Rewards table.
            var SEmployees = _context.SalesEmployeeRecords.Where(r => r.SEmployeeRecordsID == id).SingleOrDefault();

            //if the reward record is not present the view will redirect to the Index action.
            if (SEmployees == null)
            {
                return RedirectToAction("IndexSE");
            }

            //Rewards model object will be included to be rendered by the View method
            return View(SEmployees);
        }

        [HttpPost]
        public IActionResult EditSE(int? id, SalesEmployeeRecord record)
        {
            var SEmployees = _context.SalesEmployeeRecords.Where(s => s.SEmployeeRecordsID == id).SingleOrDefault();
            SEmployees.EmployeeNo = record.EmployeeNo;
            SEmployees.EmployeeUsername = record.EmployeeUsername;
            SEmployees.EmployeeFirstname = record.EmployeeFirstname;
            SEmployees.EmployeeLastname = record.EmployeeLastname;
            SEmployees.Gender = record.Gender;
            SEmployees.EmployeeAddress = record.EmployeeAddress;
            SEmployees.EmployeeBirthdate = record.EmployeeBirthdate;
            SEmployees.EmployeeEmail = record.EmployeeEmail;
            SEmployees.ContactNo = record.ContactNo;
            SEmployees.Password = record.Password;
            SEmployees.EmployeePoints = record.EmployeePoints;
            SEmployees.DateModified = DateTime.Now;

            _context.SalesEmployeeRecords.Update(SEmployees);
            _context.SaveChanges();

            return RedirectToAction("IndexSE");
        }

        public IActionResult DeleteSE(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("IndexSE");
            }

            var SEmployees = _context.SalesEmployeeRecords.Where(s => s.SEmployeeRecordsID == id).SingleOrDefault();

            if (SEmployees == null)
            {
                return RedirectToAction("IndexSE");
            }

            _context.SalesEmployeeRecords.Remove(SEmployees);
            _context.SaveChanges();

            return RedirectToAction("IndexSE");
        }

        /*REWARDS CRUD--------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public IActionResult IndexR()
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var list = _context.Rewards.Where(c => c.ApplicationUser == user).ToList();
            return View(list);
        }

        //(1) Create Reward Information
        public IActionResult CreateR()
        {
            return View();
        }

        //Overriding exising Create method through Replica
        [HttpPost]
        public IActionResult CreateR(Rewards record, IFormFile Picture)
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var rewards = new Rewards()
            {
                ApplicationUser = user,
                RewardName = record.RewardName,
                Description = record.Description,
                RewardCost = record.RewardCost,
                PointsCost = record.PointsCost,
                DateAdded = DateTime.Now,
                Category = record.Category,
                Status = record.Status,
            };

            if (Picture != null)
            {
                if (Picture.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/rewards", Picture.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Picture.CopyTo(stream);
                    }
                    rewards.Picture = "~/img/rewards/" + Picture.FileName;
                }
            }

            //Add existing value 
            _context.Rewards.Add(rewards);
            _context.SaveChanges();

            return RedirectToAction("IndexR");
        }

        //(2) Edit Reward Informaiton
        public IActionResult EditR(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("IndexR");
            }

            //variable product that retrieves the existing record from the Rewards table.
            var rewards = _context.Rewards.Where(r => r.RewardsInformationId == id).SingleOrDefault();

            //if the reward record is not present the view will redirect to the Index action.
            if (rewards == null)
            {
                return RedirectToAction("IndexR");
            }

            //Rewards model object will be included to be rendered by the View method
            return View(rewards);
        }
        //2.2 Override Existing Edit Method by a Replica using a nullable integer id and reward object as the parameters.
        [HttpPost]
        public IActionResult EditR(int? id, Rewards record)
        {
            var rewards = _context.Rewards.Where(r => r.RewardsInformationId == id).SingleOrDefault();
            rewards.Picture = record.Picture;
            rewards.RewardName = record.RewardName;
            rewards.Description = record.Description;
            rewards.RewardCost = record.RewardCost;
            rewards.PointsCost = record.PointsCost;
            rewards.DateModified = DateTime.Now;
            rewards.Category = record.Category;
            rewards.Status = record.Status;

            _context.Rewards.Update(rewards);
            _context.SaveChanges();

            return RedirectToAction("IndexR");
        }

        public IActionResult DeleteR(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("IndexR");
            }

            var rewards = _context.Rewards.Where(r => r.RewardsInformationId == id).SingleOrDefault();
            if (rewards == null)
            {
                return RedirectToAction("IndexR");
            }
            _context.Rewards.Remove(rewards);
            _context.SaveChanges();

            return RedirectToAction("IndexR");
        }
    }
}