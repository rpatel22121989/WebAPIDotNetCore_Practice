using ExportApp.Models;

namespace ExportApp.Services.Login
{
    public interface ILoginService
    {
        object VerifyCredentialsAndGetJWTToken(CredentailsInfo credentailsInfo);
    }
}
