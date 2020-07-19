//using ClosedXML;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TrainingTests.Controllers;
//using TrainingTests.Models;
//using TrainingTests.Repositories;
//using Xunit;

//namespace xUnitTest.ControllersTest
//{
//    public class CreateTestControllerTests : IDisposable
//    {
//        private readonly MySqlContext mySql;
//        private readonly CreateTestController controller;

//        public CreateTestControllerTests()
//        {
//            if (false)
//            {
//                PutData putData = new PutData();

//                var listQuestions = new List<Question>();//.AsQueryable();
//                listQuestions.AddRange(putData.questions1);
//                listQuestions.AddRange(putData.questions2);

//                var dbThema = new List<Thema>(putData.themes).AsQueryable();
//                var dbTestThemes = new List<TestThema>(putData.TestThemas).AsQueryable();
//                var dbQuestions = listQuestions.AsQueryable();
//                var dbQuestionAnswer = new List<QuestionAnswer>(putData.questionAnswers).AsQueryable();
//                var dbTestStudent = new List<TestStudent>(putData.testStudents).AsQueryable();

//                var mockSetThema = new Mock<DbSet<Thema>>();
//                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.Provider).Returns(dbThema.Provider);
//                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.Expression).Returns(dbThema.Expression);
//                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.ElementType).Returns(dbThema.ElementType);
//                mockSetThema.As<IQueryable<Thema>>().Setup(a => a.GetEnumerator()).Returns(dbThema.GetEnumerator());

//                var mockSetTestThema = new Mock<DbSet<TestThema>>();
//                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.Provider).Returns(dbTestThemes.Provider);
//                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.Expression).Returns(dbTestThemes.Expression);
//                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.ElementType).Returns(dbTestThemes.ElementType);
//                mockSetTestThema.As<IQueryable<TestThema>>().Setup(a => a.GetEnumerator()).Returns(dbTestThemes.GetEnumerator());

//                var mockSetQuestion = new Mock<DbSet<Question>>();
//                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.Provider).Returns(dbQuestions.Provider);
//                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.Expression).Returns(dbQuestions.Expression);
//                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.ElementType).Returns(dbQuestions.ElementType);
//                mockSetQuestion.As<IQueryable<Question>>().Setup(a => a.GetEnumerator()).Returns(dbQuestions.GetEnumerator());

//                var mockSetQuestionAnswer = new Mock<DbSet<QuestionAnswer>>();
//                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.Provider).Returns(dbQuestionAnswer.Provider);
//                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.Expression).Returns(dbQuestionAnswer.Expression);
//                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.ElementType).Returns(dbQuestionAnswer.ElementType);
//                mockSetQuestionAnswer.As<IQueryable<QuestionAnswer>>().Setup(a => a.GetEnumerator()).Returns(dbQuestionAnswer.GetEnumerator());

//                var mockSetTestStudent = new Mock<DbSet<TestStudent>>();
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.Provider).Returns(dbTestStudent.Provider);
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.Expression).Returns(dbTestStudent.Expression);
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.ElementType).Returns(dbTestStudent.ElementType);
//                mockSetTestStudent.As<IQueryable<TestStudent>>().Setup(a => a.GetEnumerator()).Returns(dbTestStudent.GetEnumerator());

//                var mqContext = new Mock<MySqlContext>();

//                mqContext.Setup(a => a.Questions).Returns(mockSetQuestion.Object);
//                mqContext.Setup(a => a.Themes).Returns(mockSetThema.Object);
//                mqContext.Setup(a => a.TestThemes).Returns(mockSetTestThema.Object);
//                mqContext.Setup(a => a.QuestionAnswers).Returns(mockSetQuestionAnswer.Object);
//                mqContext.Setup(a => a.TestStudents).Returns(mockSetTestStudent.Object);

//                mySql = mqContext.Object;
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
//                controller = new CreateTestController(mySql);
//            }
//        }

//        [Fact]
//        public async void GetFullThemesTest()
//        {
//            var stringName = "Name";
//            var stringDescr = "DescAns";
//            var intTimeQuest = 15000;
//            var intCountBalls = 5;
//            var result = await controller.GetAllTest();

//            var viewResult = Assert.IsType<ActionResult<List<Test>>>(result);
//            var testV = Assert.IsType<OkObjectResult>(viewResult.Result);
//            var testRes = Assert.IsAssignableFrom<IEnumerable<Test>>(testV.Value).ToList();

//            Assert.Equal(2, testRes.Count());
//            List<TestThema> testThemes = new List<TestThema>();
//            foreach (var test in testRes)
//            {
//                test.Name = stringName;
//                test.Description = stringDescr;
//                if (test.Themes != null)
//                {
//                    testThemes = test.Themes;
//                }
//                foreach (var thema in test.Themes)
//                {
//                    thema.TimeQuest = intTimeQuest;
//                    thema.CountBalls = intCountBalls;
//                }
//            }
//            Test newTest = new Test
//            {
//                Id = Guid.NewGuid().ToString(),
//                Name = stringName,
//                Description = stringDescr,
//                Themes = testThemes,
//                TimeAll = intTimeQuest,
//                Passwords = new List<string>
//                {
//                    "newStringPassword"
//                }
//            };

//            testRes.Add(newTest);
//            var resultSave = await controller.SaveAllTests(testRes);

//            var viewUpdResult = Assert.IsType<ActionResult<List<Test>>>(resultSave);
//            var testsUpdV = Assert.IsType<OkObjectResult>(viewUpdResult.Result);
//            var testsUpdRes = Assert.IsAssignableFrom<IEnumerable<Test>>(testsUpdV.Value);

//            Assert.Equal(3, testsUpdRes.Count());
//            foreach (var test in testsUpdRes)
//            {
//                Assert.Equal(test.Name, stringName);
//                Assert.Equal(test.Description, stringDescr);
//                Assert.NotNull(test.Themes);
//                Assert.NotEqual(test.Themes.Count(), 0);

//                foreach (var thema in test.Themes)
//                {
//                    Assert.Equal(thema.TimeQuest, intTimeQuest);
//                    Assert.Equal(thema.CountBalls, intCountBalls);
//                }
//            }
//        }

//        [Fact]
//        public async void GetFullTestTest()
//        {
//            var stringName = "Name";
//            var stringDescr = "DescAns";
//            var stringQuestDescr = "descQuesto";
//            var DoubleQuestAppr = 2.4; ;
//            var BooleanQuestAllAnsw = false;
//            var result = await controller.GetAllThemes();

//            var viewResult = Assert.IsType<ActionResult<List<Thema>>>(result);
//            var themesV = Assert.IsType<OkObjectResult>(viewResult.Result);
//            var themesRes = Assert.IsAssignableFrom<IEnumerable<Thema>>(themesV.Value).ToList();

//            Assert.Equal(3, themesRes.Count());
//            foreach (var thema in themesRes)
//            {
//                thema.Name = stringName;
//                thema.Description = stringDescr;
//                foreach (var question in thema.Questions)
//                {
//                    question.Description = stringQuestDescr;
//                    question.Appraisal = DoubleQuestAppr;
//                    question.AllAnswers = BooleanQuestAllAnsw;
//                }
//            }
//            Thema newThema = new Thema
//            {
//                Id = Guid.NewGuid().ToString(),
//                Name = stringName,
//                Description = stringDescr,
//                Questions = new List<Question>
//                {
//                    new Question
//                    {
//                        Id=Guid.NewGuid().ToString(),
//                        Description = stringQuestDescr,
//                        Appraisal = DoubleQuestAppr,
//                        AllAnswers = BooleanQuestAllAnsw,
//                    },
//                    new Question
//                    {
//                        Id=Guid.NewGuid().ToString(),
//                        Description = stringQuestDescr,
//                        Appraisal = DoubleQuestAppr,
//                        AllAnswers = BooleanQuestAllAnsw,
//                    },
//                }
//            };
//            themesRes.Add(newThema);
//            var resultSave = await controller.SaveAllThemes(themesRes);

//            var viewUpdResult = Assert.IsType<ActionResult<List<Thema>>>(resultSave);
//            var themesUpdV = Assert.IsType<OkObjectResult>(viewUpdResult.Result);
//            var themesUpdRes = Assert.IsAssignableFrom<IEnumerable<Thema>>(themesUpdV.Value);

//            Assert.Equal(4, themesUpdRes.Count());
//            foreach (var thema in themesRes)
//            {
//                Assert.Equal(thema.Name, stringName);
//                Assert.Equal(thema.Description, stringDescr);
//                Assert.NotNull(thema.Questions);

//                foreach (var question in thema.Questions)
//                {
//                    Assert.Equal(question.Description, stringQuestDescr);
//                    Assert.Equal(question.Appraisal, DoubleQuestAppr);
//                    Assert.Equal(question.AllAnswers, BooleanQuestAllAnsw);
//                }
//            }
//        }


//        public async void Dispose()
//        {
//            //throw new NotImplementedException();
//        }
//    }
//}
