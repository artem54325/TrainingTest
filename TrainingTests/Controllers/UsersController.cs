using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using TrainingTests.Helpers;
using TrainingTests.Models;
using TrainingTests.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingTests.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        //private readonly IUserService _userService;
        private readonly MySqlContext _context;


        public UsersController(MySqlContext mySql)
        {
            _context = mySql;
            if (_context.Questions.Count() == 0)
            {
                PutData data = new PutData();

                Console.WriteLine($"Add database Article = {data.articles.Count}");
                _context.Articles.AddRange(data.articles);
                _context.Comments.AddRange(data.comments);

                _context.SuperUsers.Add(data.super);
                _context.TestThemes.AddRange(data.TestThemas);
                _context.TeacherUsers.AddRange(data.teacher);
                _context.StudentUsers.AddRange(data.studentUser);
                _context.TestStudents.AddRange(data.testStudents);
                _context.QuestionAnswers.AddRange(data.questionAnswers);
                _context.Tests.AddRange(data.tests);

                _context.Themes.AddRange(data.themes);
                _context.Questions.AddRange(data.questions1);
                _context.Questions.AddRange(data.questions2);
                _context.Questions.AddRange(data.questions3);
                _context.Marks.AddRange(data.Marks1);
                _context.Marks.AddRange(data.Marks2);

                _context.EventProfileUsers.AddRange(data.EventProfileUsers);
                _context.Meetups.AddRange(data.Meetups);
                _context.Speakers.AddRange(data.Speakers);

                _context.SaveChanges();
            }
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
        /// <response code="403">Bad username or password</response>  
        [AllowAnonymous]
        [HttpPost("Authorization")]
        public IActionResult Authorization(string username = "SuperUser", string password = "SuperUser")
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return StatusCode(403);// BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
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
        /// <response code="403">If doesn't test Id or doesn't it test</response>
        //[Authorize]
        [HttpPost("User")]
        public ActionResult<User> UserMy()
        {
            string usermane = "test";
            User user = null;// _context.GetUser(usermane);
            if (user == null)
            {
                return StatusCode(403);
            }

            return Ok(user);
        }

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
        [Authorize]
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
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _context.TeacherUsers.FirstOrDefault(a=>a.Username.Equals(username) && a.Password.Equals(password));
            if(user == null)
            {
                user = _context.StudentUsers.FirstOrDefault(a => a.Username.Equals(username) && a.Password.Equals(password));
            }
            if (user == null)
            {
                user = _context.SuperUsers.FirstOrDefault(a => a.Username.Equals(username) && a.Password.Equals(password));
            }
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
