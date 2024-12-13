using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.ViewsModel
{
    public class SelectEmployee
    {
        public IEnumerable<SelectListItem> Employees { get; set; }
    }
}