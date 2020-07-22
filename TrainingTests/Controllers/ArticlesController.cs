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
        private readonly MySqlContext _context;

        public ArticlesController(MySqlContext mySql)
        {
            _context = mySql;

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
        public async Task<ActionResult<List<Article>>> GetAll()
        {
            var list = _context.Articles.Include(article => article.Comments).ThenInclude(a=>a.UserCreate).Include(a=>a.UserCreate).AsTracking().ToArray();

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
        public async Task<ActionResult<Article>> Get(string id = "article-1")
        {
            if (id == null)
            {
                return StatusCode(400);
            }

            var article = new ClickLikeHelpers(_context).ShowArticle(id);
            //article.Comments = _context.Comments.Where(a => a.ArticleId.Equals(article.Id)).ToList();
            //article.UserCreate = _context.StudentUsers.FirstOrDefault(a => a.Id.Equals(article.UserCreateId));
            //if (article.UserCreate == null)
            //{
            //    article.UserCreate = _context.TeacherUsers.FirstOrDefault(a => a.Id.Equals(article.UserCreateId));
            //}

            //if (article.UserCreate == null)
            //{
            //    article.UserCreate = _context.SuperUsers.FirstOrDefault(a => a.Id.Equals(article.UserCreateId));
            //}

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
        /// <response code="200">Return a comment or article, it depends on the type</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If doesn't had article</response>  
        //[Authorize]
        [HttpGet("ClickLike")]
        public async Task<ActionResult> ClickLike(string id, string type)
        {
            if (id == null || type == null)
            {
                return StatusCode(400);
            }
            string username = "UsernameStudent";
            Comment comment = null;
            Article article = null;

            switch (type)
            {
                case "comment":
                    comment = new ClickLikeHelpers(_context).ClickLikeComment(username, id);
                    return Ok(comment);
                    break;
                case "article":
                    article = new ClickLikeHelpers(_context).ClickLikeArticle(username, id);
                    return Ok(article);
                    break;
                default:
                    return StatusCode(400);
            }

            // Return article
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
        public async Task<ActionResult<Comment>> AddComment(string text, string idArticle)
        {
            string username = "UsernameStudent";
            // string username = null;
            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }
            Comment comment = new Comment();
            try
            {
                Article article = await _context.Articles.FirstOrDefaultAsync(a => a.Id.Equals(idArticle));

                comment.Id = Guid.NewGuid().ToString();
                comment.UserCreateId = username;
                comment.LikeUsers = new List<string>();
                comment.DislikeUsers = new List<string>();
                comment.Text = text;
                comment.ArticleId = idArticle;
                comment.UserCreate = user;
                comment.UserCreateId = user.Id;
                //comment.Article = article;

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
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
        public async Task<ActionResult<Comment>> DeleteComment(string idComment)
        {
            if (idComment == null) return StatusCode(400);
            string username = "UsernameStudent";
            Comment comment = await _context.Comments.FirstOrDefaultAsync(a => a.Id.Equals(idComment));

            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }

            if (comment == null)
            {
                return StatusCode(404);
            }

            if (comment.UserCreateId.Equals(user.Id))
            {
                return StatusCode(403);
            }
            // Return all comments
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
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
        public async Task<ActionResult<Article>> AddArticle(string title, string text)
        {
            string username = "UsernameStudent";
            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }

            Article article = new Article()
            {
                //Id = Guid.NewGuid().ToString(),
                Comments = new List<Comment>(),
                DislikeUsers = new List<string>(),
                LikeUsers = new List<string>(),
                Title = title,
                Text = text,
                Viewer = 0,
                DateCreate = DateTime.Now,
                DateUpdate = DateTime.Now
            };
            article.UserCreate = user;
            article.UserCreateId = user.Id;

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return Ok(article);
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
        public async Task<ActionResult<Article>> UpdateArticle(string id, string title, string text)
        {
            string username = "UsernameStudent";
            User user = GetUser(username);
            if (user == null)
            {
                return StatusCode(400);
            }
            Article article = await _context.Articles.FirstOrDefaultAsync(a => a.Id.Equals(id));
            if (article == null)
            {
                return StatusCode(400);
            }

            article.Title = title;
            article.Text = text;

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            // Return all comments
            return Ok(article);
        }

        private User GetUser(string username)
        {
            var userSt = _context.StudentUsers.FirstOrDefault(a => a.Username.Equals(username));
            if(userSt != null)
            {
                return userSt;
            }
            var userTeach = _context.TeacherUsers.FirstOrDefault(a => a.Username.Equals(username));
            if (userTeach != null)
            {
                return userTeach;
            }
            var userSuper = _context.SuperUsers.FirstOrDefault(a => a.Username.Equals(username));
            if (userTeach != null)
            {
                return userSuper;
            }
            return null;
        }
    }
}

