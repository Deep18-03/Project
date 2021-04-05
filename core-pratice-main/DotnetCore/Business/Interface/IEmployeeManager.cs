using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IEmployeeManager
    {
        IList<EmployeeVM> GetAllEmployees();
        EmployeeVM GetEmployee(int? Id);
        string AddEmployee(EmployeeVM model);
        string UpdateEmployee(EmployeeVM model);
        bool DeleteEmployee(int? Id);
    }
}
