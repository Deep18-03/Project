using Business.Interface;
using BusinessEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DotnetCore.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
    {
        private readonly IEmployeeManager _EmployeeManager;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeManager EmployeeManager, ILoggerFactory logger)
        {
            _EmployeeManager = EmployeeManager;
            logger.AddFile("Loggs/Logger.txt");
            _logger = logger.CreateLogger("MyCategory");
        }
        public List<SelectListItem> Manager = new List<SelectListItem>
        {
            new SelectListItem { Value = "HR", Text = "HR" },
            new SelectListItem { Value = "Technical", Text = "Technical" },
            new SelectListItem { Value = "Naetwork", Text = "Naetwork"  },
        };
        public List<SelectListItem> Department= new List<SelectListItem>
        {
            new SelectListItem { Value = "HR", Text = "HR" },
            new SelectListItem { Value = "Technical", Text = "Technical" },
            new SelectListItem { Value = "Naetwork", Text = "Naetwork"  },
        };
        public ActionResult Index()
        {
            return View();
        }
        // GET: Employee
        [ResponseCache(Duration = (int)0.5)]
        public IActionResult List()
        {
            var result = _EmployeeManager.GetAllEmployees().ToList();
            return View(result);
        }
        // GET: Employee/Details/5
        public IActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var Employee = _EmployeeManager.GetEmployee(id);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text");
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeVM Employee)
        {
          
            if (ModelState.IsValid)
            {
                string update = _EmployeeManager.AddEmployee(Employee);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text");
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text");
            return View(Employee);
        }

        // GET: Employee/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employee = _EmployeeManager.GetEmployee(id);
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text", selectedValue: Employee.Manager);
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text", selectedValue: Employee.Manager);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeVM Employee)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string update = _EmployeeManager.UpdateEmployee(Employee);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ManagerList = new SelectList(Manager, "Value", "Text",selectedValue: Employee.Manager);
            ViewBag.DepartmentList = new SelectList(Manager, "Value", "Text", selectedValue: Employee.Manager);
            return View(Employee);
        }

        // GET: Employee/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employee = _EmployeeManager.GetEmployee(id);
            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Employee = _EmployeeManager.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }
    }
    
}
