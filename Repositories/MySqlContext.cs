using System;
using Microsoft.EntityFrameworkCore;
using TrainingTests.Models;

namespace TrainingTests.Repositories
{
    public class MySqlContext : DbContext// , IRepository
    {
        public MySqlContext(DbContextOptions options) : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public MySqlContext()
        {
        }

        //https://metanit.com/sharp/mvc5/5.12.php

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
               .HasOne<Article>(sc => sc.Article)
               .WithMany(s => s.Comments)
               .HasForeignKey(sc => sc.ArticleId);

            modelBuilder.Entity<Comment>()
                .HasOne<Article>(s => s.Article)
                .WithMany(g => g.Comments)
                .HasForeignKey(s => s.ArticleId)
                .HasPrincipalKey(a => a.Id);

            modelBuilder.Entity<Article>()
                .HasOne<User>(a => a.UserCreate)
                .WithOne();

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

        }

        //public DbSet<StudentUser> Students { get; set; }

        // Articles
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // Users
        public DbSet<SuperUser> SuperUsers { get; set; }
        public DbSet<TeacherUser> TeacherUsers { get; set; }
        public DbSet<StudentUser> StudentUsers { get; set; }

        // Questions
        public DbSet<TestStudent> TestStudents { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestThema> TestThemes { get; set; }
        public DbSet<Thema> Themes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionWitoutAnswer> QuestionWitoutAnswers { get; set; }
        public DbSet<Mark> Marks { get; set; }

        // Meetings
        public DbSet<EventProfileUser> EventProfileUsers { get; set; }
        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<Speaker> Speakers { get; set; }

    }
}
