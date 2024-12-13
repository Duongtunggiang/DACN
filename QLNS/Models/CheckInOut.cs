using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class CheckInOut
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int Id { get; set; }
        [Display(Name = "Mã chấm công")]
        public string IdCheck {  get; set; }
        [Display(Name = "Họ tên")]
        public string Name {  get; set; }
        [Display(Name = "Thời gian check-in")]
        public DateTime CheckInTime {  get; set; }
        [Display(Name = "Thời gian check-out")]
        public DateTime CheckOutTime { get; set; }
        [Display(Name = "Hệ số công")]
        public double Coe {  get; set; }
        public virtual SalaryHistory SalaryHistory { get; set; }
    }
}