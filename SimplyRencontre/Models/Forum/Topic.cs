using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyRencontre.Models.Forum
{
    public class Topic
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(60,MinimumLength = 5, ErrorMessage = "Le titre doit contenir entre 5 et 60 caractéres")]
        public string Title { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        [StringLength(10000, MinimumLength = 3, ErrorMessage = "Le topic doit contenir entre 10 et 10 000 caractéres")]
        public string Content { get; set; }

        public List<TopicMessages> Messages { get; set; }

        public ApplicationUser Owner { get; set; }
        public DateTime CreationDate { get; set; }

        public Topic()
        {
            CreationDate = DateTime.Now;
        }
    }
}
