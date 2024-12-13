using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using QLNS.Models;

namespace QLNS.Controllers
{
    public class SalariesController : Controller
    {
        private QLNSContext db = new QLNSContext();

        // GET: Salaries
        public ActionResult Index()
        {
            var salaries = db.Salaries.Include(s => s.Employee);
            return View(salaries.ToList());
        }
        public ActionResult HistorySalaryEmployee()
        {
            var id =Session["idcheck"].ToString();
            var salaries =db.SalaryHistories.Where(s=>s.IdCheck == id).ToList();
            return View(salaries);
        }
        // GET: Salaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            return View(salary);
        }

        // GET: Salaries/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SalaryBase,KPI,DateVacation,IdCheck")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                db.Salaries.Add(salary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", salary.Id);
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", salary.Id);
            return PartialView(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SalaryBase,KPI,DateVacation,IdCheck,NameEmployee")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", salary.Id);
            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Salary salary = db.Salaries.Find(id);
            db.Salaries.Remove(salary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateSalaryTable()
        {
            string mess;
            var employees= db.Employees.ToList();
            foreach (var item in employees)
            {
                var e = db.Salaries.Where(s => s.IdCheck == item.IdCheck).FirstOrDefault();
                if (e == null && item.IdCheck!=null) {
                    var sal = new Salary() {
                        NameEmployee=item.LastName+" "+item.FirstName,
                        IdCheck = item.IdCheck,
                        KPI = 24,
                        DateVacation=4,
                        SalaryBase= 5000000, 
                        
                    };
                    db.Salaries.Add(sal);
                    db.SaveChanges();
                }
                ViewBag.Message = "Cập nhật thành công";
            }
            return RedirectToAction("Index");
        }
        public ActionResult HistorySalary()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Kiểm tra xem có bản ghi nào trong tháng này không
            bool hasRecords = db.SalaryHistories.Any(e => e.Date.Month == currentMonth && e.Date.Year == currentYear);
            if (hasRecords)
            {
                ViewBag.mess = "tháng này";
            }
            else
            {
                var employees = db.Employees.ToList();
                foreach (var item in employees)
                {
                    var sa = db.Salaries.Where(s => s.IdCheck == item.IdCheck).FirstOrDefault();
                    var e = db.SalaryHistories.Where(s => s.IdCheck == item.IdCheck).FirstOrDefault();
                    
                    if (e == null && item.IdCheck != null)
                    {
                        var sal = new SalaryHistory()
                        {
                            Name = item.LastName + " " + item.FirstName,
                            IdCheck = item.IdCheck,
                            KPI = 0,
                            KPITarget=sa.KPI,
                            DateVacationTarget=sa.DateVacation,
                            DateVacation = 0,
                            SalaryBase = sa.SalaryBase,
                            Revenue = 0,
                            Reward = 0,
                            Date= DateTime.Now,
                            Coe = item.Coe,
                            Result= 0,
                            Status=false
                        };
                        db.SalaryHistories.Add(sal);
                        db.SaveChanges();
                    }
                    ViewBag.mess = "Cập nhật thành công";
                }
            }
            return View(db.SalaryHistories.ToList());
        }
        [HttpGet]
        public ActionResult UpdateHistorySalaryTable()
        {
            string mess;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var employeesbysa = db.SalaryHistories.ToList();
            var employees = db.Employees.ToList();
            foreach (var item in employees)
            {
                var sa = db.Salaries.Where(s => s.IdCheck == item.IdCheck).FirstOrDefault();
                var e = db.SalaryHistories.Where(s => s.IdCheck == item.IdCheck).FirstOrDefault();

                if (e == null && item.IdCheck != null)
                {
                    var sal = new SalaryHistory()
                    {
                        Name = item.LastName + " " + item.FirstName,
                        IdCheck = item.IdCheck,
                        KPI = 0,
                        KPITarget = sa.KPI,
                        DateVacationTarget = sa.DateVacation,
                        DateVacation = 0,
                        SalaryBase = sa.SalaryBase,
                        Revenue = 0,
                        Reward = 0,
                        Date = DateTime.Now,
                        Coe = item.Coe,
                        Result = 0,
                        Status = false
                    };
                    db.SalaryHistories.Add(sal);
                    db.SaveChanges();
                }
                ViewBag.mess = "Cập nhật thành công";
            }
            foreach (var item in employeesbysa)
            {
                var totalbill = db.Bills.Where(cc => cc.IdCheck == item.IdCheck && cc.Date.Month == currentMonth && cc.Date.Year == currentYear && cc.Value != null).ToList();
                var totalkpi= db.CheckInOuts.Where(cc=>cc.IdCheck==item.IdCheck && cc.CheckInTime.Month == currentMonth && cc.CheckInTime.Year == currentYear && cc.Coe != null).Sum(cc => (double?)cc.Coe ?? 0);
                var totalvacation = db.Vacations.Where(cc => cc.IdCheck == item.IdCheck && cc.Date.Month == currentMonth && cc.Date.Year == currentYear).Count();

                if (totalbill != null)
                {
                    item.Revenue = totalbill.Sum(cc => (double?)cc.Value ?? 0);
                }
                item.KPI = totalkpi;
                item.DateVacation = totalvacation;
                item.Reward =Math.Round(item.Revenue * 0.05,2);
                item.Result = Math.Round(item.SalaryBase * (double)((item.KPI + item.DateVacation) / item.KPITarget) * item.Coe + item.Reward,2);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("HistorySalary");
        }
        public ActionResult ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var historysalary = db.SalaryHistories
                .Select(b => new
                {
                    b.Name,
                    b.IdCheck,
                    b.Date,
                    b.SalaryBase,
                    b.Coe,
                    b.KPI,
                    b.DateVacation,
                    b.Revenue,
                    b.Reward,
                    b.Result,
                    b.Status
                }).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("historysalary");
                worksheet.Cells["A1"].Value = "Tên";
                worksheet.Cells["B1"].Value = "Mã chấm công";
                worksheet.Cells["C1"].Value = "Ngày khởi tạo";
                worksheet.Cells["D1"].Value = "Hệ số lương";
                worksheet.Cells["E1"].Value = "KPI";
                worksheet.Cells["F1"].Value = "Ngày nghỉ phép";
                worksheet.Cells["G1"].Value = "Doanh thu";
                worksheet.Cells["H1"].Value = "Thưởng";
                worksheet.Cells["I1"].Value = "Lương";
                worksheet.Cells["J1"].Value = "Trạng thái";

                int row = 2;
                foreach (var bill in historysalary)
                {
                    worksheet.Cells[row, 1].Value = bill.Name;
                    worksheet.Cells[row, 2].Value = bill.IdCheck;
                    worksheet.Cells[row, 3].Value = bill.Date.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 4].Value = bill.Coe;
                    worksheet.Cells[row, 5].Value = bill.KPI;
                    worksheet.Cells[row, 6].Value = bill.DateVacation;
                    worksheet.Cells[row, 7].Value = bill.Revenue;
                    worksheet.Cells[row, 8].Value = bill.Reward;
                    worksheet.Cells[row, 9].Value = bill.Result;
                    worksheet.Cells[row, 10].Value = bill.Status;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "history_salary.xlsx");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
