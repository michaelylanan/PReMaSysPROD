using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PReMaSys.Data;
using PReMaSys.Models;

namespace PReMaSys.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public IActionResult Index()
        {
            return View();
        }

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }
        public IActionResult LoginSE()
        {
            return View();
        }

        public IActionResult LoginSA()
        {
            return View();
        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Register(DomainAccount record, IFormFile CompanyLogo, IFormFile AdminPicture)
        {
            var domain = new DomainAccount()
            {
                CompanyName = record.CompanyName,
                CompanyAddress = record.CompanyAddress,
                BusinessSize = record.BusinessSize,
                NatureOfBusiness = record.NatureOfBusiness,
                CompanyBday = record.CompanyBday,
                AdminLastname = record.AdminLastname,
                AdminFirstname = record.AdminFirstname,
                ContactNo = record.ContactNo,
                AdminEmail = record.AdminEmail,
                password = record.password,
                DateAdded = DateTime.Now,

            };
            if (CompanyLogo != null)
            {
                if (CompanyLogo.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/companylogo", CompanyLogo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        CompanyLogo.CopyTo(stream);
                    }
                    domain.CompanyLogo = "~/img/companylogo/" + CompanyLogo.FileName;
                }
            };
            if (AdminPicture != null)
            {
                if (AdminPicture.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/domainaccount", AdminPicture.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AdminPicture.CopyTo(stream);
                    }
                    domain.AdminPicture = "~/img/domainaccount/" + AdminPicture.FileName;
                }
            }
            _context.DomainAccount.Add(domain);
            _context.SaveChanges();

            ViewBag.Message = record.AdminFirstname + "" + record.AdminLastname + " successfully registered";


            return RedirectToAction("Register");
        }



        /* if (ModelState.IsValid)
         
        /*using (ApplicationDBContext db = _context)
        {
            db.DomainAccount.Add(account);

            db.SaveChanges();
        } 

        //_context.AdminRoles.Add(adminRole);
        //_context.SaveChanges();

        //return RedirectToAction("AdminRoles");

        }*/

        [HttpPost]
        public ActionResult Login(DomainAccount domain)
        {

            //ApplicationDBContext _applicationDB = _context();
            //return View(_domainAcc);
            using (ApplicationDbContext db = _context)
            {
                var dmn = db.DomainAccount.Where(d => d.AdminEmail == domain.AdminEmail && d.password == domain.password);
                if (/*dmn != null*/ dmn.Count() != 0)
                {

                    //    HttpContext.Session.SetString("AdminInfoId", dmn.AdminInfoId.ToString());
                    //    //Session["AdminInfoId"] = dmn.AdminInfoId.ToString;
                    //    HttpContext.Session.SetString("AdminEmail", dmn.AdminEmail);
                    //    //Session["AdminEmail"] = dmn.AdminEmail.ToString();
                    return RedirectToAction("DomainPage", "Domain");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                }
                return View();

            }
        }

        [HttpPost]
        public ActionResult LoginAdmin(AdminRoles adminR)
        {
            using (ApplicationDbContext db = _context)
            {
                var admnR = db.AdminRoles.Where(ar => ar.AdminEmail == adminR.AdminEmail && ar.Password == adminR.Password);
                if (admnR.Count() != 0)
                {
                    //if (admnR.Count() == 1)
                    //{
                    //    return RedirectToAction("AdminDashboard", "Admin");
                    //}

                    //else
                    //{
                    //    ModelState.AddModelError("", " You Admin Account");

                    //}

                    //if (admnR.Count() == 2)
                    //{
                    //    return RedirectToAction("SupportPage", "Support");
                    //}



                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                }
                return View();

            }
        }

        [HttpPost]
        public ActionResult LoginSE(SalesEmployeeRecord salesemp)
        {
            using (ApplicationDbContext db = _context)
            {
                var salesE = db.SalesEmployeeRecords.Where(se => se.EmployeeEmail == salesemp.EmployeeEmail && se.Password == salesemp.Password);
                if (salesE.Count() != 0)
                {

                    return RedirectToAction("EmployeeHomePage", "Employee");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                }
                return View();

            }
        }

        //public ActionResult AdminDashboard()
        //{
        //    if (Session["AdminInfo"] != null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }
        //}


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
