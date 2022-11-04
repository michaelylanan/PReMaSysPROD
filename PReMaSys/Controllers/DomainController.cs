using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Testertest.Data;
using Testertest.Models;
using Testertest.ViewModel;

namespace RecognitionSystemFinal.Controllers
{
    //[Authorize(Roles = "Domain")]
    public class DomainController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DomainController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult DomainPage()
        {
            return View();
        }

        /*APPROVAL OF REWARDS-------------------------------------------------------------------------------------------------------------------------------------*/
        public IActionResult ApproveRewards()
        {
            /*  ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));
              var latestCId = _context.Rewards.FirstOrDefault(c => c.ApplicationUser == user).CustomerId;
              Customer cust = _context.Customers.FirstOrDefault(c => c.CustomerId == latestCId);

              var list = _context.AddToCarts.Where(c => c.Customer == cust).ToList();*/
            var list = _context.Rewards.ToList();
            return View(list);
        }

        public IActionResult ApproveR(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ApproveRewards");
            }

            var rewards = _context.Rewards.Where(r => r.RewardsInformationId == id).SingleOrDefault();

            if (rewards == null)
            {
                return RedirectToAction("ApproveRewards");
            }

            return View(rewards);
        }

        [HttpPost]
        public IActionResult ApproveR(int? id, Rewards record)
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

            return RedirectToAction("ApproveRewards");
        }

        /* ------------------------------------------------------------------------------------------------------------------------------------------------*/
        public IActionResult AdminAllRoles()
        {
            var roles = _roleManager.Roles;

            return View(roles);
        }

        /*ADMIN ROLES CRUD---------------------------------------------------------------------------------------------------------------------------------*/

        [HttpGet]
        public IActionResult ListAdminRoles()
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var list = _context.Users.Where(c => c.user == user).ToList();
            return View(list);
        }



        /*CREATE NEW ADMIN ROLE*/
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(AdminUser admin)
        {
            ApplicationUser userz = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));

            var user = new ApplicationUser
            {
                UserName = admin.Email,
                Email = admin.Email,
                EmailConfirmed = true,
                user = userz
            };
            var insertrec = await _userManager.CreateAsync(user, admin.Password);
            if (insertrec.Succeeded)
            {
                ViewBag.message = "The User " + admin.Email + "Is Saved Succesfully..!!";
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

        /*EDIT ADMIN ROLES Account------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> EditAdminRole(string id)
        {
            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(admin);
            var userRoles = await _userManager.GetRolesAsync(admin);

            var model = new EditAdminUserViewModel
            {
                Id = admin.Id,
                Email = admin.Email,
                UserName = admin.UserName,
                /*Claims = User.Claims.Select(c => c.Value).ToList(),*/
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdminRole(EditAdminUserViewModel model)
        {
            var admin = await _userManager.FindByIdAsync(model.Id);

            if (admin == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                admin.UserName = model.Email;
                admin.Email = model.Email;
                var result = await _userManager.UpdateAsync(admin);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListAdminRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //DELETE ADMIN ACCOUNT
        public async Task<IActionResult> DeleteAdminAccount(string id)
        {
            var admin = await _userManager.FindByIdAsync(id);
            if (User == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(admin);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListAdminRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListAdminRoles");
            }
        }

        /*CREATE NEW ADMIN ROLE*/

        /* public IActionResult CreateRole()
         {
             return View();
         }

         [HttpPost]
         public IActionResult CreateRole( AdminRoles record, IFormFile AdminPicture)
         {
             ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));

             var adminRole = new AdminRoles()
             {
                 ApplicationUser = user,
                 Role = record.Role,
                 AdminEmployeeNo = record.AdminEmployeeNo,
                 AdminFirstname = record.AdminFirstname,
                 AdminLastname = record.AdminLastname,
                 AdminEmail = record.AdminEmail,
                 ContactNo = record.ContactNo,
                 Password = record.Password,
                 DateAdded = DateTime.Now,
             };
             if (AdminPicture != null)
             {
                 if (AdminPicture.Length > 0)
                 {
                     var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/adminrole", AdminPicture.FileName);
                     using (var stream = new FileStream(filePath, FileMode.Create))
                     {
                         AdminPicture.CopyTo(stream);
                     }
                     adminRole.AdminPicture = "~/img/adminrole/" + AdminPicture.FileName;
                 }
             }

             _context.AdminRoles.Add(adminRole);
             _context.SaveChanges();

             return RedirectToAction("AdminRoles");

         }*/
        public IActionResult EditRole(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AdminRoles");
            }

            var adminRoles = _context.AdminRoles.Where(r => r.AdminRoleId == id).SingleOrDefault();

            if (adminRoles == null)
            {
                return RedirectToAction("AdminRoles");
            }
            return View(adminRoles);

        }

        [HttpPost]
        public IActionResult EditRole(int? id, AdminRoles record)
        {
            var adminRoles = _context.AdminRoles.Where(s => s.AdminRoleId == id).SingleOrDefault();
            adminRoles.Role = record.Role;
            adminRoles.AdminEmployeeNo = record.AdminEmployeeNo;
            adminRoles.AdminFirstname = record.AdminFirstname;
            adminRoles.AdminLastname = record.AdminLastname;
            adminRoles.AdminEmail = record.AdminEmail;
            adminRoles.ContactNo = record.ContactNo;
            adminRoles.Password = record.Password;
            adminRoles.DateModified = DateTime.Now;

            _context.AdminRoles.Update(adminRoles);
            _context.SaveChanges();

            return RedirectToAction("AdminRoles");

        }

        public IActionResult DeleteRole(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AdminRoles");
            }

            var adminRoles = _context.AdminRoles.Where(s => s.AdminRoleId == id).SingleOrDefault();

            if (adminRoles == null)
            {
                return RedirectToAction("AdminRoles");
            }

            _context.AdminRoles.Remove(adminRoles);
            _context.SaveChanges();

            return RedirectToAction("AdminRoles");
        }

        /*SALES-POINTS PROFIT CRUD---------------------------------------------------------------------------------------------------------------------------------*/

        public IActionResult SEPoints()
        {
            var list = _context.SalesEmployeeRecords.ToList();
            return View(list);
        }

    }
}
