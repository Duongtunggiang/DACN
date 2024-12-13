using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLNS.App_Start;
using QLNS.Models;
using ZXing;
using ZXing.QrCode;

namespace QLNS.Controllers
{
    [RoleAuthorization("Admin")]
    public class EmployeesController : Controller
    {
        private QLNSContext db = new QLNSContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Account).Include(e => e.Salary);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Accounts, "Id", "Username");
            ViewBag.Id = new SelectList(db.Salaries, "Id", "Id");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Age,Address,Phone,Avatar,Gender,StartDate,Email,Coe,Description,AccountId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Accounts, "Id", "Username", employee.Id);
            ViewBag.Id = new SelectList(db.Salaries, "Id", "Id", employee.Id);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Accounts, "Id", "Username", employee.Id);
            ViewBag.Id = new SelectList(db.Salaries, "Id", "Id", employee.Id);
            return PartialView(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Age,Address,Phone,Gender,StartDate,Email,Coe,Description,AccountId,CCCD,BHYT")] Employee employee, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin hiện tại của nhân viên từ cơ sở dữ liệu
                var existingEmployee = db.Employees.Find(employee.Id);


                // Cập nhật Avatar nếu có file tải lên
                if (file != null && file.ContentLength > 0)
                {
                    string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string fileExtension = Path.GetExtension(file.FileName);

                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string newFileName = $"{originalFileName}_{timestamp}{fileExtension}";

                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), newFileName);

                    file.SaveAs(_path);

                    existingEmployee.Avatar = "/UploadedFiles/" + newFileName;
                    
                }
                else
                {
                    employee.Avatar = existingEmployee.Avatar;
                }
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Age = employee.Age;
                existingEmployee.Address = employee.Address;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.StartDate = employee.StartDate;
                existingEmployee.Email = employee.Email;
                existingEmployee.Coe = employee.Coe;
                existingEmployee.Description = employee.Description;
                existingEmployee.AccountId = employee.AccountId;
                existingEmployee.CCCD = employee.CCCD;
                existingEmployee.BHYT = employee.BHYT;

                db.Entry(existingEmployee).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
