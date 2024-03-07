using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null)
                return NotFound();
            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            //elimina los try catch para dejar las exceptions ser manejadas por a middleware
            //try
            //{

                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "Error: "+ex.Message);
            //}
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }

    }
}
