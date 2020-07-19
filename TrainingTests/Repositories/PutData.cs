using System;
using System.Collections.Generic;
using TrainingTests.Models;

namespace TrainingTests.Repositories
{
    public class PutData
    {
        public SuperUser super;
        public List<StudentUser> students;
        public List<TestStudent> testStudents;
        public StudentUser studentUser;

        public List<Question> questions1;
        public List<Question> questions2;
        public List<Question> questions3;
        public List<Thema> themes;
        public List<TestThema> TestThemas;
        public List<Mark> Marks1;
        public List<Mark> Marks2;
        public List<Test> tests;
        public TestStudent testStudent;
        public List<QuestionAnswer> questionAnswers;
        public TeacherUser teacher;

        public List<EventProfileUser> EventProfileUsers;
        public List<Meetup> Meetups;
        public List<Speaker> Speakers;

        public List<Article> articles;
        public List<Comment> comments;

        public PutData()
        {
            ef();
            db();
            art();
        }

        //public PutData()
        //{
        //    ef();
        //    db();
        //    art();

        //    //context.EventProfileUsers = EventProfileUsers;
        //    //context.Meetups = Meetups;
        //    //context.Speakers = Speakers;

        //    //context.SuperUsers = super;
        //    //context.TeacherUsers = teacher;

        //    //context.TestStudents = testStudent;
        //    //context.StudentUsers = students;
        //    //context.QuestionAnswers = questionAnswers;

        //    //context.themes = themes;
        //    //context.Questions = questions1;
        //    //context.Questions.AddRange(questions2);
        //    //context.Questions.AddRange(questions3);
        //    //context.Tests = tests;

        //    //context.Articles = (articles);
        //    //context.Comments = (comments);
        //    //Console.WriteLine("writeLine {0} {1}", context.Articles.Count, context.Comments.ToString());

        //    //modelBuilder.Entity<TeacherUser>().HasData(teacher);
        //    //modelBuilder.Entity<Thema>().HasData(themes);
        //    //modelBuilder.Entity<Question>().HasData(questions1);
        //    //modelBuilder.Entity<Question>().HasData(questions2);
        //    //modelBuilder.Entity<Question>().HasData(questions3);
        //    //modelBuilder.Entity<Test>().HasData(tests);

        //    //context.SaveChanges();
        //}

        public void ef()
        {
            EventProfileUsers = new List<EventProfileUser>()
            {
                new EventProfileUser()
                {
                    Id="id-1",
                    Phone = "Phone",
                    Email = "asd@asd.com",
                    Fullname = "Fullname1",
                    Registration = new DateTime(),
                    Activity = false
                },
                new EventProfileUser()
                {
                    Id="id-2",
                    Phone = "Phone2",
                    Fullname = "Fullname2",
                    Email = "asd2@asd.com",
                    Registration = new DateTime(),
                    Activity = false
                },
                new EventProfileUser()
                {
                    Id="id-3",
                    Phone = "Phon3e",
                    Fullname = "Fullname3",
                    Email = "as3d@asd.com",
                    Registration = new DateTime(),
                    Activity = true
                }
            };
            Meetups = new List<Meetup>()
            {
                new Meetup()
                {
                    Id="meet-1",
                    EventProfileUserId=EventProfileUsers[0].Id,
                    PlaceWork="WORK",
                    PositionWork="PositionWork",
                    Group="Group"
                },
                new Meetup()
                {
                    Id="meet-2",
                    EventProfileUserId=EventProfileUsers[1].Id,
                    PlaceWork="PlaceWork2",
                    PositionWork="PositionWork2",
                    Group="Group2"
                }
            };
            Speakers = new List<Speaker>()
            {
                new Speaker()
                {
                    Id="speacker-1",
                    ReportTitle = "Title",
                    ArticleTitle = "Article",
                    EventProfileUserId=EventProfileUsers[0].Id,
                    Performance=Performance.onlyReport
                },
                new Speaker()
                {
                    Id="speacker-2",
                    ReportTitle = "Title",
                    ArticleTitle = "Article",
                    EventProfileUserId=EventProfileUsers[2].Id,
                    Performance=Performance.onlyArticle
                }
            };

            //EventProfileUsers[0].MeetupId = Meetups[0].Id;
            //EventProfileUsers[1].MeetupId = Meetups[1].Id;

            //EventProfileUsers[0].SpeakerId = Speakers[0].Id;
            //EventProfileUsers[2].SpeakerId = Speakers[1].Id;
        }


        public void db()
        {
            super = new SuperUser()
            {
                //Id = "SuperUser",
                Password = "SuperUser",
                Username = "SuperUser",
                Firstname = "SuperUser",
                Email = "Email",
                Secondname = "SuperUser",
                DateRegistration = DateTime.Now
            };

            students = new List<StudentUser>()
            {
                new StudentUser()
            {
                Id="qw",
                Username = "UsernameStudent",
                Firstname = "FirstnameStudent",
                Secondname = "Seocndname",
                Email = "Email",
                Password = "Password",
                Group = "Group",
                DateRegistration = DateTime.Now
            },
            new StudentUser()
            {
                //Id = "qw2",
                Username = "Username2Student",
                Firstname = "Firstname2Student",
                Secondname = "Seocndname2",
                Password = "Password",
                Email = "Email",
                Group = "Group",
                DateRegistration = DateTime.Now
            }

            };

            teacher = new TeacherUser()
            {
                Id = "teacher2",
                Password = "password",
                Firstname = "Teacher2",
                Secondname = "TeacherSencName2",
                Username = "login2",
                Email = "Email",
                Department = "Пара2",
                Discipline = "Discipline",
                DateRegistration = DateTime.Now
            };
            List<string> answers = new List<string>()
                {
                    "Ответ 1", "Ответ 2", "Ответ 3", "Ответ 4", "Ответ 5"
                };

            questions1 = new List<Question>()
            {
                new Question()
            {
                Id="quest1",
                Name="Question 1",
                TypeAnswer=AnswerType.Many,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 1"
                }

            },
                new Question()
            {
                Id="quest2",
                Name="Question 2",
                TypeAnswer=AnswerType.Many,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 1","Ответ 2",
                }

            },
                new Question()
            {
                Id="quest3",
                Name="Question 1",
                TypeAnswer=AnswerType.One,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 3"
                }
            }
            };
            questions2 = new List<Question>()
            {
                new Question()
            {
                Id="quest21",
                Name="Question 1",
                TypeAnswer=AnswerType.Many,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 1"
                }

            },
                new Question()
            {
                Id="quest22",
                Name="Question 2",
                TypeAnswer=AnswerType.Many,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 1","Ответ 2",
                }

            },
                new Question()
            {
                Id="quest23",
                Name="Question 1",
                TypeAnswer=AnswerType.One,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 3"
                }
            }
            };
            questions3 = new List<Question>()
            {
                new Question()
            {
                Id="quest31",
                Name="Question 1",
                TypeAnswer=AnswerType.Many,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 1"
                }

            },
                new Question()
            {
                Id="quest32",
                Name="Question 2",
                TypeAnswer=AnswerType.Many,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 1","Ответ 2",
                }

            },
                new Question()
            {
                Id="quest33",
                Name="Question 1",
                TypeAnswer=AnswerType.One,
                Appraisal = 5,
                Description = "Текст ВОПРОСА 1",
                Answers = answers,
                CountAnswers = 5,
                RightAnswers = new List<string>()
                {
                    "Ответ 3"
                }
            }
            };

            themes = new List<Thema>()
            {
                new Thema()
                {
                    Id="thema-1",
                    Name="Thema 1",
                    Questions=questions1,
                    TeacherUserId="teacher2",
                },
                new Thema()
                {
                    Id="thema-2",
                    Name="Thema 2",
                    Questions=questions2,
                    TeacherUserId="teacher2",
                },
                new Thema()
                {
                    Id="thema-3",
                    Name="Thema 3",
                    Questions=questions3,
                    TeacherUserId="teacher2",
                }
            };

            for (int i = 0; i < questions1.Count; i++)
            {
                questions1[i].Thema = themes[0];
            }
            for (int i = 0; i < questions2.Count; i++)
            {
                questions2[i].Thema = themes[1];
            }
            for (int i = 0; i < questions3.Count; i++)
            {
                questions3[i].Thema = themes[2];
            }

            Marks1 = new List<Mark>()
                    {
                        new Mark()
                        {
                            Id="mark-1",
                            Name = "Оценка",
                            Successfully=40,
                        },
                        new Mark()
                        {
                            Id="mark-2",
                            Name = "Оценка2",
                            Successfully=60,
                        },
                        new Mark()
                        {
                            Id="mark-3",
                            Name = "Оценка3",
                            Successfully=80,
                        }
                    };
            Marks2 = new List<Mark>()
                    {
                        new Mark()
                        {
                            Id="mark-4",
                            Name = "Оценка",
                            Successfully=40,
                        },
                        new Mark()
                        {
                            Id="mark-5",
                            Name = "Оценка2",
                            Successfully=60,
                        }
                    };
            TestThemas = new List<TestThema>();
            tests = new List<Test>()
            {
                new Test()
                {
                    Id="test-1",
                    Name="test Nma",
                    Description="DEscription",
                    DateCreate=DateTime.Now,
                    DateLastChanges=DateTime.Now,
                    TimeAll=2000000,
                    Passwords = new List<string>()
                    {
                        "pas1"
                    },
                    Themes = new List<TestThema>()
                    {
                        new TestThema(){
                            TestId= "test-1",
                            Id="TestThem1",
                            Thema = themes[0],
                            ThemaId = themes[0].Id,
                            CountQuest=2,
                            CountBalls = 2
                        },
                        new TestThema(){
                            Id="TestThem2",
                            TestId= "test-1",
                            Thema = themes[1],
                            ThemaId = themes[1].Id,
                            CountQuest = 2,
                            CountBalls = 2
                        }
                    },
                    TeacherUser = teacher,

                    TestType=TestType.Relative,
                    Marks=Marks1
                },
                new Test()
                {
                    Id="test-2",
                    Name="test Nma2",
                    Description="DEscription2",
                    DateCreate=DateTime.Now,
                    DateLastChanges=DateTime.Now,
                    TimeAll=400000,
                    Passwords = new List<string>()
                    {
                        "pas2"
                    },
                    Themes = new List<TestThema>()
                    {
                        new TestThema(){
                            Id="TestThem3",
                            TestId= "test-2",
                            Thema = themes[2],
                            ThemaId = themes[2].Id,
                            CountQuest=2,
                            CountBalls = 2
                        }
                    },
                    TeacherUser = teacher,
                    TestType=TestType.Usual,
                    Marks=Marks2
                }
            };
            tests[0].Themes[0].Test = tests[0];
            tests[0].Themes[1].Test = tests[0];
            tests[1].Themes[0].Test = tests[1];
            TestThemas.AddRange(tests[0].Themes);
            TestThemas.AddRange(tests[1].Themes);

            for (int i = 0; i < Marks1.Count; i++)
            {
                Marks1[i].Test = tests[0];
            }
            for (int i = 0; i < Marks2.Count; i++)
            {
                Marks2[i].Test = tests[1];
            }

            //Ответы студента
            studentUser = students[0];
            questionAnswers = new List<QuestionAnswer>()
            {
                new QuestionAnswer()
                {
                    Id="questanswer 1",
                    Answers = new List<string>()
                    {
                        "Ответ 1", "Ответ 2"
                    },
                    DateAnswer=DateTime.Now,
                    Time = 2000,
                    Appraisal = 20.0,
                    Question = questions1[0],
                    Name = questions1[0].Name,
                    Description = questions1[0].Description,
                    QuestionAnswers = questions1[0].Answers,
                    QuestionRightAnswers = questions1[0].RightAnswers
                },
                new QuestionAnswer()
                {
                    Id="questanswer 2",
                    Answers = new List<string>()
                    {
                        "Ответ 1"
                    },
                    DateAnswer=DateTime.Now,
                    Time = 2000,
                    Appraisal = 1.0,
                    Question = questions1[1],
                    Name = questions1[1].Name,
                    Description = questions1[1].Description,
                    QuestionAnswers = questions1[1].Answers,
                    QuestionRightAnswers = questions1[1].RightAnswers
                },
                new QuestionAnswer()
                {
                    Id="questanswer 3",
                    Answers = new List<string>()
                    {
                        "Ответ 2"
                    },
                    DateAnswer=DateTime.Now,
                    Time = 2000,
                    Appraisal = 5.0,
                    Question = questions2[0],
                    Name = questions2[0].Name,
                    Description = questions2[0].Description,
                    QuestionAnswers = questions2[0].Answers,
                    QuestionRightAnswers = questions2[0].RightAnswers
                }
            };

            testStudent = new TestStudent()
            {
                Id = "test-student-1",
                DateStart = DateTime.Now,
                DateFinish = DateTime.Now,
                Mistakes = 20,
                QuestionAnswers = questionAnswers,
                StudentUser = studentUser,
                Correctly = 15,
                Test = tests[0],
                Time = 20000
            };
            testStudents = new List<TestStudent>();
            testStudents.Add(testStudent);

            for (int i = 0; i < questionAnswers.Count; i++)
            {
                questionAnswers[i].TestStudent = testStudent;
            }
        }

        public void art()
        {
            comments = new List<Comment>();
            articles = new List<Article>();

            Article article = new Article();
            article.Id = "article-1";
            article.Title = "Name article";
            article.Text = "Text qweqwqewqe";
            article.Viewer = 0;
            article.UserCreate = studentUser;
            article.UserCreateId = studentUser.Id;
            article.DateCreate = DateTime.Now;
            article.DateUpdate = DateTime.Now;
            List<Comment> coms = new List<Comment>();
            coms.Add(new Comment()
            {
                Id = "comment-id-1",
                //Article = article,
                ArticleId = article.Id,
                Text = "qewkdf",
                UserCreate = studentUser,
                UserCreateId = studentUser.Id,
                DateCreate = DateTime.Now
            });
            article.Comments = coms;
            article.Image = "https://www.renewhr.com/wp-content/uploads/2019/12/slider-renew.jpg";
            articles.Add(article);
            comments.AddRange(coms);

            article = new Article();
            article.Id = "article-2";
            article.Title = "Name 2";
            article.Text = "Text 2";
            article.Viewer = 100;
            article.Image = "https://www.renewhr.com/wp-content/uploads/2019/12/slider-renew.jpg";
            article.UserCreate = studentUser;
            article.UserCreateId = studentUser.Id;
            article.DateCreate = DateTime.Now;
            article.DateUpdate = DateTime.Now;
            coms = new List<Comment>();
            coms.Add(new Comment()
            {
                Id = "comment-id-2",
                //Article = article,
                ArticleId = article.Id,
                Text = "Example comments",
                UserCreate = studentUser,
                UserCreateId = studentUser.Id,
                DateCreate = DateTime.Now
            });
            coms.Add(new Comment()
            {
                Id = "comment-id-3",
                //Article = article,
                ArticleId = article.Id,
                Text = "Example comments",
                UserCreate = teacher,
                UserCreateId = teacher.Id,
                DateCreate = DateTime.Now
            });

            article.Comments = coms;
            comments.AddRange(coms);
            articles.Add(article);
        }
    }

}
