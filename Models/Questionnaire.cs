using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TrainingTests.Helpers;

namespace TrainingTests.Models
{
    public class TestStudent
    {
        [Key, Required]
        public string Id { get; set; }

        public string TestId { get; set; }

        [Required]//Сделать чтобы в JSON не записывался
        public Test Test { get; set; }

        public string StudentUserId { get; set; }
        public StudentUser StudentUser { get; set; }

        public string Username { get; set; }// Имя пользователя,
        public string Group { get; set; }// группа,студент
        public string Departament { get; set; }// Имя пользователя, 

        public List<QuestionAnswer> QuestionAnswers { get; set; }

        public int Mistakes { get; set; }//Кол-во не правильно выбранных ответов
        public int Correctly { get; set; }//Кол-во правильно выбранных ответов

        public int CorrectlyQuestions { get; set; }//Правильные ответы полные

        public double Appraisal { get; set; }//  Кол-вл баллов     

        public DateTime DateStart { get; set; } //Начало тестирование
        public DateTime DateFinish { get; set; } // Окончание ответа
        public long Time { get; set; } // Время прохождения вопроса
        public string IpAddress { get; set; }

        public string MarkId { get; set; }
        public Mark Mark { get; set; }
    }

    public class QuestionAnswer
    {
        [Key, Required]
        public string Id { get; set; }

        public string TestStudentId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public TestStudent TestStudent { get; set; }
        //Сделал, чтобы в JSON не записывался
        public string QuestionId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Question Question { get; set; }

        public long Time { get; set; } //Время прохождения на ответ данного вопроса
        public double Appraisal { get; set; }//Баллы для вопроса  
        public DateTime DateAnswer { get; set; }//Время начала ответа

        public string Description { get; set; }//Вопрос
        public string Name { get; set; }//Название вопроса

        public int Successfully { get; set; }// Кол-во правильных вопросов
        public int Error { get; set; }// Кол-во неправильных вопросов

        public bool Status { get; set; }//Состояния Ответа

        [NotMapped]
        public List<string> QuestionAnswers { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string JsonQuestionAnswers
        {
            get { return TypeConverters.ListToString(QuestionAnswers); }
            set { QuestionAnswers = TypeConverters.StringToList(value); }
        }

        [NotMapped]
        public List<string> QuestionRightAnswers { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string JsonQuestionRightAnswers
        {
            get { return TypeConverters.ListToString(QuestionRightAnswers); }
            set { QuestionRightAnswers = TypeConverters.StringToList(value); }
        }

        [NotMapped]
        public List<string> Answers { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string JsonAnswers
        {
            get { return TypeConverters.ListToString(Answers); }
            set { Answers = TypeConverters.StringToList(value); }
        }
    }
    public class TestThema
    {
        [Key, Required]
        public string Id { get; set; }

        public Test Test { get; set; }
        public string TestId { get; set; }

        public Thema Thema { get; set; }
        public string ThemaId { get; set; }
        public int CountQuest { get; set; }

        //public long TimeQuest { get; set; }
        public int CountBalls { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string TeacherUserId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public TeacherUser TeacherUser { get; set; }
        public String ToString()
        {
            return $" {Id} {TestId} {ThemaId} {CountQuest} {CountBalls} {TeacherUserId}";
        }
    }

    public class Test
    {
        [Key, Required]
        public string Id { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public List<TestStudent> TestStudents { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string TeacherUserId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public TeacherUser TeacherUser { get; set; }

        [NotMapped]
        public List<string> Passwords { get; set; }// Пароль на данный тест
        [JsonIgnore]
        [IgnoreDataMember]
        public string JsonPasswords
        {
            get { return TypeConverters.ListToString(Passwords); }
            set { Passwords = TypeConverters.StringToList(value); }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public long TimeAll { get; set; }//Время выполнение теста

        public List<Mark> Marks { get; set; }

        public List<TestThema> Themes { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DateLastChanges { get; set; }//Последние изменение

        public TestType TestType { get; set; }//Способ расчета ответов
    }

    public class Thema
    {
        [Key, Required]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public string TeacherUserId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public TeacherUser TeacherUser { get; set; }

        public List<Question> Questions { get; set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        //public string TestId { get; set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        //public Test Test { get; set; }
    }

    public class Question
    {
        [Key, Required]
        public string Id { get; set; }

        public string ThemaId { get; set; }

        public Thema Thema { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public List<string> Answers { get; set; }
        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public string JsonAnswer
        {
            get { return TypeConverters.ListToString(Answers); }
            set { Answers = TypeConverters.StringToList(value); }
        }

        public List<string> RightAnswers { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string JsonRightAnswers
        {
            get { return TypeConverters.ListToString(RightAnswers); }
            set { RightAnswers = TypeConverters.StringToList(value); }
        }
        public double Appraisal { get; set; }//Баллы для всего вопроса

        public int CountAnswers { get; set; }//Кол-во вопросов в теме
        public long Time { get; set; }

        public bool AllAnswers { get; set; }//Все ли ответы засчитывать или только один
        public AnswerType TypeAnswer { get; set; }
    }

    public class QuestionWitoutAnswer
    {
        public QuestionWitoutAnswer() { }
        public QuestionWitoutAnswer(Question question)
        {
            Id = question.Id;
            Name = question.Name;
            Description = question.Description;
            Answers = question.Answers;
            TypeAnswer = question.TypeAnswer;
        }

        [Key, Required]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public List<string> Answers { get; set; }
        
        [JsonIgnore, IgnoreDataMember, NotMapped]
        public string JsonAnswer
        {
            get { return TypeConverters.ListToString(Answers); }
            set { Answers = TypeConverters.StringToList(value); }
        }

        public AnswerType TypeAnswer { get; set; }
    }

    public class Mark
    {//Подумать над реализацией ЭТОГО!!! Либо сделать реализать изменения в другом месте
        [Key, Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Successfully { get; set; }//Минимальное кол-во баллов для оценки и название самой оценки

        [JsonIgnore, IgnoreDataMember]
        public string TestId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Test Test { get; set; }
    }

    public enum AnswerType
    {
        All,//Все ответы, если хоть один из них не верен, то ответ будет не верный
        One,//Баллы за весь вопрос дадут если хоть один из правильный
        Many//Баллы будут разделены за между ответами пользователя
    }

    public enum QuestionType
    {
        Many,// Если несколько ответов - Checkbox
        One  // Если только один ответ - Radio
    }
    public enum TestType
    {
        Usual,//Обычный расчета
        Relative//Способ расчета относительный который выбрал Жабко!
    }
}
