using ExportApp.Models;

namespace ExportApp.Repositories.EmployeeDetails
{
    public interface IEmployeeDetailsRepository
    {
        List<Employee> Get();
        Employee GetById(int id);
        Employee GetByEmail(string email);
        Employee GetByCredentials(string email, string password);
        int Insert(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
        void Delete(Employee employee);
    }
}
