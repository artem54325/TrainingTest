using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using TrainingTests.Helpers;
using TrainingTests.Models;
using TrainingTests.Repositories;
using TrainingTests.ViewModels;

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
        public async Task<ActionResult<UserView>> Authorization([FromBody] string username = "SuperUser", [FromBody] string password = "SuperUser")
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

            var response = new UserView
            {
                AccessToken = encodedJwt,
                Username = identity.Name
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
        [Authorize]
        [HttpGet("User")]
        public async Task<ActionResult<User>> UserMy()
        {
            string usermane = User.Identity.Name;
            var user = GetUser(usermane);

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
        /// <response code="403">Don't auth</response>
        [Authorize]
        [HttpPost("Update")]
        public async Task<ActionResult<User>> Update([FromBody] User user)
        {
            string usermane = User.Identity.Name;
            User userOld = GetUser(usermane);
            if (user == null)
            {
                return StatusCode(400);
            }

            if (userOld.Role == Roles.Student)
            {
                StudentUser student = await _context.StudentUsers.FirstOrDefaultAsync(a=>a.Id.Equals(userOld.Id));

                student.Firstname = user.Firstname;
                student.Secondname = user.Secondname;
                student.Email = user.Email;
                student.DateBirth = user.DateBirth;

                _context.StudentUsers.Update(student);
            }
            if (userOld.Role == Roles.SuperUser)
            {
                SuperUser super = _context.SuperUsers.FirstOrDefault(a => a.Id.Equals(userOld.Id));

                super.Firstname = user.Firstname;
                super.Secondname = user.Secondname;
                super.Email = user.Email;
                super.DateBirth = user.DateBirth;

                _context.SuperUsers.Update(super);
            }
            if (userOld.Role == Roles.Teacher)
            {
                TeacherUser teacher = _context.TeacherUsers.FirstOrDefault(a => a.Id.Equals(userOld.Id));

                teacher.Firstname = user.Firstname;
                teacher.Secondname = user.Secondname;
                teacher.Email = user.Email;
                teacher.DateBirth = user.DateBirth;

                _context.TeacherUsers.Update(teacher);
            }

            _context.SaveChanges();
            return Ok(userOld);
        }

        ///<summary>
        /// Registration new user
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///     POST /User
        /// </remarks>
        /// <returns>Get user</returns>
        /// <response code="200">Return new user</response>
        /// <response code="403">if there is such a username</response>
        [HttpPost("Registration")]
        public async Task<ActionResult<UserView>> Registration([FromBody] RegistrationView registration)
        {
            // Check username
            var sameUser = GetUser(registration.Username);
            if (sameUser != null || registration.Role == null || registration.Role.Equals(""))
            {
                return StatusCode(403);
            }

            switch (registration.Role)
            {
                case "super":
                    SuperUser user = new SuperUser
                    {
                        Username = registration.Username,
                        Email = registration.Email,
                        Firstname = registration.Firstname,
                        Secondname = registration.Secondname,
                        Password = registration.Password,
                        DateBirth = registration.DateBirth,
                        DateRegistration = DateTime.Now
                    };
                    await _context.SuperUsers.AddAsync(user);
                    break;

                case "teacher":
                    TeacherUser teacher = new TeacherUser
                    {
                        Username = registration.Username,
                        Email = registration.Email,
                        Firstname = registration.Firstname,
                        Secondname = registration.Secondname,
                        DateBirth = registration.DateBirth,
                        DateRegistration = DateTime.Now,
                        Password = registration.Password,
                        Discipline = registration.Discipline,
                        Department = registration.Department
                    };
                    await _context.TeacherUsers.AddAsync(teacher);
                    break;

                case "student":
                    StudentUser student = new StudentUser
                    {
                        Username = registration.Username,
                        Email = registration.Email,
                        Firstname = registration.Firstname,
                        Secondname = registration.Secondname,
                        Password = registration.Password,
                        DateBirth = registration.DateBirth,
                        DateRegistration = DateTime.Now
                    };
                    await _context.StudentUsers.AddAsync(student);
                    break;
            }
            await _context.SaveChangesAsync();

            var newUser = GetUser(registration.Username);

            return Ok(newUser);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _context.TeacherUsers.FirstOrDefault(a => a.Username.Equals(username) && a.Password.Equals(password));
            if (user == null)
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

        private User GetUser(string username)
        {
            var userSt = _context.StudentUsers.FirstOrDefault(a => a.Username.Equals(username));
            if (userSt != null)
            {
                return userSt;
            }
            var userTeach = _context.TeacherUsers.FirstOrDefault(a => a.Username.Equals(username));
            if (userTeach != null)
            {
                return userTeach;
            }
            var userSuper = _context.SuperUsers.FirstOrDefault(a => a.Username.Equals(username));
            if (userSuper != null)
            {
                return userSuper;
            }
            return null;
        }
    }
}
