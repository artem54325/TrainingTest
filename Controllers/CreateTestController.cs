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
            if (_context.Questions.Count() == 0)
            {
                PutData data = new PutData();

                Console.WriteLine($"Add database Article = {data.articles.Count}");
                _context.Articles.AddRange(data.articles);
                _context.Comments.AddRange(data.comments);

                _context.SuperUsers.Add(data.super);
                _context.TeacherUsers.AddRange(data.teacher);
                _context.StudentUsers.AddRange(data.studentUser);
                _context.TestStudents.AddRange(data.testStudents);
                _context.QuestionAnswers.AddRange(data.questionAnswers);
                _context.Tests.AddRange(data.tests);

                _context.Themes.AddRange(data.themes);
                _context.TestThemes.AddRange(data.TestThemas);
                _context.Questions.AddRange(data.questions1);
                _context.Questions.AddRange(data.questions2);
                //_context.Questions.AddRange(data.questions3);
                _context.Marks.AddRange(data.Marks1);
                _context.Marks.AddRange(data.Marks2);

                _context.EventProfileUsers.AddRange(data.EventProfileUsers);
                _context.Meetups.AddRange(data.Meetups);
                _context.Speakers.AddRange(data.Speakers);

                _context.SaveChanges();
            }
        }

        ///<summary>
        /// Saving thema
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SaveThema
        ///     {
        ///     
        ///     }
        /// </remarks>
        /// <returns>Save all themes</returns>
        /// <response code="200">Get Thema</response>
        /// <response code="400">Bad object for save</response>
        [HttpPost("SaveThema")]
        //[Authorize]
        public ActionResult<Thema> SaveThema(Thema thema)
        {
            string username = null;
            TeacherUser user = null;

            try
            {
                username = HttpContext.User.Identity.Name;
                //user = null;
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
            thema.TeacherUserId = username;

            return Ok(thema);
        }

        ///<summary>
        /// Saving all themes
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SaveThema
        ///     {
        ///     
        ///     }
        /// </remarks>
        /// <returns>Save all themes</returns>
        /// <response code="200">Get Themes</response>
        /// <response code="400">Bad object for save</response>
        [HttpPost("SaveAllThemes")]
        //[Authorize]// [FromBody] ICollection<Thema> array
        public ActionResult<ICollection<Thema>> SaveAllThemes([FromBody] ICollection<Thema> themes)
        {
            // Проверить наличие таких же тем, если нету то сохранить
            string username = null;
            string teacherId = "teacher2";


            var dbThemes = _context.Themes.AsNoTracking();
            var dbQuestions = _context.Questions.AsNoTracking();

            foreach (Thema thema in themes)
            {
                var nowQuestions = dbQuestions.Where(a => a.ThemaId.Equals(thema.Id)).AsNoTracking().ToList();
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

            try
            {
                //_context.Questions.AddRange(newQuestions);
                //_context.Questions.UpdateRange(oldQuestions);

                //_context.Themes.AddRange(newThemes);
                //_context.Themes.UpdateRange(oldThemes);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error save all themes {e.Message}");
            }

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
        public ActionResult<List<Thema>> GetAllThemes()
        {
            // string username = null;
            List<Thema> Themes;
            try
            {
                // username = HttpContext.User.Identity.Name;
                Themes = _context.Themes.ToList();// _context.GetThemaFromUsername(username);
                for (var i = 0; i < Themes.Count(); i++)
                {
                    Themes[i].Questions = _context.Questions.Where(a => a.ThemaId.Equals(Themes[i].Id)).ToList();
                }
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }

            return Ok(Themes);
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
        public ActionResult<List<Test>> GetFullTest()
        {
            string username = null;
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
        public ActionResult<List<Test>> GetAllTest()
        {
            List<Test> tests;
            try
            {
                // username = HttpContext.User.Identity.Name;
                tests = _context.Tests.ToList();// _context.GetThemaFromUsername(username);
                for (var i = 0; i < tests.Count(); i++)
                {
                    tests[i].Themes = _context.TestThemes.Where(a => a.TestId.Equals(tests[i].Id)).ToList();
                }
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
            return Ok(_context.Tests);
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
        public ActionResult<ICollection<Test>> SaveAllTests([FromBody] ICollection<Test> tests)
        {
            // Проверить наличие таких же тем, если нету то сохранить
            string username = null;
            string teacherId = "teacher2";

            var dbTests = _context.Tests.AsNoTracking().AsQueryable();
            var dbTestThemes = _context.TestThemes.AsNoTracking().AsQueryable();

            Console.WriteLine($"Save tests {tests.Count()}");
            //Console.WriteLine($"Save dbTestThemes {dbTestThemes.Count()}");
            //TestThema testThema = new TestThema();
            //testThema.Id = "eqweqweq23";
            //testThema.TeacherUserId = teacherId;
            //_context.TestThemes.Add(testThema);

            foreach (Test test in tests)
            {
                // Исправить ошибку сохранения тестов !!!
                //var nowTestThemes = dbTestThemes.Where(a => a.TestId.Equals(test.Id)).AsNoTracking().ToList();
                if (test.Themes != null)
                {
                    foreach (TestThema testThema in test.Themes)
                    {
                        testThema.TeacherUserId = teacherId;
                        //var findTestThema = nowTestThemes.FirstOrDefault(a => a.Id.Equals(testThema.Id));
                        testThema.Test = null;
                        //testThema.TestId = "qwe";
                        testThema.TeacherUserId = null;
                        testThema.Id = (new Random().Next() * 10).ToString() + "qwe";
                        //if (findTestThema == null)
                        if (true)
                        {
                            Console.WriteLine($"Line add {testThema.Id} {testThema.TeacherUserId} {testThema.TestId} {testThema.ToString()}");
                            _context.TestThemes.Add(testThema);
                        }
                        else
                        {
                            Console.WriteLine($"Line update {testThema.Id} {testThema.TeacherUserId} {testThema.TestId}");
                            //_context.TestThemes.Update(testThema);
                            //nowTestThemes.Remove(findTestThema);
                        }
                    }
                    Console.WriteLine($"Line remove");
                    //Remove elements
                    //_context.TestThemes.RemoveRange(nowTestThemes);
                }

                //test.TeacherUserId = teacherId;
                //var findTest = dbTests.FirstOrDefault(a => a.Id.Equals(test.Id));
                //Console.WriteLine($"Find test {findTest}");
                //if (findTest == null)
                //{
                //    _context.Tests.Add(test);
                //}
                //else
                //{
                //    _context.Tests.Update(test);
                //}
            }
            try
            {// AutomaticMigrationsEnabled = true;
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error save all themes {e.Message}");
            }

            return Ok(tests);
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
        public ActionResult<Question> GetQuestion(string id)
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
        public ActionResult<Question> RemoveQuestion(string idQuestion)
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
        public ActionResult<Test> RemoveTest(string idTest)
        {
            var test = _context.Tests.FirstOrDefault(a => a.Equals(idTest));
            if (test == null)
            {
                return StatusCode(400);
            }
            _context.Remove(test);
            if (test == null)
            {
                return StatusCode(400);
            }
            return Ok(test);
        }
    }
}
