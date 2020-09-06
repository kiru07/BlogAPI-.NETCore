using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DatabaseContext
{
    // Used to manage DB operations.
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) :base(option)
        {

        }
        
        // Represent the tables in MSSQLServer DB.
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
