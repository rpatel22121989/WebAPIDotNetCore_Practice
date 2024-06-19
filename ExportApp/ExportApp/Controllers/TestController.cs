using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExportApp.Controllers
{
    public class TestController : ControllerBase
    {
        // GET api/test/GetId

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //  GET api/test/GetId/1
        public string GetId(int id)
        {
            return "Your Id is:: " + id;
        }
    }
}
