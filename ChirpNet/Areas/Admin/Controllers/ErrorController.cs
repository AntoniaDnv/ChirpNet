using Microsoft.AspNetCore.Mvc;

namespace ChirpNet.Web.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode:int}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return statusCode switch
            {
                400 => View("BadRequest"),
                403 => View("Forbidden"),
                404 => View("NotFound"),
                _ => View("Error")
            };
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
