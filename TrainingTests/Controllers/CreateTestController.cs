using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TrainingTests.Helpers;
using TrainingTests.Models;
using TrainingTests.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingTests.Controllers
{
    [Route("api/[controller]")]
    public class CreateTestController : Controller
    {

        /*
         * 1. Первое будут темы, в которых будут вопросы
         * 2. Второе в Тестах будут темы, и кол-во вопрос и тд...
         */

        //private MySqlContext _mySql;
        private readonly MySqlContext _context;

        public CreateTestController(MySqlContext mySql)//MySqlContext mySql
        {
            //_mySql = new LocalDatabase();   //mySql;
            _context = mySql;
        }

        ///<summary>
        /// Saving all themes
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SaveThema
        ///     [
        ///     
        ///     ]
        /// </remarks>
        /// <returns>Save all themes</returns>
        /// <response code="200">Get Themes</response>
        /// <response code="400">Bad object for save</response>
        [HttpPost("SaveAllThemes")]
        //[Authorize]// [FromBody] ICollection<Thema> array
        public async Task<ActionResult<List<Thema>>> SaveAllThemes([FromBody] IEnumerable<Thema> themes)
        {
            // Проверить наличие таких же тем, если нету то сохранить
            string username = null;
            string teacherId = "teacher2";


            var dbThemes = _context.Themes.AsTracking();
            var dbQuestions = _context.Questions.AsTracking();

            foreach (Thema thema in themes)
            {
                var checkThema = await dbThemes.FirstOrDefaultAsync(a => a.Id.Equals(thema.Id));
                if(checkThema == null)
                {
                    _context.Themes.Add(thema);
                }
                else
                {
                    checkThema.Name = thema.Name;
                    checkThema.Description = thema.Description;
                    _context.Themes.Update(checkThema);
                }

                var nowQuestions = await dbQuestions.Where(a => a.ThemaId.Equals(thema.Id)).AsTracking().ToListAsync();
                if (thema.Questions != null)
                {
                    foreach (Question question in thema.Questions)
                    {
                        var findTestThema = nowQuestions.FirstOrDefault(a => a.Id.Equals(question.Id));

                        if (findTestThema == null)
                        {
                            _context.Add(question);
                        }
                        else
                        {
                            _context.Update(question);
                            nowQuestions.Remove(findTestThema);
                        }
                    }
                    //Remove elements
                    _context.RemoveRange(nowQuestions);
                }

                thema.TeacherUserId = teacherId;
                var findTest = dbThemes.FirstOrDefault(a => a.Id.Equals(thema.Id));
                Console.WriteLine($"Find test {findTest}");
                if (findTest == null)
                {
                    _context.Themes.Add(thema);
                }
                else
                {
                    _context.Themes.Update(thema);
                }
            }
            _context.SaveChanges();
            return Ok(themes);
        }

        ///<summary>
        /// Get all Tests
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Result
        /// </remarks>
        /// <returns>Get all Themes</returns>
        /// <response code="200">Returns list articles</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("GetAllThemes")]
        //[Authorize]
        public async Task<ActionResult<List<Thema>>> GetAllThemes()
        {
            try
            {
                // username = HttpContext.User.Identity.Name;
                var themes = await _context.Themes.Include(a => a.Questions).ToArrayAsync();
                return Ok(themes);
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
        }

        ///<summary>
        /// Get user Tests 
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetTest
        /// </remarks>
        /// <returns>Get all Tests</returns>
        /// <response code="200">Returns list tests</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("GetTest")]
        //[Authorize]
        public async Task<ActionResult<Test>> GetFullTest()
        {
            string username = "login2";
            TeacherUser teacher = await GetTeacherUser(username);
            List<Test> tests;
            try
            {
                username = HttpContext.User.Identity.Name;
                tests = _context.Tests.Where(a => a.TeacherUserId.Equals(username)).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }

            return Ok(tests);
        }

        ///<summary>
        /// Get all tests
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetAllTest
        /// </remarks>
        /// <returns>Get all Article</returns>
        /// <response code="200">Returns list articles</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("GetAllTest")]
        public async Task<ActionResult<List<Test>>> GetAllTest()
        {
            try
            {
                // username = HttpContext.User.Identity.Name;
                var tests = await _context.Tests.Include(a=>a.Themes).ToArrayAsync();   // _context.GetThemaFromUsername(username);

                return Ok(tests);
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
        }

        ///<summary>
        /// Saving all tests
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SaveAllTests
        ///     {
        ///     
        ///     }
        /// </remarks>
        /// <returns>Save all tests</returns>
        /// <response code="200">Get tests</response>
        /// <response code="400">Bad object for save</response>
        [HttpPost("SaveAllTests")]
        //[Authorize]// [FromBody] ICollection<Thema> array
        public async Task<ActionResult<List<Test>>> SaveAllTests([FromBody] IEnumerable<Test> tests)
        {
            // Проверить наличие таких же тем, если нету то сохранить
            string username = "login2";
            TeacherUser teacher = await GetTeacherUser(username);

            var dbTests = _context.Tests.AsTracking();
            var dbTestThemes = _context.TestThemes.AsTracking();

            foreach (Test test in tests)
            {
                // Исправить ошибку сохранения тестов !!!
                var checkTest = dbTests.FirstOrDefault(a=>a.Id.Equals(test.Id));

                if(checkTest == null)
                {
                    test.TeacherUser = teacher;
                    test.TeacherUserId = teacher.Id;
                    test.DateCreate = DateTime.Now;
                    _context.Tests.Add(test);
                }
                else
                {
                    if(checkTest.Name.Equals(test.Name) || checkTest.Description.Equals(test.Description) || 
                        checkTest.TimeAll == test.TimeAll || checkTest.Passwords.Equals(test.Passwords))
                    {
                        checkTest.DateLastChanges = DateTime.Now;
                        checkTest.Name = test.Name;
                        checkTest.Description = test.Description;
                        checkTest.TimeAll = test.TimeAll;
                        checkTest.Passwords = test.Passwords;
                        _context.Tests.Update(checkTest);
                    }
                }
                await _context.SaveChangesAsync();

                var nowTestThemes = await dbTestThemes.Where(a => a.TestId.Equals(test.Id)).AsTracking().ToListAsync();
                if (test.Themes != null)
                {
                    foreach (TestThema testThema in test.Themes)
                    {
                        var findTestThema = nowTestThemes.FirstOrDefault(a => a.Id.Equals(testThema.Id));

                        if (findTestThema == null)
                        {
                            testThema.TeacherUser = teacher;
                            testThema.TeacherUserId = teacher.Id;
                            testThema.Test = test;
                            testThema.TestId = test.Id;
                            testThema.Id = Guid.NewGuid().ToString();
                            _context.TestThemes.Add(testThema);
                        }
                        else
                        {
                            nowTestThemes.Remove(findTestThema);

                            findTestThema.TeacherUser = teacher;
                            findTestThema.TeacherUserId = teacher.Id;
                            findTestThema.Test = test;
                            findTestThema.TestId = test.Id;
                            findTestThema.CountBalls = testThema.CountBalls;
                            findTestThema.CountQuest = testThema.CountQuest;
                            findTestThema.TimeQuest = testThema.TimeQuest;
                            findTestThema.Test = testThema.Test;
                            findTestThema.TestId = testThema.TestId;

                            _context.TestThemes.Update(findTestThema);
                        }
                    }
                    Console.WriteLine($"Line remove");

                    _context.TestThemes.RemoveRange(nowTestThemes);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(await _context.Tests.Include(a => a.Themes).ToArrayAsync());
        }

        ///<summary>
        /// Get question
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetQuestion
        /// </remarks>
        /// <returns>Get Question</returns>
        /// <response code="200">Returns qustion</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("GetQuestion={id}")]
        public async Task<ActionResult<Question>> GetQuestion(string id)
        {
            Question question = _context.Questions.FirstOrDefault(a => a.Id.Equals(id));

            return Ok(question);
        }

        ///<summary>
        /// Remove a qustion
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /RemoveQuestion
        ///     {
        ///         idQustion = "id-qustion"
        ///     }
        /// </remarks>
        /// <returns>Return Qustion after remove</returns>
        /// <response code="200">Return Qustion after remove</response>
        /// <response code="404">If the item is null</response>
        [HttpPost("RemoveQuestion")]
        public async Task<ActionResult<Question>> RemoveQuestion(string idQuestion)
        {
            Question question = _context.Questions.FirstOrDefault(a => a.Id.Equals(idQuestion));
            _context.Questions.Remove(question); // (idQuestion);
            _context.SaveChanges();
            if (question == null)
            {
                return StatusCode(400);
            }
            if (question == null)
            {
                return StatusCode(400);
            }
            return Ok(question);
        }

        ///<summary>
        /// Remove a test
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /RemoveTest
        ///     {
        ///         idTest = "test-1"
        ///     }
        /// </remarks>
        /// <returns>Return test after remove</returns>
        /// <response code="200">Return test after remove</response>
        /// <response code="404">If the item is null</response>
        [HttpPost("RemoveTest")]
        public async Task<ActionResult<Test>> RemoveTest(string idTest)
        {
            var test = _context.Tests.FirstOrDefault(a => a.Equals(idTest));
            if (test == null)
            {
                return StatusCode(404);
            }
            _context.Remove(test);
            return Ok(test);
        }


        private async Task<TeacherUser> GetTeacherUser(string username)
        {
            return await _context.TeacherUsers.FirstOrDefaultAsync(a => a.Username.Equals(username));
        }
    }
}
