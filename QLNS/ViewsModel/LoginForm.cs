using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.ViewsModel
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}