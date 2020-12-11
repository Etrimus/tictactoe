using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Web.Spa
{
    [Route("")]
    [AllowAnonymous]
    public class SpaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/src/index.html");
        }
    }
}