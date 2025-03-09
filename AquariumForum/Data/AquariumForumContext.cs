using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AquariumForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AquariumForum.Data
{
    public class AquariumForumContext : IdentityDbContext<ApplicationUser>
    {
        public AquariumForumContext (DbContextOptions<AquariumForumContext> options)
            : base(options)
        {
        }

        public DbSet<AquariumForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<AquariumForum.Models.Comment> Comment { get; set; } = default!;
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }



    }
}
