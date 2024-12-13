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
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [Display(Name = "Tuổi")]
        public int Age { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }
        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        [Display(Name = "Ngày vào làm")]
        public DateTime StartDate { get; set; }
        public string Email { get; set; }
        [Display(Name = "Hệ số lương")]
        public double Coe { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Mã chấm công")]
        public string IdCheck {  get; set; }
        [Display(Name = "Mã QR")]
        public string QRCode {  get; set; }
        [Display(Name = "BHYT")]
        public string BHYT { get; set; }
        [Display(Name = "CCCD")]
        public string CCCD {  get; set; }
        public virtual ICollection<Salary> Salary { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public int AccountId { get; set; }  // Foreign Key
    }
}