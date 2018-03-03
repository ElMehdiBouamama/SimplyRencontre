using System;
using System.ComponentModel.DataAnnotations;

namespace SimplyRencontre.Models.Forum
{
    public class TopicMessage
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(10000, MinimumLength = 3, ErrorMessage = "Le topic doit contenir entre 10 et 10 000 caractéres")]
        public string Content { get; set; }

        [Timestamp]
        public DateTime? LastModified { get; set; }
        [Timestamp]
        public DateTime CreationTime { get; set; }
        public ApplicationUser Owner { get; set; }
        public Topic Topic { get; set; }

        public TopicMessage()
        {
            CreationTime = DateTime.Now;
            LastModified = null;
        }
    }
}