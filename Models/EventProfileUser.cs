using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrainingTests.Models
{
    public class EventProfileUser
    {
        [Key]
        public string Id { get; set; }

        [Required]
        /// <summary>  
        /// Phone user.  
        /// </summary>  
        public string Phone { get; set; }
        [Required]
        /// <summary>  
        /// Email user.  
        /// </summary>  
        public string Email { get; set; }
        [Required]
        /// <summary>  
        /// Fullname user.  
        /// </summary>  
        public string Fullname { get; set; }
        /// <summary>  
        /// Date registration user.  
        /// </summary>  
        public DateTime Registration { get; set; }
        /// <summary>  
        /// Last user actions.  
        /// </summary>  
        public DateTime LastActions { get; set; }
        [DefaultValue(true)]
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public bool Activity { get; set; }

        public string Token { get; set; }
        public string TokenHeader { get; set; }

        public string Status { get; set; }
        public string TEXT { get; set; }// After DELETE

        //public string MeetupId { get; set; }
        //public virtual Meetup Meetup { get; set; }

        //public string SpeakerId { get; set; }
        //public virtual Speaker Speaker { get; set; }
    }

    public class Meetup
    {
        [Key]
        public string Id { get; set; }
        [Required]
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public string EventProfileUserId { get; set; }
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public string PlaceWork { get; set; }
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public string PositionWork { get; set; }
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public string Group { get; set; }
    }

    public class Speaker
    {

        [Key]
        public string Id { get; set; }
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        [Required]
        public string EventProfileUserId { get; set; }

        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public string ReportTitle { get; set; }
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public string ArticleTitle { get; set; }
        /// <summary>  
        ///  is Activity user.  
        /// </summary> 
        public Performance Performance { get; set; }
    }

    public enum Performance
    {
        onlyReport,
        onlyArticle,
        all
    }
}
