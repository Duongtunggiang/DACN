using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class QLNSContext:DbContext
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Account_Position> Account_Positions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Thiết lập quan hệ 1-1 giữa Employee và Salary
            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.Salary)
                .WithRequired(s => s.Employee);

            // Thiết lập quan hệ 1-1 giữa Employee và Account
            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.Account)
                .WithRequired(a => a.Employee);

            // Thiết lập quan hệ 1-n giữa Account_Position và Position
            modelBuilder.Entity<Account_Position>()
                .HasRequired(ap => ap.Position)
                .WithMany(p => p.Account_Positions)
                .HasForeignKey(ap => ap.PositionId);

            // Thiết lập quan hệ 1-n giữa Account_Position và Account
            modelBuilder.Entity<Account_Position>()
                .HasRequired(ap => ap.Account)
                .WithMany(a => a.Account_Positions)
                .HasForeignKey(ap => ap.AccountId);

            base.OnModelCreating(modelBuilder);
        }
    }
}