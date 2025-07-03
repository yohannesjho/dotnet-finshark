using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Make sure this is included
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Only if your Stock and Comment classes are in this namespace

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<Comment> Comment { get; set; }
    }
}
