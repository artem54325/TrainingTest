//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using TrainingTests.Controllers;
//using TrainingTests.Helpers;
//using TrainingTests.Models;
//using TrainingTests.Repositories;
//using Xunit;
//using Microsoft.AspNetCore.Mvc;

//namespace xUnitTest.ControllersTest
//{
//    public class ArticlesControllerTests : IDisposable
//    {
//        private readonly MySqlContext mySql;
//        private readonly ArticlesController controller;

//        public ArticlesControllerTests()
//        {
//            if (false)
//            {

//                PutData putData = new PutData();

//                var listSuperUser = new List<SuperUser>();//.AsQueryable();
//                listSuperUser.Add(putData.super);

//                var dbArticles = new List<Article>(putData.articles).AsQueryable();
//                var dbComments = new List<Comment>(putData.comments).AsQueryable();
//                var dbSuperUser = listSuperUser.AsQueryable();
//                var dbTestStudent = new List<TestStudent>(putData.testStudents).AsQueryable();
//                var dbStudentUser = new List<StudentUser>(putData.students).AsQueryable();

//                var mockSet = new Mock<DbSet<Article>>();
//                mockSet.As<IQueryable<Article>>().Setup(a => a.Provider).Returns(dbArticles.Provider);
//                mockSet.As<IQueryable<Article>>().Setup(a => a.Expression).Returns(dbArticles.Expression);
//                mockSet.As<IQueryable<Article>>().Setup(a => a.ElementType).Returns(dbArticles.ElementType);
//                mockSet.As<IQueryable<Article>>().Setup(a => a.GetEnumerator()).Returns(dbArticles.GetEnumerator());

//                var mockSetCom = new Mock<DbSet<Comment>>();
//                mockSetCom.As<IQueryable<Comment>>().Setup(a => a.Provider).Returns(dbComments.Provider);
//                mockSetCom.As<IQueryable<Comment>>().Setup(a => a.Expression).Returns(dbComments.Expression);
//                mockSetCom.As<IQueryable<Comment>>().Setup(a => a.ElementType).Returns(dbComments.ElementType);
//                mockSetCom.As<IQueryable<Comment>>().Setup(a => a.GetEnumerator()).Returns(dbComments.GetEnumerator());

//                var mockSetSuperUsers = new Mock<DbSet<SuperUser>>();
//                mockSetSuperUsers.As<IQueryable<SuperUser>>().Setup(a => a.Provider).Returns(dbSuperUser.Provider);
//                mockSetSuperUsers.As<IQueryable<SuperUser>>().Setup(a => a.Expression).Returns(dbSuperUser.Expression);
//                mockSetSuperUsers.As<IQueryable<SuperUser>>().Setup(a => a.ElementType).Returns(dbSuperUser.ElementType);
//                mockSetSuperUsers.As<IQueryable<SuperUser>>().Setup(a => a.GetEnumerator()).Returns(dbSuperUser.GetEnumerator());

//                var mockSetTestStudent = new Mock<DbSet<TestStudent>>();
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.Provider).Returns(dbTestStudent.Provider);
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.Expression).Returns(dbTestStudent.Expression);
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.ElementType).Returns(dbTestStudent.ElementType);
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.GetEnumerator()).Returns(dbTestStudent.GetEnumerator());

//                var mockSetStudentUser = new Mock<DbSet<StudentUser>>();
//                mockSetStudentUser.As<IQueryable<StudentUser>>().Setup(a => a.Provider).Returns(dbStudentUser.Provider);
//                mockSetStudentUser.As<IQueryable<StudentUser>>().Setup(a => a.Expression).Returns(dbStudentUser.Expression);
//                mockSetStudentUser.As<IQueryable<StudentUser>>().Setup(a => a.ElementType).Returns(dbStudentUser.ElementType);
//                mockSetStudentUser.As<IQueryable<StudentUser>>().Setup(a => a.GetEnumerator()).Returns(dbStudentUser.GetEnumerator());

//                var mqContext = new Mock<MySqlContext>();
//                mqContext.Setup(a => a.Articles).Returns(mockSet.Object);
//                mqContext.Setup(a => a.Comments).Returns(mockSetCom.Object);
//                mqContext.Setup(a => a.SuperUsers).Returns(mockSetSuperUsers.Object);
//                mqContext.Setup(a => a.StudentUsers).Returns(mockSetStudentUser.Object);
//                mqContext.Setup(a => a.TestStudents).Returns(mockSetTestStudent.Object);
//                mySql = mqContext.Object;

//                controller = new ArticlesController(mySql);
//            }
//            else
//            {
//                var optionsBuilder = new DbContextOptionsBuilder<MySqlContext>();
//                var options = optionsBuilder
//                    .UseNpgsql(xUnitTest.Helpers.Constants.DATABASE)
//                    .Options;
//                mySql = new MySqlContext(options);
//                mySql.Database.EnsureDeleted();
//                mySql.Database.EnsureCreated();
//                controller = new ArticlesController(mySql);
//            }
//        }

//        [Fact]
//        public async void GetAllArticler()// System.NotSupportedException
//        {
//            var result = await controller.GetAll();

//            var viewResult = Assert.IsType<ActionResult<List<Article>>>(result);
//            var articleV = Assert.IsType<OkObjectResult>(viewResult.Result);
//            var articleRes = Assert.IsAssignableFrom<IEnumerable<Article>>(articleV.Value);

//            Assert.Equal(2, articleRes.Count());

//            foreach (var article in articleRes)
//            {
//                var articlePage = await controller.Get(article.Id);
//                var articleView = Assert.IsType<ActionResult<Article>>(articlePage);
//                var articleVw = Assert.IsType<OkObjectResult>(articleView.Result);
//                var articleModel = Assert.IsAssignableFrom<Article>(articleVw.Value);

//                Assert.NotNull(articleModel);
//                Assert.NotNull(articleModel.Text);
//            }
//        }

//        [Fact]
//        public async void GetWritAndUpdateArticle()// System.NotSupportedException
//        {
//            string title = "Title";
//            string text = "All text";
//            string newText = "Update new text";
//            string newTitle = "Title new";

//            var articleP = await controller.AddArticle(title, text);
//            var articleView = Assert.IsType<ActionResult<Article>>(articleP);
//            var articleVw = Assert.IsType<OkObjectResult>(articleView.Result);
//            var article = Assert.IsAssignableFrom<Article>(articleVw.Value);

//            Assert.NotNull(article);
//            Assert.Equal(article.Title, title);
//            Assert.Equal(article.Text, text);
//            Assert.NotNull(article.UserCreate);
//            Assert.Equal(article.Comments.Count(), 0);
//            Assert.NotNull(article.DateCreate);

//            var result = await controller.GetAll();
//            // Начать отсюда 
//            var viewResult = Assert.IsType<ActionResult<List<Article>>>(result);
//            var articleVw2 = Assert.IsType<OkObjectResult>(viewResult.Result);
//            var model = Assert.IsAssignableFrom<IEnumerable<Article>>(articleVw2.Value);
//            Assert.Equal(3, model.Count());

//            foreach (var articleL in model)
//            {
//                var articlePage = await controller.Get(articleL.Id);

//                var articlePage2 = Assert.IsType<ActionResult<Article>>(articlePage);
//                var articleViewL = Assert.IsType<OkObjectResult>(articlePage2.Result);
//                var articleModel = Assert.IsAssignableFrom<Article>(articleViewL.Value);

//                Assert.NotNull(articleModel);
//                Assert.NotNull(articleModel.Text);
//                Assert.NotNull(articleModel.UserCreateId);

//                var updArticle = await controller.UpdateArticle(articleL.Id, newTitle, newText);

//                var updArticleViewL = Assert.IsType<ActionResult<Article>>(updArticle);
//                var updArticle2 = Assert.IsType<OkObjectResult>(updArticleViewL.Result);
//                var updArticleModel = Assert.IsAssignableFrom<Article>(updArticle2.Value);

//                Assert.Equal(newTitle, updArticleModel.Title);
//                Assert.Equal(newText, updArticleModel.Text);
//            }
//        }

//        [Fact]
//        public async void GetWriteComment()// System.NotSupportedException
//        {
//            var result = await controller.GetAll();

//            var viewResult = Assert.IsType<ActionResult<List<Article>>>(result);
//            var articleV = Assert.IsType<OkObjectResult>(viewResult.Result);
//            var model = Assert.IsAssignableFrom<IEnumerable<Article>>(articleV.Value);

//            Assert.Equal(2, model.Count());
//            var commentText = "Comment";

//            foreach (var article in model)
//            {
//                var commentPage = await controller.AddComment(commentText, article.Id);

//                var commentView = Assert.IsType<ActionResult<Comment>>(commentPage);
//                var commentV2 = Assert.IsType<OkObjectResult>(commentView.Result);
//                var commentModel = Assert.IsAssignableFrom<Comment>(commentV2.Value);

//                Assert.NotNull(commentModel);
//                Assert.Equal(commentModel.Text, commentText);
//                Assert.Equal(commentModel.ArticleId, article.Id);
//                Assert.NotNull(commentModel.UserCreate);
//            }
//        }

//        [Fact]
//        public async void GetClickLikeArticleAndCommentAndDelete()// System.NotSupportedException
//        {
//            var result = await controller.GetAll();

//            var viewResult = Assert.IsType<ActionResult<List<Article>>>(result);
//            var articleV = Assert.IsType<OkObjectResult>(viewResult.Result);
//            var model = Assert.IsAssignableFrom<IEnumerable<Article>>(articleV.Value);

//            Assert.Equal(2, model.Count());

//            foreach (var article in model)
//            {
//                var articlePage = await controller.ClickLike(article.Id, "article");
//                var articleView = Assert.IsType<OkObjectResult>(articlePage);
//                var articleModel = Assert.IsAssignableFrom<Article>(articleView.Value);

//                Assert.NotNull(articleModel);
//                //Assert.NotNull(articleModel.UserCreate);
//                Assert.NotNull(articleModel.LikeUsers);


//                foreach (Comment comm in article.Comments)
//                {
//                    var commentPage = await controller.ClickLike(comm.Id, "comment");
//                    var commentView = Assert.IsType<OkObjectResult>(commentPage);
//                    var commentModel = Assert.IsAssignableFrom<Comment>(commentView.Value);

//                    Assert.NotNull(commentModel);
//                    Assert.NotNull(commentModel.UserCreate);
//                    Assert.NotNull(commentModel.LikeUsers);
//                    Assert.NotEqual(commentModel.LikeUsers.Count, 0);
//                }

//                foreach (Comment comm in article.Comments)
//                {
//                    var commentPage = await controller.DeleteComment(comm.Id);

//                    //var commentView = Assert.IsType<OkObjectResult>(commentPage);
//                    //var commentModel = Assert.IsAssignableFrom<Comment>(commentView.Value);
//                    try
//                    {
//                        var commentView = Assert.IsType<ActionResult<Comment>>(commentPage);
//                        var commentV = Assert.IsType<OkObjectResult>(commentView.Result);
//                        var commentModel = Assert.IsAssignableFrom<Comment>(commentV.Value);

//                        Assert.NotNull(commentModel);
//                        Assert.NotNull(commentModel.UserCreate);
//                        Assert.NotNull(commentModel.LikeUsers);
//                        Assert.NotEqual(commentModel.LikeUsers.Count, 0);
//                    }
//                    catch (Exception e)
//                    {
//                        continue;
//                    }
//                }
//            }
//        }


//        public void Dispose()
//        {
//        }
//    }
//}
