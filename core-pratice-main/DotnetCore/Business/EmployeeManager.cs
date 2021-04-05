using Business.Interface;
using BusinessEntities;
using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeManager(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public string AddEmployee(EmployeeVM model)
        {
            return _employeeRepository.AddEmployee(model);
        }

        public bool DeleteEmployee(int? Id)
        {
            return _employeeRepository.DeleteEmployee(Id);
        }

        public IList<EmployeeVM> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public EmployeeVM GetEmployee(int? Id)
        {
            return _employeeRepository.GetEmployee(Id);
        }

        public string UpdateEmployee(EmployeeVM model)
        {
            return _employeeRepository.UpdateEmployee(model);
        }
    }
}
