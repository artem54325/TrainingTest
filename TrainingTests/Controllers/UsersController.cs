using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using TrainingTests.Models;
using TrainingTests.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingTests.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        //private readonly IUserService _userService;
        private readonly MySqlContext _context;


        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
            //_userService = userService;
            _context = new MySqlContext(null);
        }


        ///<summary>
        /// Authorization user
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///     POST /Authorization
        /// </remarks>
        /// <returns>todoUser</returns>
        /// <response code="200">Return user</response>
        /// <response code="400">Bad username or password</response>  
        [AllowAnonymous]
        [HttpPost("Authorization")]
        public IActionResult Authorization(string username, string password)
        {
            //var user = _context.Authenticate(username, password);

            //if (user == null)
            //    return BadRequest(new { message = "Username or password is incorrect" });

            //return Ok(user);
            return null;
        }

        ///<summary>
        /// Get user
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///     POST /User
        /// </remarks>
        /// <returns>Get user</returns>
        /// <response code="200">Return user</response>
        /// <response code="400">If doesn't test Id or doesn't it test</response>
        //[Authorize]
        [HttpPost("User")]
        public ActionResult<User> UserMy()
        {
            string usermane = "test";
            User user = null;// _context.GetUser(usermane);
            if (user == null)
            {
                return StatusCode(400);
            }

            return Ok(user);
        }

        //[Authorize]
        //[HttpPost("Description")]
        //public JsonResult Description(string username)
        //{
        //    JObject user = new JObject();

        //    user.Add("name", "username");
        //    user.Add("url", "https://i.pinimg.com/474x/76/6c/f7/766cf770ea8dd3529bd8e0c41d6784be--lilo-and-stitch-cute-things.jpg");
        //    user.Add("type", "student");
        //    user.Add("type", "student");
        //    user.Add("type", "student");

        //    return new JsonResult(user);
        //}

        ///<summary>
        /// Updating user
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///     POST /Update
        /// </remarks>
        /// <returns>Update user</returns>
        /// <response code="200">User</response>
        /// <response code="400">Bad a request</response>
        //[Authorize]
        [HttpPost("Update")]
        public ActionResult<User> Update(User user)
        {
            string usermane = "test";
            //user = _context.UserUpdate(user);
            if (user.Username.Equals(usermane))
            {
                return StatusCode(400);
            }
            if (user == null)
            {
                return StatusCode(400);
            }

            return Ok(user);
        }
    }
}
