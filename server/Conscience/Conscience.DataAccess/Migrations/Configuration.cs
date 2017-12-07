namespace Conscience.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Conscience.DataAccess.ConscienceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Conscience.DataAccess.ConscienceContext";
        }

        protected override void Seed(Conscience.DataAccess.ConscienceContext context)
        {
        }
    }
}
