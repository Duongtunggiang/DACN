using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        public DateTime StartDate { get; set; }
        public string Email { get; set; }
        public double Coe { get; set; }
        public string Description { get; set; }
        public virtual Salary Salary { get; set; }
        public virtual Account Account { get; set; }
        public int AccountId { get; set; }  // Foreign Key
    }
}