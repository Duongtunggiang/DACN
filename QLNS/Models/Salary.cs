using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string NameEmployee { get; set; }
        [Display(Name = "Lương cơ bản")]
        public double SalaryBase { get; set; }
        [Display(Name = "KPI")]
        public double KPI { get; set; }
        [Display(Name = "Số ngày nghỉ phép")]
        public long DateVacation {  get; set; }
        [Display(Name = "Mã chấm công")]
        public string IdCheck { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<SalaryHistory> SalaryHistories { get; set;}
    }
}