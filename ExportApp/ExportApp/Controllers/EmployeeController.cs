using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using ExportApp.Models;
using ExportApp.Services.Employee;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ExportApp.Controllers
{
    [Authorize]
    // [NonController]
    [ApiController]
    [Route("/api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<ExportController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<ExportController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // [NonAction]
        [HttpGet]
        [Route("GetAllEmployee")]
        //public IActionResult GetAllEmployee()
        public ActionResult<List<Employee>> GetAllEmployee()
        {
            List<Employee> employees = _employeeService.GetEmployees();
            // Microsoft.AspNetCore.Mvc.AcceptedAtActionResult acceptedAtActionResult = new AcceptedAtActionResult("GetAllEmployee", "Employee", null, employees);
            // Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult acceptedAtRouteResult = new AcceptedAtRouteResult(null, employees);
            // Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult acceptedAtRouteResult = new AcceptedAtRouteResult("DefaultRoute1", null, employees);
            // Microsoft.AspNetCore.Mvc.AcceptedResult acceptedResult = new AcceptedResult();
            // Microsoft.AspNetCore.Mvc.AcceptedResult acceptedResult = new AcceptedResult("https://localhost:7067/api/Employee/GetAllEmployee", employees);
            // Microsoft.AspNetCore.Mvc.AcceptedResult acceptedResult = new AcceptedResult(new Uri("https://localhost:7067/api/Employee/GetAllEmployee"), employees);
            // Microsoft.AspNetCore.Mvc.ActionResult<List<Employee>> actionResult = new ActionResult<List<Employee>>(employees);
            // Microsoft.AspNetCore.Mvc.BadRequestObjectResult badRequestObjectResult = new BadRequestObjectResult(this.ModelState);
            // Microsoft.AspNetCore.Mvc.BadRequestObjectResult badRequestObjectResult = new BadRequestObjectResult(new { error = "Bad Request Parameters....Please pass a valid request parameters." });
            // Microsoft.AspNetCore.Mvc.BadRequestResult badRequestResult = new BadRequestResult();

            // Microsoft.AspNetCore.Mvc.ContentResult contentResult = new ContentResult();
            // contentResult.StatusCode = (int)HttpStatusCode.OK;
            // contentResult.ContentType = "application/json";
            // contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(employees);

            // Microsoft.AspNetCore.Mvc.JsonResult jsonResult = new JsonResult(employees);
            // jsonResult.StatusCode = (int)HttpStatusCode.OK;
            // jsonResult.ContentType = "application/json";
            // jsonResult.SerializerSettings = JsonSerializerOptions.Default;

            // Microsoft.AspNetCore.Mvc.NotFoundObjectResult notFoundObjectResult = new NotFoundObjectResult(new { error = "Employee Not Found...." });
            // Microsoft.AspNetCore.Mvc.NotFoundResult notFoundResult = new NotFoundResult();
            // Microsoft.AspNetCore.Mvc.ObjectResult objectResult = new ObjectResult(employees);
            // objectResult.StatusCode = (int)HttpStatusCode.OK;

            return StatusCode((int)HttpStatusCode.OK, employees);
            // return acceptedAtActionResult;
            // return acceptedAtRouteResult;
            // return acceptedResult;
            // return actionResult;
            // return contentResult;
            // return badRequestObjectResult;
            // return badRequestResult;
            // return jsonResult;
            // return objectResult;
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            Employee employee = _employeeService.GetEmployeeById(id);
            return StatusCode((int)HttpStatusCode.OK, employee);
        }

        [HttpGet]
        [Route("GetEmployeeByEmail/{email}")]
        public IActionResult GetEmployeeByEmail(string email)
        {
            Employee employee = _employeeService.GetEmployeeByEmail(email);
            return StatusCode((int)HttpStatusCode.OK, employee);
        }

        [HttpPost]
        [Route("SaveEmployeeDetails")]
        public IActionResult SaveEmployeeDetails([Bind(include: "Name,Email")] EmployeeInfo employeeInfo)
        {
            int employeeId = _employeeService.InsertEmployee(employeeInfo);
            return StatusCode((int)HttpStatusCode.OK, new { employeeId = employeeId });
        }

        [HttpPut]
        [Route("UpdateEmployeeDetails/{id}")]
        public IActionResult UpdateEmployeeDetails(EmployeeInfo employeeInfo, int id)
        {
            _employeeService.UpdateEmployee(id, employeeInfo);
            return StatusCode((int)HttpStatusCode.OK, new { employeeId = id });
        }

        [HttpDelete]
        [Route("DeleteEmployeeDetails/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeService.DeleteEmployee(id);
            return StatusCode((int)HttpStatusCode.OK, new { employeeId = id });
        }
    }
}
