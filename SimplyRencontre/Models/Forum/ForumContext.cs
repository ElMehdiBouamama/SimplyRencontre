using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplyRencontre.Models.Forum;

namespace SimplyRencontre.Models
{
    public class ForumContext : DbContext
    {
        public ForumContext (DbContextOptions<ForumContext> options)
            : base(options)
        {
        }

        public DbSet<Topic> Topic { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<TopicMessage> TopicMessage { get; set; }
    }
}
