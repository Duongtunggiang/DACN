using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLNS.Models;

namespace QLNS.Controllers
{
    public class VacationsController : Controller
    {
        private QLNSContext db = new QLNSContext();

        // GET: Vacations
        public ActionResult Index()
        {
            return View(db.Vacations.ToList());
        }
        public ActionResult HistoryVacationEmployee()
        {
            var id = Session["idcheck"].ToString();
            var vas = db.Vacations.Where(s => s.IdCheck == id).ToList();
            return View(vas);
        }

        // GET: Vacations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacation vacation = db.Vacations.Find(id);
            if (vacation == null)
            {
                return HttpNotFound();
            }
            return View(vacation);
        }

        // GET: Vacations/Create
        public ActionResult Create()
        {
            ViewBag.Employees1 = db.Employees.Select(e => new SelectListItem
            {
                Value = e.IdCheck.ToString(),
                Text = e.FirstName + " " + e.LastName
            }).ToList();
            return PartialView();
        }
        public ActionResult CreateByEmp()
        {
            ViewBag.Employees1 = db.Employees.Select(e => new SelectListItem
            {
                Value = e.IdCheck.ToString(),
                Text = e.FirstName + " " + e.LastName
            }).ToList();
            return PartialView();
        }
        // POST: Vacations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IdCheck,Reson,Description,Date")] Vacation vacation, string SelectedItem1)
        {
            if (ModelState.IsValid)
            {
                var countDate= db.Vacations.Where(va=>va.IdCheck==SelectedItem1).Count();
                if (countDate>=4)
                {
                    
                }
                else
                {
                    vacation.Date = DateTime.Now;
                    vacation.IdCheck = SelectedItem1;
                    var em = db.Employees.Where(e => e.IdCheck == SelectedItem1).FirstOrDefault();
                    vacation.Name = em.LastName + " " + em.FirstName;
                    db.Vacations.Add(vacation);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }

            return View(vacation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateByEmp([Bind(Include = "Id,Name,IdCheck,Reson,Description,Date")] Vacation vacation)
        {
            var id = Session["idcheck"].ToString();
            if (ModelState.IsValid)
            {
                var countDate = db.Vacations.Where(va => va.IdCheck == id).Count();
                if (countDate >= 4)
                {

                }
                else
                {
                    vacation.Date = DateTime.Now;
                    vacation.IdCheck = id;
                    var em = db.Employees.Where(e => e.IdCheck == id).FirstOrDefault();
                    vacation.Name = em.LastName + " " + em.FirstName;
                    db.Vacations.Add(vacation);
                    db.SaveChanges();

                }
                return RedirectToAction("HistoryVacationEmployee");
            }

            return View(vacation);
        }
        // GET: Vacations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacation vacation = db.Vacations.Find(id);
            if (vacation == null)
            {
                return HttpNotFound();
            }
            return PartialView(vacation);
        }

        // POST: Vacations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IdCheck,Reson,Description,Date")] Vacation vacation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vacation);
        }


        // POST: Vacations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vacation vacation = db.Vacations.Find(id);
            db.Vacations.Remove(vacation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedByEmp(int id)
        {
            Vacation vacation = db.Vacations.Find(id);
            db.Vacations.Remove(vacation);
            db.SaveChanges();
            return RedirectToAction("HistoryVacationEmployee");
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
