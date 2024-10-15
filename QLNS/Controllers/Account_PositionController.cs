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
    public class Account_PositionController : Controller
    {
        private QLNSContext db = new QLNSContext();

        // GET: Account_Position
        public ActionResult Index()
        {
            var account_Positions = db.Account_Positions.Include(a => a.Account).Include(a => a.Position);
            return View(account_Positions.ToList());
        }

        // GET: Account_Position/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account_Position account_Position = db.Account_Positions.Find(id);
            if (account_Position == null)
            {
                return HttpNotFound();
            }
            return View(account_Position);
        }

        // GET: Account_Position/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Username");
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name");
            return View();
        }

        // POST: Account_Position/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionId,AccountId")] Account_Position account_Position)
        {
            if (ModelState.IsValid)
            {
                db.Account_Positions.Add(account_Position);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Username", account_Position.AccountId);
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", account_Position.PositionId);
            return View(account_Position);
        }

        // GET: Account_Position/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account_Position account_Position = db.Account_Positions.Find(id);
            if (account_Position == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Username", account_Position.AccountId);
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", account_Position.PositionId);
            return View(account_Position);
        }

        // POST: Account_Position/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionId,AccountId")] Account_Position account_Position)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account_Position).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Username", account_Position.AccountId);
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", account_Position.PositionId);
            return View(account_Position);
        }

        // GET: Account_Position/Delete/5
        public ActionResult Delete(int? accountId)
        {
            if (accountId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account_Position account_Position = db.Account_Positions.Where(e=>e.AccountId==accountId).FirstOrDefault();
            if (account_Position == null)
            {
                return HttpNotFound();
            }
            return View(account_Position);
        }

        // POST: Account_Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int accountId)
        {
            Account_Position account_Position = db.Account_Positions.Where(e => e.AccountId == accountId).FirstOrDefault();
            db.Account_Positions.Remove(account_Position);
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
