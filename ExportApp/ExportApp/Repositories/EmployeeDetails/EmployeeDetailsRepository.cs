using ExportApp.Context;
using ExportApp.Models;

namespace ExportApp.Repositories.EmployeeDetails
{
    public class EmployeeDetailsRepository : IEmployeeDetailsRepository
    {
        private readonly CompanyDBContext _context;
        public EmployeeDetailsRepository()
        {
        
        }

        public EmployeeDetailsRepository(CompanyDBContext context)
        {
            _context = context;
        }
        public List<Employee> Get()
        {
            return _context.Employees.ToList();
        }
        public Employee GetById(int id)
        {
            var employee = _context.Employees.Where(employee => employee.EmployeeId == id).FirstOrDefault();
            return employee;
        }
        public Employee GetByEmail(string email)
        {
            var employee = _context.Employees.Where(employee => employee.Email == email).FirstOrDefault();
            return employee;
        }
        public Employee GetByCredentials(string email, string password)
        {
            var employee = _context.Employees.Where(employee => employee.Email == email && employee.Password == password).FirstOrDefault();
            return employee;
        }
        public int Insert(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee.EmployeeId;
        }
        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var employee = this.GetById(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }
        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
