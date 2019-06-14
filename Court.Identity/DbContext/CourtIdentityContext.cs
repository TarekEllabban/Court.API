using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Court.Data
{
    public class CourtIdentityContext: IdentityDbContext<IdentityUser>
    {
        public CourtIdentityContext(DbContextOptions<CourtIdentityContext> options): base(options)
        {

        }
    }
}
