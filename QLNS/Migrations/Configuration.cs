namespace QLNS.Migrations
{
    using QLNS.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QLNS.Models.QLNSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QLNS.Models.QLNSContext context)
        {
            if (!context.Positions.Any())
            {
                context.Positions.AddOrUpdate(
                    e => e.Id,
                    new Position { Name="Admin" , Description="đây là quản lý" },
                    new Position { Name = "Employee", Description = "đây là nhân viên" }

                );
            }

            if (!context.Employees.Any())
            {
                var emp = new Employee()
                {
                    FirstName = "Admin",
                    LastName = "DG",
                    StartDate = DateTime.Now,
                    Email = "admin",
                };
                context.Employees.AddOrUpdate(
                    e => e.Id,
                    emp);
                context.Accounts.AddOrUpdate(
                    a => a.Username,
                    new Account
                    {
                        Id = emp.Id,
                        Username = emp.Email,
                        Password = "123123"
                    }
                );
                
            }
            if (!context.Account_Positions.Any() && context.Employees.Any())
            {
                context.Account_Positions.AddOrUpdate(
                    e => e.AccountId,
                    new Account_Position
                    {
                        AccountId = context.Accounts.Where(e=> e.Username=="admin").FirstOrDefault().Id,
                        PositionId = context.Positions.Where(e => e.Name == "Admin").FirstOrDefault().Id,
                    }
                );
            }
            
        }
    }
}
