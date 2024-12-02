using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using party_helperbe.DataAccess.Models;
using System.Reflection.Emit;

namespace party_helperbe
{
    public class PgSQLDbContext: DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Party> Partys { get; set; }
        public DbSet<Participant> Participants { get; set; }


        public PgSQLDbContext()
        {
        }

        public PgSQLDbContext(DbContextOptions<PgSQLDbContext> options)
        : base(options)
        {
        }

       
    }
}
