using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTests.Models;

namespace TrainingTests.Helpers
{
    public class CreateQuestionsStudent
    {
        public static List<Question> TestToQuestions(Test test)
        {
            List<Question> questions = new List<Question>();
            int rand;
            Console.WriteLine("themes " + test.Themes.Count());
            for (int i = 0; i < test.Themes.Count; i++)
            {
                for (int a = 0; a < test.Themes[i].CountQuest; a++)// test.Themes[i].Thema.Question
                {
                    if (test.Themes[i].Thema.Questions.Count > 0)
                    {
                        rand = new Random().Next(test.Themes[i].Thema.Questions.Count);
                        var quest = test.Themes[i].Thema.Questions[rand];
                        test.Themes[i].Thema.Questions.Remove(quest);
                        quest.RightAnswers = null;
                        quest.Thema = null;
                        questions.Add(quest);
                    }
                }
            }
            return questions;
        }

        public static TestStudent CreateTest(Test test)
        {
            TestStudent testUser = new TestStudent()
            {
                Test = test,
                DateStart = DateTime.Now
            };

            List<QuestionAnswer> questions = new List<QuestionAnswer>();

            foreach (TestThema thema in test.Themes)
            {
                for (var i = 0; i < 3; i++)// thema.Thema.Question
                {
                    var question = thema.Thema.Questions[new Random().Next(thema.Thema.Questions.Count)];
                    thema.Thema.Questions.Remove(question);
                    questions.Add(new QuestionAnswer()
                    {
                        Question = question,
                        TestStudent = testUser,
                        Name = question.Name,
                        Description = question.Description,
                        QuestionRightAnswers = question.Answers,
                        Appraisal = question.Appraisal
                    });
                }
            }
            testUser.QuestionAnswers = questions;

            return testUser;
        }

        public static TestStudent ResultTest(TestStudent testStudent)
        {
            int correctlyAll = 0;
            int mistakeAll = 0;
            int correctlyQuestions = 0;

            double balls = 0;
            double ballsAll = 0;

            for (int i = 0; i < testStudent.QuestionAnswers.Count; i++)
            {
                int correctly = 0;
                int mistake = 0;
                ballsAll += testStudent.QuestionAnswers[i].Appraisal;

                for (int a = 0; a < testStudent.QuestionAnswers[i].QuestionRightAnswers.Count; a++)
                {
                    if (testStudent.QuestionAnswers[i].Answers.SequenceEqual(testStudent.QuestionAnswers[i].QuestionRightAnswers))
                    {
                        correctly++;
                    }
                    else
                    {
                        mistake++;
                    }
                }

                if (mistake == 0)
                {
                    testStudent.QuestionAnswers[i].Status = true;
                    correctlyQuestions++;
                    balls += testStudent.QuestionAnswers[i].Appraisal;
                    correctlyAll += correctly;
                }
                else
                {
                    testStudent.QuestionAnswers[i].Status = false;
                    mistakeAll++;
                }
                Console.WriteLine($"mistake {mistake}");
                Console.WriteLine($"Appraisal {balls}, {testStudent.QuestionAnswers[i].Appraisal}");
                // mistakeAll += mistake;
            }
            testStudent.Appraisal = balls;
            testStudent.Correctly = correctlyAll;
            testStudent.Mistakes = mistakeAll;
            testStudent.CorrectlyQuestions = correctlyQuestions;

            // Double appraisal = 0;
            //if (testStudent.Test.TestType == TestType.Relative)
            //{
            //    if (correctlyAll != 0)
            //    {
            //        appraisal = (correctlyAll - mistakeAll) * 100 / correctlyAll;
            //    }
            //}
            //else if (testStudent.Test.TestType == TestType.Usual)
            //{
            //    if (ballsAll == 0)
            //    {
            //        appraisal = 0;
            //    }
            //    else
            //    {
            //        appraisal = balls * 100 / ballsAll;
            //    }
            //}
            // testStudent.Appraisal = appraisal;

            //Mark markNow = null;
            //var marks = testStudent.Test.Marks.OrderBy(a => a.Successfully);
            //foreach (Mark mark in marks)
            //{
            //    if (testStudent.Appraisal >= mark.Successfully)
            //    {
            //        markNow = mark;
            //    }
            //}
            //testStudent.Mark = markNow;

            return testStudent;
        }
    }
}
