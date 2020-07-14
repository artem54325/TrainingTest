using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrainingTests.Helpers;
using TrainingTests.Models;
using TrainingTests.Repositories;

//Swagger
// https://www.c-sharpcorner.com/article/how-to-use-swagger-with-asp-net-core-web-apis/
// https://docs.microsoft.com/ru-ru/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio
// https://github.com/reactiveui/refit
// 

namespace TrainingTests.Controllers
{// /api/Articles/GetAll
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly MySqlContext _context;

        public ArticlesController(ILogger<ArticlesController> logger, MySqlContext mySql)
        {
            _logger = logger;
            _context = mySql;
            //_localContext = new LocalRepository();
            //var putData = new PutData();
            //_localContext.Articles = putData.articles;
            //_localContext.Comments = putData.comments;
            if (_context.Articles.Count() == 0)
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
        /// Get all articles
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Article
        /// </remarks>
        /// <returns>Get all Article</returns>
        /// <response code="200">Returns list articles</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public ActionResult<List<Article>> GetAll()
        {
            var list = _context.Articles.Include(article => article.Comments).ToArray();// _context.Articles;
            // Console.WriteLine($"articles get all count = {list.Count()}");

            return Ok(list);
        }


        ///<summary>
        /// Get an article
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Todo
        ///     {
        ///         id = "article-1"
        ///     }
        ///
        /// </remarks>
        /// <returns>Get an TodoArticle</returns>
        /// <response code="200">Returns an article</response>
        /// <response code="400">Bad request</response>
        [HttpGet("Get")]
        [AllowAnonymous]
        public ActionResult<Article> Get(string id = "article-1")
        {
            if (id == null)
            {
                return StatusCode(400);
            }

            var article = new ClickLikeHelpers(_context).ShowArticle(id);
            article.Comments = _context.Comments.Where(a => a.ArticleId.Equals(article.Id)).ToList();
            article.UserCreate = _context.StudentUsers.FirstOrDefault(a => a.Id.Equals(article.UserCreateId));
            if (article.UserCreate == null)
            {
                article.UserCreate = _context.TeacherUsers.FirstOrDefault(a => a.Id.Equals(article.UserCreateId));
            }

            if (article.UserCreate == null)
            {
                article.UserCreate = _context.SuperUsers.FirstOrDefault(a => a.Id.Equals(article.UserCreateId));
            }

            return Ok(article);
        }

        ///<summary>
        /// Click like
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Todo
        ///     {
        ///         id = "article-1",
        ///         type = "comment," // comment/article
        ///     }
        ///
        /// </remarks>
        /// <returns>Get TodoBoolean</returns>
        /// <response code="200">Returns a status</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If doesn't had article</response>  
        //[Authorize]
        [HttpGet("ClickLike")]
        public ActionResult<Boolean> ClickLike(string id, string type)
        {
            if (id == null || type == null)
            {
                return StatusCode(400);
            }
            string user = "username";
            JObject jObject = new JObject();
            bool status = false;
            switch (type)
            {
                case "comment":
                    status = true;
                    new ClickLikeHelpers(_context).ClickLikeComment(user, id);
                    break;
                case "article":
                    status = true;
                    new ClickLikeHelpers(_context).ClickLikeArticle(user, id);
                    break;
                default:
                    jObject.Add("status", false);
                    jObject.Add("error", "Error with type");
                    return StatusCode(400);
            }

            // Return article
            return Ok(status);
        }


        ///<summary>
        /// Add comment to article
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Todo
        ///     {
        ///         text = "example-1",
        ///         idArticle = "article-1,"
        ///     }
        ///
        /// </remarks>
        /// <returns>Get TodoComment</returns>
        /// <response code="200">Returns a comment</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If doesn't had article</response>  
        //[Authorize]
        [HttpPost("AddComment")]
        public ActionResult<Comment> AddComment(string text, string idArticle)
        {
            string username = "user";
            // string username = null;
            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }
            Comment comment = new Comment();
            try
            {
                Article article = _context.Articles.Where(a => a.Id.Equals(idArticle)).FirstOrDefault();

                comment.Id = Guid.NewGuid().ToString();
                comment.UserCreateId = username;
                comment.LikeUsers = new List<string>();
                comment.DislikeUsers = new List<string>();
                comment.Text = text;
                comment.ArticleId = idArticle;
                comment.UserCreate = user;
                //comment.Article = article;

                _context.Comments.Add(comment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
            // Return all comments
            return Ok(comment);
        }

        ///<summary>
        /// Remove a Comment
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Todo
        ///     {
        ///         id = "comment-1",
        ///     }
        ///
        /// </remarks>
        /// <returns>Get TodoComment</returns>
        /// <response code="200">Returns a article</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If doesn't had article</response>  
        /// <response code="403">Нету прав на удаление</response>  
        [HttpPost("DeleteComment")]
        public ActionResult<Comment> DeleteComment(string idComment)
        {
            if (idComment == null) return StatusCode(400);
            string username = "username";
            Comment comment = _context.Comments.Where(a => a.Id.Equals(idComment)).FirstOrDefault();

            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }

            if (comment == null)
            {
                return NotFound();
            }

            if (!comment.UserCreateId.Equals(username))
            {
                return StatusCode(403);
            }
            // Return all comments
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return StatusCode(200);
        }

        ///<summary>
        /// Add article
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Todo
        ///     {
        ///         title = "example-1",
        ///         text = "comment," // comment/article
        ///     }
        ///
        /// </remarks>
        /// <returns>Get TodoArticle</returns>
        /// <response code="200">Returns a article</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If doesn't had article</response>  
        [HttpPost("AddArticle")]
        public ActionResult<Article> AddArticle(string title, string text)
        {
            string username = null;
            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }

            Article article = new Article()
            {
                Id = Guid.NewGuid().ToString(),
                Comments = new List<Comment>(),
                DislikeUsers = new List<string>(),
                LikeUsers = new List<string>(),
                Title = title,
                Text = text,
                Viewer = 0,
                UserCreate = user,
                UserCreateId = username
            };

            _context.Articles.Add(article);
            _context.SaveChanges();

            return Ok(200);
        }

        ///<summary>
        /// Update Article
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Todo
        ///     {
        ///     id = "article-1",
        ///     title = "comment,"
        ///     text = "comment,"
        ///     }
        ///
        /// </remarks>
        /// <returns>Updated TodoArticle</returns>
        /// <response code="200">Returns a article</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If doesn't had article</response>  
        [HttpPost("UpdateArticle")]
        public ActionResult<Article> UpdateArticle(string id, string title, string text)
        {
            string username = "username";
            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }
            Article article = _context.Articles.Where(a => a.Id.Equals(id)).FirstOrDefault();
            if (article == null)
            {
                return StatusCode(400);
            }

            article.Title = title;
            article.Text = text;

            _context.Articles.Update(article);
            _context.SaveChanges();
            // Return all comments
            return Ok(article);
        }

        private User GetUser(string username)
        {
            return new StudentUser();
        }
    }
}

