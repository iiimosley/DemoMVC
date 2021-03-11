namespace DemoMVC.Migrations
{
    using Bogus;
    using DemoMVC.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<DemoMVC.DataAccess.RPDContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DemoMVC.DataAccess.RPDContext context)
        {
            var faker = new Faker("en");
            var providerSuffix = new List<string>() { "Clinic", "Hospital", "Family Practice" };

            var providers = new List<Provider>()
            {
                new Provider()
                {
                    Name = "Lavergne Clinic",
                    Address = "123 Paper Street",
                    City = "Lavergne",
                    State = "TN"
                },
                new Provider()
                {
                    Name = "Bellevue Clinic",
                    Address = "123 Bell Street",
                    City = "Bellevue",
                    State = "TN"
                },
                new Provider()
                {
                    Name = "Madison Clinic",
                    Address = "123 Galliton Pike",
                    City = "Madison",
                    State = "TN"
                }
            };
            
            var providerFaker = new Faker<Provider>()
                .RuleFor(p => p.Address, f => f.Address.StreetAddress())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.State, f => f.Address.StateAbbr())
                .RuleFor(p => p.Name, (f, p) => $"{p.City} {f.PickRandom("Clinic", "Hospital", "Family Practice")}");

            
            providers.AddRange(providerFaker.Generate(100));
            context.Provider.AddOrUpdate(p => p.Name, providers.ToArray());

            SeedMembership();
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Profiles", "ID", "UserName", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
                roles.CreateRole("Admin");

            if (membership.GetUser("sa", false) == null)
                membership.CreateUserAndAccount("sa", "123456");

            if (!roles.GetRolesForUser("sa").Contains("Admin"))
                roles.AddUsersToRoles(new[] { "sa" }, new[] { "Admin" });
        }
    }
}
