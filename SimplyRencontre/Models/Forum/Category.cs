using System.ComponentModel.DataAnnotations;

namespace SimplyRencontre.Models.Forum
{
    public class Category
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}