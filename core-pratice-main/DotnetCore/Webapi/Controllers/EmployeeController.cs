using Business;
using Business.Interface;
using BusinessEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [ApiController]
  
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeManager _EmployeeManager;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeManager EmployeeManager, ILoggerFactory logger)
        {
            _EmployeeManager = EmployeeManager;
            _logger = logger.CreateLogger("MyLogger");
        }

        [HttpGet]
        [Route("Get")]
        public IList<EmployeeVM> Get()
        {
            return _EmployeeManager.GetAllEmployees();
            //return StatusCode(200, _EmployeeManager.GetAllEmployees().ToList());
        }
        [HttpGet, Route("GetEmployee/{id}")]
        public IActionResult GetEmployee(int id)
        {

            //return StatusCode(200, _EmployeeManager.GetEmployee(id));
            var employee = _EmployeeManager.GetEmployee(id);
            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return StatusCode(500);
            }
        }
        [HttpPost]
        [Route("PostEmployee")]
        public IActionResult PostEmployee([FromBody] EmployeeVM employee)
        {
            var emp = _EmployeeManager.AddEmployee(employee);
            if (emp != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

            //return StatusCode(201,_EmployeeManager.CreateEmployee(employee));
        }
        [HttpPut]
        [Route("PutEmployee")]
        public ActionResult PutEmployee([FromBody] EmployeeVM employee)
        {
            var emp = _EmployeeManager.UpdateEmployee(employee);
            if(emp!=null)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
            //return StatusCode(201,_EmployeeManager.UpdateEmployee(employee));
        }
        [HttpDelete]
        [Route("Delete/{id}i")]
        public bool Delete(int id)
        {
            bool result = _EmployeeManager.DeleteEmployee(id);
            return result;
            //return StatusCode(201, _EmployeeManager.DeleteEmployee(id));
        }
    }
}
