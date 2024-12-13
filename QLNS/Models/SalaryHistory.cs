using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class SalaryHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Mã chấm công")]
        public string IdCheck {  get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime Date { get; set; }
        [Display(Name = "Lương cơ bản")]
        public double SalaryBase { get; set; }
        [Display(Name = "KPI")]
        public double KPI { get; set; }
        public double KPITarget { get; set; }
        [Display(Name = "Ngày nghỉ")]
        public long DateVacation { get; set; }
        public long DateVacationTarget { get; set; }
        [Display(Name = "Hệ số lương")]
        public double Coe { get; set; }
        [Display(Name = "Doanh thu")]
        public double Revenue {  get; set; }
        [Display(Name = "Thưởng")]
        public double Reward {  get; set; }
        [Display(Name = "Tổng lương")]
        public double Result { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        public virtual Salary Salary { get; set; }
        public virtual ICollection<CheckInOut> CheckInOuts { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
    }
}