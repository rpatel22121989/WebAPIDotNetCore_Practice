using ExportApp.Repositories.EmployeeDetails;

namespace ExportApp.Services.Employee
{
    using ExportApp.Models;

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDetailsRepository _employeeDetails;
        public EmployeeService(IEmployeeDetailsRepository employeeDetails)
        {
            _employeeDetails = employeeDetails;
        }
        public List<Employee> GetEmployees()
        {
            return _employeeDetails.Get();
        }
        public Employee GetEmployeeById(int id)
        {
            return _employeeDetails.GetById(id);
        }
        public Employee GetEmployeeByEmail(string email)
        {
            return _employeeDetails.GetByEmail(email);
        }
        public Employee GetEmployeeByCredentials(string email, string password)
        {
            return _employeeDetails.GetByCredentials(email, password);
        }
        public int InsertEmployee(EmployeeInfo employeeInfo)
        {
            Employee employee = new Employee()
            {
                Name = employeeInfo.Name,
                Email = employeeInfo.Email,
                DateOfBirth = employeeInfo.DateOfBirth,
                Gender = employeeInfo.Gender,
                MaritalStatus = employeeInfo.MaritalStatus,
                CurrentAddress = employeeInfo.CurrentAddress,
                PermanentAddress = employeeInfo.PermanentAddress,
                City = employeeInfo.City,
                Nationality = employeeInfo.Nationality,
                Pincode = employeeInfo.Pincode,
                State = employeeInfo.State,
                Age = employeeInfo.Age,
                Salary = employeeInfo.Salary
            };
            return _employeeDetails.Insert(employee);
        }
        public void UpdateEmployee(int id, EmployeeInfo employeeInfo)
        {
            Employee employee = new Employee()
            {
                EmployeeId = id,
                Name = employeeInfo.Name,
                Email = employeeInfo.Email,
                DateOfBirth = employeeInfo.DateOfBirth,
                Gender = employeeInfo.Gender,
                MaritalStatus = employeeInfo.MaritalStatus,
                CurrentAddress = employeeInfo.CurrentAddress,
                PermanentAddress = employeeInfo.PermanentAddress,
                City = employeeInfo.City,
                Nationality = employeeInfo.Nationality,
                Pincode = employeeInfo.Pincode,
                State = employeeInfo.State,
                Age = employeeInfo.Age,
                Salary = employeeInfo.Salary
            };
            _employeeDetails.Update(employee);
        }
        public void DeleteEmployee(int id)
        {
            _employeeDetails.Delete(id);
        }
    }
}
