using AutoMapper;
using BusinessEntities;
using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public string AddEmployee(EmployeeVM model)
        {
            if (model != null)
            {
                Employee entity = _mapper.Map<EmployeeVM, Employee>(model);
                _context.Employee.Add(entity);
                _context.SaveChanges();
                return "Employee added";
            }

            return "Model is null";
        }

        public bool DeleteEmployee(int? Id)
        {
            Employee entity = _context.Employee.Find(Id);
            _context.Employee.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public IList<EmployeeVM> GetAllEmployees()
        {
            IList<EmployeeVM> EmployeeList = new List<EmployeeVM>();
            var result = _context.Employee.ToList();

            for (int i = 0; i < result.Count; i++)
            {
                EmployeeVM employee = _mapper.Map<EmployeeVM>(result[i]);
                EmployeeList.Add(employee);
            }
            return EmployeeList;
        }

        public EmployeeVM GetEmployee(int? Id)
        {
            EmployeeVM employeesEntities = _mapper.Map<EmployeeVM>(_context.Employee.Find(Id));
            return employeesEntities;
        }

        public string UpdateEmployee(EmployeeVM model)
        {
            if (model != null)
            {
                Employee entity = _context.Employee.Find(model.Id);
                _mapper.Map(model, entity);
                _context.Update(entity);
                _context.SaveChanges();
                return "Employee added";
            }

            return "Model is null";
        }
    }
    
}
