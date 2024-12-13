using QLNS.App_Start;
using QLNS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Controllers
{
    

    public class HomeController : Controller
    {
        private QLNSContext db = new QLNSContext();

        public ActionResult Index()
        {
            var id = Session["idcheck"].ToString();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var currentDay = DateTime.Now.Day;
            var cin = db.CheckInOuts.Where(c => c.CheckInTime.Day == currentDay && c.CheckInTime.Month == currentMonth
                                           && c.CheckInTime.Year == currentYear && c.IdCheck==id).Count();
            var cout = db.CheckInOuts.Where(c => c.CheckOutTime.Day == currentDay && c.CheckOutTime.Month == currentMonth
                                           && c.CheckOutTime.Year == currentYear && c.CheckOutTime.Hour != 0 && c.IdCheck == id).Count();
            var revenue = db.Bills.Where(c => c.Date.Month == currentMonth && c.Date.Year == currentYear && c.IdCheck == id).ToList();
            var dateva = db.Vacations.Where(c => c.Date.Month == currentMonth && c.Date.Year == currentYear && c.IdCheck == id).Count();
            ViewBag.Cin = cin;
            ViewBag.Cout = cout;

            var totalRevenue = revenue
                .Select(c => (double?)c.Value) // Chuyển cột `Value` sang nullable type
                .Sum() ?? 0; // Nếu null, trả về giá trị mặc định là 0

            ViewBag.Revenue = totalRevenue;
            ViewBag.Dateva = dateva;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NotFound()
        {
            

            return View();
        }
        [RoleAuthorization("Admin")]
        public ActionResult Admin()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var currentDay = DateTime.Now.Day;
            var cin = db.CheckInOuts.Where(c => c.CheckInTime.Day == currentDay && c.CheckInTime.Month == currentMonth
                                           && c.CheckInTime.Year == currentYear).Count();
            var cout = db.CheckInOuts.Where(c => c.CheckOutTime.Day == currentDay && c.CheckOutTime.Month == currentMonth
                                           && c.CheckOutTime.Year == currentYear && c.CheckOutTime.Hour!=0).Count();
            var revenue= db.Bills.Where(c => c.Date.Month==currentMonth && c.Date.Year==currentYear).Sum(c=>c.Value);
            var dateva = db.Vacations.Where(c => c.Date.Month == currentMonth && c.Date.Year == currentYear).Count();
            ViewBag.Cin=cin;
            ViewBag.Cout=cout; 
            ViewBag.Revenue=revenue;
            ViewBag.Dateva=dateva;
            return View();
        }
    }
}