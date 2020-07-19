using System;
using Microsoft.EntityFrameworkCore;
using TrainingTests.Models;

namespace TrainingTests.Repositories
{
    public class MySqlContext : DbContext// , IRepository
    {
        public MySqlContext(DbContextOptions options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        //https://metanit.com/sharp/mvc5/5.12.php

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region
            //modelBuilder.Entity<Comment>()
            //   .HasOne<Article>(sc => sc.Article)
            //   .WithMany(s => s.Comments)
            //   .HasForeignKey(sc => sc.ArticleId);

            //modelBuilder.Entity<Comment>()
            //    .HasOne<Article>(s => s.Article)
            //    .WithMany(g => g.Comments)
            //    .HasForeignKey(s => s.ArticleId)
            //    .HasPrincipalKey(a => a.Id);

            //modelBuilder.Entity<Article>()
            //    .HasOne<User>(a => a.UserCreate)
            //    .WithOne();

            //modelBuilder.Entity<StudentCourse>()
            //    .HasKey(t => new { t.StudentId, t.CourseId });

            //modelBuilder.Entity<Article>()
            //    .HasOne(sc => sc.)
            //    .WithMany(s => s.StudentCourses)
            //    .HasForeignKey(sc => sc.StudentId);

            //modelBuilder.Entity<StudentCourse>()
            //    .HasOne(sc => sc.Course)
            //    .WithMany(c => c.StudentCourses)
            //    .HasForeignKey(sc => sc.CourseId);
            #endregion

        }

        // Articles
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        // Users
        public virtual DbSet<SuperUser> SuperUsers { get; set; }
        public virtual DbSet<TeacherUser> TeacherUsers { get; set; }
        public virtual DbSet<StudentUser> StudentUsers { get; set; }

        // Questions
        public virtual DbSet<TestStudent> TestStudents { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<TestThema> TestThemes { get; set; }
        public virtual DbSet<Thema> Themes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionWitoutAnswer> QuestionWitoutAnswers { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }

        // Meetings
        public virtual DbSet<EventProfileUser> EventProfileUsers { get; set; }
        public virtual DbSet<Meetup> Meetups { get; set; }
        public virtual DbSet<Speaker> Speakers { get; set; }

    }
}
