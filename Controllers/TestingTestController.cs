using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingTests.Helpers;
using TrainingTests.Models;
using TrainingTests.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingTests.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestingTestController : Controller
    {
        private readonly MySqlContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TestingTestController(MySqlContext mySql, IHttpContextAccessor httpContextAccessor)//MySqlContext mySql
        {
            //_mySql = new LocalDatabase();   //mySql;
            _context = mySql;
            this.httpContextAccessor = httpContextAccessor;
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
        /// Checking password for question
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Password
        /// </remarks>
        /// <returns>Get all Article</returns>
        /// <response code="200">Returns list questions</response>
        /// <response code="400">Not found test</response>  
        [HttpPost("Password")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<Question>> Password([FromForm] string password)
        {

            // Check password and return questionnaire

            Test test = null;
            try
            {
                test = _context.Tests.AsEnumerable().FirstOrDefault(a => a.Passwords.Contains(password));
            }
            catch (Exception e)
            {
                return StatusCode(404);
            }

            test.Themes = _context.TestThemes.Where(a => a.TestId.Equals(test.Id)).ToList();
            for (var i = 0; i < test.Themes.Count; i++)
            {
                test.Themes[i].Thema = _context.Themes.FirstOrDefault(a => a.Id.Equals(test.Themes[i].ThemaId));
                test.Themes[i].Thema.Questions = _context.Questions.Where(a => a.ThemaId.Equals(test.Themes[i].Thema.Id)).ToList();
            }

            if (test == null)
            {
                return StatusCode(400);
            }

            var questions = CreateQuestionsStudent.TestToQuestions(test);

            Response.Cookies.Append("test", test.Id);
            Response.Cookies.Append("dateStart", DateTime.Now.Ticks.ToString());

            return Ok(questions);
        }

        ///<summary>
        /// Get questions from test
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///     GET /GetTest
        /// </remarks>
        /// <returns>Get all questions</returns>
        /// <response code="200">Returns list questions</response>
        /// <response code="400">Not found test</response>  
        [HttpPost("GetTest")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<Question>> GetTest([FromForm] string id)
        {

            // Check password and return questionnaire
            Test test = _context.Tests.FirstOrDefault(a => a.Id.Equals(id));
            test.Themes = _context.TestThemes.Where(a => a.TestId.Equals(test.Id)).ToList();
            for (var i = 0; i < test.Themes.Count; i++)
            {
                test.Themes[i].Thema = _context.Themes.FirstOrDefault(a => a.Id.Equals(test.Themes[i].ThemaId));
                test.Themes[i].Thema.Questions = _context.Questions.Where(a => a.ThemaId.Equals(test.Themes[i].Thema.Id)).ToList();
            }

            if (test == null)
            {
                return StatusCode(400);
            }

            var questions = CreateQuestionsStudent.TestToQuestions(test);

            Response.Cookies.Append("test", test.Id);
            Response.Cookies.Append("dateStart", DateTime.Now.Ticks.ToString());

            return Ok(questions);
        }

        ///<summary>
        /// Get all tests for users
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
            // Change data without questions and answers
            return Ok(_context.Tests.Select(a => new { id = a.Id, name = a.Name, description = a.Description, questions = a.Themes.Sum(q => q.CountQuest) }));
        }

        ///<summary>
        /// Get questions of test
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///     POST /WriteUser
        /// </remarks>
        /// <returns>Get test</returns>
        /// <response code="200">Returns list articles</response>
        /// <response code="400">If doesn't test Id or doesn't it test</response>
        [HttpPost("WriteUser")]
        public ActionResult<List<Question>> UserMy([FromForm] string fullname,
            string group, string departament)
        {
            Test test = null;
            //var testId = Request.Cookies["test"];

            //if (testId != null)
            //{
            //    test = _context.GetTest(testId);
            //}
            //else
            //{
            //    return StatusCode(400);
            //}
            //if (test == null)
            //{
            //    return StatusCode(400);
            //}

            // Added user in users
            //if(){// Если пользователь не авторизован!

            //}else{


            //}

            //Response.Cookies.Append("fullname", fullname);
            //Response.Cookies.Append("group", group);
            //Response.Cookies.Append("departament", departament);

            var questions = CreateQuestionsStudent.TestToQuestions(test);

            return Ok(questions);
        }

        ///<summary>
        /// Checking Result of test
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Result
        /// </remarks>
        /// <returns>Checking result</returns>
        /// <response code="200">Returns list articles</response>
        /// <response code="404">If the item is null</response>
        [HttpPost("Result")]
        public ActionResult<TestStudent> Result([FromBody] ICollection<Question> array)// List<Question> array
        {
            // Check answered and return database
            // Дописать null
            Console.WriteLine("result = " + (array == null));
            if (array == null)
            {
                return null;
            }

            string idTest = Request.Cookies["test"];
            Console.WriteLine($"test id = {idTest}");
            Test test = _context.Tests.FirstOrDefault(a => a.Id.Equals(idTest));
            //JArray array = (JArray)jObject["data"];

            TestStudent testStudent = new TestStudent();

            testStudent.Id = Guid.NewGuid().ToString();
            testStudent.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            testStudent.Test = test;
            //testStudent.TestId = test.Id;

            List<QuestionAnswer> questionAnswers = new List<QuestionAnswer>();
            QuestionAnswer answer;

            long allTime = 0;
            foreach (var quest in array)
            {
                answer = new QuestionAnswer();
                answer.Id = Guid.NewGuid().ToString();
                answer.TestStudent = testStudent;

                answer.QuestionId = quest.Id;// (string) array[i]["id"].ToString();
                answer.Question = _context.Questions.FirstOrDefault(a => a.Id.Equals(quest.Id));

                answer.Answers = quest.RightAnswers;// Users answers
                answer.Time = quest.Time;
                allTime += answer.Time;

                answer.Name = answer.Question.Name;
                answer.Description = answer.Question.Description;

                answer.QuestionAnswers = answer.Question.Answers;
                answer.QuestionRightAnswers = answer.Question.RightAnswers;
                answer.Appraisal = answer.Question.Appraisal;

                questionAnswers.Add(answer);
            }
            testStudent.Time = allTime;
            testStudent.QuestionAnswers = questionAnswers;
            testStudent = CreateQuestionsStudent.ResultTest(testStudent);
            testStudent.IpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            testStudent.DateFinish = DateTime.Now;

            return Ok(testStudent);
        }
    }
}
