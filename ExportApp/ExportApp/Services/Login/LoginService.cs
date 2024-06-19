using System.IdentityModel.Tokens.Jwt;
using ExportApp.Repositories.EmployeeDetails;
using ExportApp.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExportApp.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExportApp.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;

        private readonly IEmployeeDetailsRepository _employeeDetailsRepository;

        public LoginService(IConfiguration configuration, IEmployeeDetailsRepository employeeDetailsRepository)
        {
            _configuration = configuration;
            _employeeDetailsRepository = employeeDetailsRepository;
        }

        public object VerifyCredentialsAndGetJWTToken(CredentailsInfo credentailsInfo)
        {
            var employee = _employeeDetailsRepository.GetByCredentials(credentailsInfo.email, credentailsInfo.password);
            var connectionStr = _configuration.GetConnectionString("CompanyDBConnectionString");
            DataTable dt = SqlHelper.ExecuteDataTable(new SqlConnection(connectionStr), "sp_get_employee_based_on_credentials", new object[] { credentailsInfo.email, credentailsInfo.password });

            if (employee == null)
            {
                return null;
            }

            var claims = new Claim[] {
                 new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 // new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                 new Claim("UserId", employee.EmployeeId.ToString()),
                 new Claim("UserName", employee.Name),
                 new Claim(ClaimTypes.Email, employee.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: signingCredentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new { employee, jwtToken };
        }
    }
}
