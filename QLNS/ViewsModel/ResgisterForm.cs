using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.ViewsModel
{
    public class ResgisterForm
    {
        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "Tên")]
        public string FirstName {  get; set; }

        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "Địa chỉ email hoặc tên tài khoản")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        public string PasswordConfirm { get; set; }

    }
}