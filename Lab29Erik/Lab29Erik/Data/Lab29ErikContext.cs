using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab29Erik.Models
{
    public class Lab29ErikContext : DbContext
    {
        public Lab29ErikContext (DbContextOptions<Lab29ErikContext> options)
            : base(options)
        {
        }

        public DbSet<Lab29Erik.Models.Product> Product { get; set; }
    }
}
