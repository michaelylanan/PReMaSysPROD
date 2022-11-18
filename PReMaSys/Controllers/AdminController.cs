using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PReMaSys.Data;
using PReMaSys.Models;
using System.Data;

namespace PReMaSys.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;


        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*  //For Excel
          private IHostingEnvironment Environment;
          public AdminController(IHostingEnvironment _environment)
          {
              Environment = _environment;   
          }*/
        public IActionResult ReportsPage()
        {
            return View();
        }

        /*  [HttpPost]
          public IActionResult ReportsPage(IFormFile postedFile, IXLWorksheet workSheet, Row row, object cell)
          {
              string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
              if (!Directory.Exists(path))
              {
                  Directory.CreateDirectory(path);
              }
              string fileName = Path.GetFileName(postedFile.FileName);
              string filePath = Path.Combine(path, fileName);
              using (FileStream stream = new FileStream(filePath, FileMode.Create))
              {
                  postedFile.CopyTo(stream);
              }
              DataTable dt = new DataTable();
              using (XLWorkbook workBook = new XLWorkbook(filePath))
              {
                  workSheet = workBook.Worksheet(1);
                  bool firstRow = true;
                  foreach (IXLRow rowin in workSheet.Rows())
                  {
                      if (firstRow)
                      {
                          foreach (IXLCell cellin in row.Cells())
                          {
                              dt.Columns.Add(cell.Value.ToString());
                          }
                          firstRow = false;
                      }
                      else
                      {
                          dt.Rows.Add();
                          int i = 0;
                          foreach (IXLCell cellin in row.Cells())
                          {
                              dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                              i++;
                          }
                      }
                  }
              }

              ViewBag.Data = dt;

              System.IO.File.Delete(filePath);

              return View();

          }*/


        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult Ranking()
        {
            return View();
        }

        public IActionResult Forecasts()
        {
            return View();
        }

        public IActionResult Diagnostic()
        {
            return View();
        }

        public IActionResult Descriptive()
        {
            return View();
        }

        /*Reports Page: EXCEL FILE-----------------------------------------------------------------------------------------------------------*/


        /*View of Rewards List -----------------------------------------------------------------------------------------------------------*/
        public IActionResult Reward()
        {
            var list = _context.Rewards.ToList();
            return View(list);
        }


        /*Allocation of Sales-Profit Points-----------------------------------------------------------------------------------------------------------*/
        public IActionResult ESalesProfitPoints()
        {
            var list = _context.SERecord.ToList();
            return View(list);
        }


        public IActionResult AddPoints(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ESalesProfitPoints");
            }

            var SEmployees = _context.SERecord.Where(r => r.SEmployeeRecordsID == id).SingleOrDefault();

            if (SEmployees == null)
            {
                return RedirectToAction("ESalesProfitPoints");
            }

            return View(SEmployees);
        }

        [HttpPost]
        public IActionResult AddPoints(int? id, SalesEmployeeRecord record, decimal temp)
        {
            var SEmployees = _context.SERecord.Where(s => s.SEmployeeRecordsID == id).SingleOrDefault();

            SEmployees.EmployeeNo = record.EmployeeNo;
            SEmployees.EmployeeLastname = record.EmployeeLastname;

            temp = Convert.ToDecimal(SEmployees.EmployeePoints) + Convert.ToDecimal(record.EmployeePoints);

            SEmployees.EmployeePoints = temp.ToString();
            SEmployees.DateModified = DateTime.Now;

            _context.SERecord.Update(SEmployees);
            _context.SaveChanges();

            return RedirectToAction("ESalesProfitPoints");
        }

        public IActionResult DeductPoints(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ESalesProfitPoints");
            }

            var SEmployees = _context.SERecord.Where(r => r.SEmployeeRecordsID == id).SingleOrDefault();

            if (SEmployees == null)
            {
                return RedirectToAction("ESalesProfitPoints");
            }

            return View(SEmployees);
        }

        [HttpPost]
        public IActionResult DeductPoints(int? id, SalesEmployeeRecord record, decimal temp)
        {
            var SEmployees = _context.SERecord.Where(s => s.SEmployeeRecordsID == id).SingleOrDefault();

            SEmployees.EmployeeNo = record.EmployeeNo;
            SEmployees.EmployeeLastname = record.EmployeeLastname;

            temp = Convert.ToDecimal(SEmployees.EmployeePoints) - Convert.ToDecimal(record.EmployeePoints);

            SEmployees.EmployeePoints = temp.ToString();
            SEmployees.DateModified = DateTime.Now;

            _context.SERecord.Update(SEmployees);
            _context.SaveChanges();

            return RedirectToAction("ESalesProfitPoints");
        }
    }
}
