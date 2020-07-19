using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TrainingTests.Models;

namespace TrainingTests.Helpers
{
    public class ThemaConverter
    {
        public static List<Thema> JsonToThemas(JArray array)
        {
            List<Thema> List = new List<Thema>();

            Thema thema;
            List<Question> listQuest;
            Question objQuest;

            foreach (JObject jObject in array)
            {
                thema = new Thema();

                thema.Id = jObject["id"].Value<string>();
                thema.Name = jObject["name"].Value<string>();
                thema.Description = jObject["description"].Value<string>();

                JArray questions = jObject["questions"].Value<JArray>();
                listQuest = new List<Question>();

                foreach (JObject question in questions)
                {
                    objQuest = new Question();

                    objQuest.Id = question["id"].Value<string>();
                    objQuest.Name = question["name"].Value<string>();
                    objQuest.Appraisal = question["balls"].Value<int>();
                    objQuest.Description = question["question"].Value<string>();

                    List<string> all = new List<string>();
                    List<string> right = new List<string>();
                    JArray answers = question["answers"].Value<JArray>();
                    foreach (JObject answer in answers)
                    {
                        all.Add(answer["text"].Value<string>());
                        if (answer["right"].Value<bool>())
                        {
                            right.Add(answer["text"].Value<string>());
                        }
                    }
                    objQuest.Answers = all;
                    objQuest.RightAnswers = right;

                    listQuest.Add(objQuest);
                }
                thema.Questions = listQuest;

                List.Add(thema);
            }
            return List;
        }

        public static JArray ThemasToJson(List<Thema> List)
        {
            JArray array = new JArray();

            foreach (Thema thema in List)
            {
                JObject jThema = new JObject();

                jThema.Add("id", thema.Id);
                jThema.Add("name", thema.Name);
                jThema.Add("description", thema.Description);
                JArray array1 = new JArray();
                foreach (Question question in thema.Questions)
                {
                    JObject jQuest = new JObject();

                    jQuest.Add("id", question.Id);
                    jQuest.Add("name", question.Name);
                    jQuest.Add("balls", question.Appraisal);
                    jQuest.Add("question", question.Description);

                    JArray array2 = new JArray();
                    foreach (string answer in question.Answers)
                    {
                        JObject jAnswer = new JObject();

                        jAnswer.Add("text", answer);
                        jAnswer.Add("right", question.RightAnswers.Contains(answer));

                        array2.Add(jAnswer);
                    }
                    jQuest.Add("answers", array2);
                    array1.Add(jQuest);
                }
                jThema.Add("questions", array1);
                array.Add(jThema);
            }

            return array;
        }
    }

}
