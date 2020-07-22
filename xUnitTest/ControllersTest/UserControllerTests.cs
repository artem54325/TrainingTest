using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrainingTests.Controllers;
using TrainingTests.Models;
using TrainingTests.Repositories;
using Xunit;

namespace xUnitTest.ControllersTest
{
    public class UserControllerTests
    {
        private readonly MySqlContext mySql;
        private readonly UsersController controller;

        public UserControllerTests()
        {
            if (false)
            {
                PutData putData = new PutData();

                var listQuestions = new List<Question>();//.AsQueryable();
                listQuestions.AddRange(putData.questions1);
                listQuestions.AddRange(putData.questions2);

                var dbThema = new List<Thema>(putData.themes).AsQueryable();
                var dbTestThemes = new List<TestThema>(putData.TestThemas).AsQueryable();
                var dbQuestions = listQuestions.AsQueryable();
                var dbQuestionAnswer = new List<QuestionAnswer>(putData.questionAnswers).AsQueryable();
                var dbTestStudent = new List<TestStudent>(putData.testStudents).AsQueryable();

                var mockSetThema = new Mock<DbSet<Thema>>();
                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.Provider).Returns(dbThema.Provider);
                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.Expression).Returns(dbThema.Expression);
                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.ElementType).Returns(dbThema.ElementType);
                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.GetEnumerator()).Returns(dbThema.GetEnumerator());

                var mockSetTestThema = new Mock<DbSet<TestThema>>();
                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.Provider).Returns(dbTestThemes.Provider);
                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.Expression).Returns(dbTestThemes.Expression);
                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.ElementType).Returns(dbTestThemes.ElementType);
                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.GetEnumerator()).Returns(dbTestThemes.GetEnumerator());

                var mockSetQuestion = new Mock<DbSet<Question>>();
                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.Provider).Returns(dbQuestions.Provider);
                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.Expression).Returns(dbQuestions.Expression);
                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.ElementType).Returns(dbQuestions.ElementType);
                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.GetEnumerator()).Returns(dbQuestions.GetEnumerator());

                var mockSetQuestionAnswer = new Mock<DbSet<QuestionAnswer>>();
                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.Provider).Returns(dbQuestionAnswer.Provider);
                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.Expression).Returns(dbQuestionAnswer.Expression);
                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.ElementType).Returns(dbQuestionAnswer.ElementType);
                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.GetEnumerator()).Returns(dbQuestionAnswer.GetEnumerator());

                var mockSetTestStudent = new Mock<DbSet<TestStudent>>();
                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.Provider).Returns(dbTestStudent.Provider);
                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.Expression).Returns(dbTestStudent.Expression);
                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.ElementType).Returns(dbTestStudent.ElementType);
                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.GetEnumerator()).Returns(dbTestStudent.GetEnumerator());

                var mqContext = new Mock<MySqlContext>();

                mqContext.Setup(a => a.Questions).Returns(mockSetQuestion.Object);
                mqContext.Setup(a => a.Themes).Returns(mockSetThema.Object);
                mqContext.Setup(a => a.TestThemes).Returns(mockSetTestThema.Object);
                mqContext.Setup(a => a.QuestionAnswers).Returns(mockSetQuestionAnswer.Object);
                mqContext.Setup(a => a.TestStudents).Returns(mockSetTestStudent.Object);

                mySql = mqContext.Object;
            }
            else
            {
                var optionsBuilder = new DbContextOptionsBuilder<MySqlContext>();
                var options = optionsBuilder
                    .UseNpgsql(xUnitTest.Helpers.Constants.DATABASE)
                    .Options;
                mySql = new MySqlContext(options);
                mySql.Database.EnsureDeleted();
                mySql.Database.EnsureCreated();
            }
            controller = new UsersController(mySql);
        }

        [Fact]
        public void AuthorizationTest()
        {

        }

        [Fact]
        public void SettingsTest()
        {
            //My
            //Update

        }
    }
}
