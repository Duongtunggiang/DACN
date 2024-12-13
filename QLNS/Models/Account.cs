using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Account_Position> Account_Positions { get; set; }
    }
}