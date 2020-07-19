using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TrainingTests.Models
{

    public class Article
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [JsonIgnore]
        public string UserCreateId { get; set; }
        [ForeignKey("UserCreateId ")]
        public virtual User UserCreate { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public string? Image { get; set; }
        public int Viewer { get; set; }
        public List<string> LikeUsers { get; set; }
        public List<string> DislikeUsers { get; set; }
        public DateTime DateCreate { get; set; }// Дата создания
        public DateTime DateUpdate { get; set; }// Дата последнего обновления
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [JsonIgnore]
        public string UserCreateId { get; set; }
        [ForeignKey("UserCreateId ")]
        public virtual User UserCreate { get; set; }

        public List<string> LikeUsers { get; set; }
        public List<string> DislikeUsers { get; set; }
        //[Required]
        public string Text { get; set; }
        //[Required]
        public DateTime DateCreate { get; set; }
        [JsonIgnore]
        public string ArticleId { get; set; }

    }
}
