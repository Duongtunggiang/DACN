using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using QLNS.App_Start;
using QLNS.Models;
using QLNS.ViewsModel;

namespace QLNS.Controllers
{
    
    public class AccountsController : Controller
    {
        private QLNSContext db = new QLNSContext();

        // GET: Accounts
        
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Employee);
            return View(accounts.ToList());
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginForm acc)
        {
            var e= db.Accounts.Where(a=>a.Username==acc.UserName && a.Password==acc.Password).FirstOrDefault();
            if (e==null)
            {
                ViewData["msg"] = "Tên đăng nhập hoặc mật khẩu không chính xác";
                return View();
            }
            var idp=e.Account_Positions.FirstOrDefault().PositionId;
            var namep=db.Positions.Where(n=>n.Id==idp).FirstOrDefault().Name;
            if (namep != null)
            {
                Session["role"] = namep;
                Session["accountId"] = e.Id;
                return RedirectToAction("Index", "Home");
            }
            return View();
            
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ResgisterForm infor)
        {
            var e = new Employee()
            {
                FirstName = infor.FirstName,
                LastName = infor.LastName,
                Email = infor.Email,
                Coe = 1.2,
                StartDate = DateTime.Now,
            };
            db.Employees.Add(e);
            db.SaveChanges();
            var a = new Account()
            {
                Username = infor.Email,
                Password = infor.Password,
            };
            a.Id = e.Id;
            db.Accounts.Add(a);
            db.SaveChanges();
            var r = new Account_Position()
            {
                AccountId = a.Id,
                PositionId = db.Positions.Where(p=>p.Name== "Employee").FirstOrDefault().Id,
            };
            db.Account_Positions.Add(r);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                var e = new Employee()
                {
                    FirstName = "Dương",
                    LastName = "Dương",
                    Coe = 1.2,
                    StartDate = DateTime.Now,
                };
                db.Employees.Add(e);
                db.SaveChanges();
                account.Id = e.Id;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", account.Id);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", account.Id);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", account.Id);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AccountInfor()
        {
            var accountIdSession = Session["accountId"];
            if (accountIdSession != null && int.TryParse(accountIdSession.ToString(), out int accountId))
            {
                var user = db.Employees.Where(e => e.Id == accountId).FirstOrDefault();
                if (user != null)
                {
                    return View(user);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
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
