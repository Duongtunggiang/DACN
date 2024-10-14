using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Models
{
    public class Account_Position
    {
        [Key, Column(Order = 0)]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }

        [Key, Column(Order = 1)]
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}