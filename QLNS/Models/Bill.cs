 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Mã nhân viên")]
        public string IdCheck { get; set; }
        [Display(Name = "Ngày xuất hóa đơn")]
        public DateTime Date { get; set; }
        [Display(Name = "Thành tiền")]
        public double Value {  get; set; }
        public virtual Employee Employee { get; set; }
    }
}