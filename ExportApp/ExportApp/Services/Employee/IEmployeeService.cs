namespace ExportApp.Services.Employee
{
    using ExportApp.Models;
    public interface IEmployeeService
    {
        List<Employee> GetEmployees();
        Employee GetEmployeeById(int id);
        Employee GetEmployeeByEmail(string email);
        Employee GetEmployeeByCredentials(string email, string password);
        int InsertEmployee(EmployeeInfo employee);
        void UpdateEmployee(int id, EmployeeInfo employee);
        void DeleteEmployee(int id);
    }
}
