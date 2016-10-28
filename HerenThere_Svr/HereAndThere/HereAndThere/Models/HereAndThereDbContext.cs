using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace HereAndThere.Models
{
   
    public class HereAndThereDbContext : DbContext
    {

        public  DbSet<Player> players { get; set; }
        public DbSet<PlayerType> playerTypes { get; set; }
        public DbSet<Match> matches { get; set; }
        public DbSet<MatchType> matchTypes { get; set; }
        public DbSet<Score> scores { get; set; }
        public DbSet<Boundary> boundaries { get; set; }
        public DbSet<Movement> movements { get; set; }
        public DbSet<Location> locations { get; set; }
        public HereAndThereDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public HereAndThereDbContext():base("DefaultDbConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}