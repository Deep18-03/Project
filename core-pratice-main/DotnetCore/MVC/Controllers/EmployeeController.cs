using BusinessEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        public List<SelectListItem> Manager = new List<SelectListItem>
        {
            new SelectListItem { Value = "HR", Text = "HR" },
            new SelectListItem { Value = "Technical", Text = "Technical" },
            new SelectListItem { Value = "Naetwork", Text = "Naetwork"  },
        };
        public List<SelectListItem> Department = new List<SelectListItem>
        {
            new SelectListItem { Value = "HR", Text = "HR" },
            new SelectListItem { Value = "Technical", Text = "Technical" },
            new SelectListItem { Value = "Naetwork", Text = "Naetwork"  },
        };
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = (int)0.5)]
        public IActionResult EmployeeList()
        {
            IEnumerable<EmployeeVM> employees = null;
            using (var client = new HttpClient())
            {
                string getUri = "https://localhost:44382/Get";

                //HTTP GET
                var responseTask = client.GetAsync(getUri);

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<IEnumerable<EmployeeVM>>();
                    employees = readTask.Result;
                }
                else
                {
                    employees = Enumerable.Empty<EmployeeVM>();
                    ModelState.AddModelError(string.Empty, "Error.");
                }
            }
            return View(employees);
        }
        public IActionResult AddEmployee()
        {
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text");
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text");
            return View();
        }


        [HttpPost]
        public IActionResult AddEmployee(EmployeeVM employee)
        {
            try
            {
                _logger.LogInformation("Calling AddEmployee Method");
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        string postUri = "https://localhost:44382/PostEmployee";

                        //HTTP POST
                        var postTask = client.PostAsJsonAsync<EmployeeVM>(postUri, employee);
                        var result = postTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Error.");
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while Adding an Employee" + ex.Message.ToString());
                return View(employee);
            }
            
        }

        [NonAction]
        public EmployeeVM GetEmployeeById(int? id)
        {
            try
            {
                _logger.LogInformation("Calling GetEmployeeById() method");
                EmployeeVM employee = null;

                using (var client = new HttpClient())
                {
                    string getUri = "https://localhost:44382/GetEmployee/" + id;

                    //HTTP GET
                    var responseTask = client.GetAsync(getUri);

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadFromJsonAsync<EmployeeVM>();
                        employee = readTask.Result;
                    }
                    else
                    {
                        employee = null;
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while GetEmployeeById()" + ex.Message.ToString());
                return null;
            }

        }

        [HttpGet]
        public IActionResult EditEmployee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = GetEmployeeById(id);
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text", selectedValue: employee.Manager);
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text", selectedValue: employee.Manager);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        [HttpPost]
        
        public IActionResult EditEmployee(EmployeeVM employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        string putUri = "https://localhost:44382/PutEmployee";

                        //HTTP GET
                        var responseTask = client.PutAsJsonAsync<EmployeeVM>(putUri, employee);

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("EmployeeList", "Employee");
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text", selectedValue: employee.Manager);
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text", selectedValue: employee.Manager);
            return View(employee);
        }
    }
}
