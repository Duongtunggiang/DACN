using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class Vacation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Mã chấm công")]
        public string IdCheck { get; set; }
        [Display(Name = "Lý do")]
        public string Reson {  get; set; }
        [Display(Name = "Mô tả")]
        public string Description {  get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime Date { get; set; }
        public virtual SalaryHistory SalaryHistory { get; set; }
    }
}