//using Microsoft.AspNetCore.Mvc;

namespace ExportApp.Models
{
    //[Bind(include: "Name,Email")]
    public class EmployeeInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public string Nationality { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string Password { get; set; }
    }
}
