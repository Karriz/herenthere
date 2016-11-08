using System;
using System.Data.Entity.Migrations;
using HereAndThere.Models;

namespace HereAndThere.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HereAndThereDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HereAndThereDbContext context)
        {
            context.matchTypes.AddOrUpdate(x => x.name,
                new MatchType
                {
                    name = "DEFAULT",
                    createdBy = "system",
                    description = "Default Match Type",
                    timeStamp = DateTime.Now
                });
            context.playerTypes.AddOrUpdate(x => x.name, new PlayerType
            {
                name = "DEFAULT",
                createdBy = "system",
                description = "Default Match Type",
                timeStamp = DateTime.Now
            });


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}