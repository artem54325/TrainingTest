using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TrainingTests.Models
{
    [ComplexType]
    public class Article
    {
        [Key, ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }

        public string? Image { get; set; }
        [Required]
        public int Viewer { get; set; }
        public List<string> LikeUsers { get; set; }
        public List<string> DislikeUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        [ForeignKey("UserCreateId ")]
        public User UserCreate { get; set; }
        [Required, JsonIgnore]
        public string UserCreateId { get; set; }

    }
    [ComplexType]
    public class Comment
    {
        [Key]
        public string Id { get; set; }
        [Required, JsonIgnore]
        public string UserCreateId { get; set; }
        [ForeignKey("UserCreateId ")]
        public User UserCreate { get; set; }

        public List<string> LikeUsers { get; set; }
        public List<string> DislikeUsers { get; set; }
        [Required]
        public string Text { get; set; }
        [Required, JsonIgnore]
        public string ArticleId { get; set; }
        [JsonIgnore, IgnoreDataMember]
        public Article Article { get; set; }

    }
}
