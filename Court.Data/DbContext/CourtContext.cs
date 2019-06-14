using Court.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Court.Data
{
    public class CourtContext: DbContext
    {
        public CourtContext(DbContextOptions<CourtContext> options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
