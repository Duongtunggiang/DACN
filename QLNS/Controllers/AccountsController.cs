using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QLNS.App_Start;
using QLNS.Models;
using QLNS.ViewsModel;
using QRCoder;

namespace QLNS.Controllers
{
    
    public class AccountsController : Controller
    {
        private QLNSContext db = new QLNSContext();

        // GET: Accounts
        [RoleAuthorization("Admin")]
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
            //var inf= db.Employees.Where(i=>i.Id==e.).FirstOrDefault();
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
                Session["avatar"] = e.Employee.Avatar;
                Session["name"] = e.Employee.LastName+' '+e.Employee.FirstName;
                Session["idcheck"] = e.Employee.IdCheck;
                if (namep == "Admin")
                {
                    return RedirectToAction("Admin","Home");
                }
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
            Random random = new Random();
            string qr;
            int randomNumber = 0001;
            var checknumber = db.Employees.Where(p => p.IdCheck == randomNumber.ToString()).FirstOrDefault();
            do
            {
                randomNumber = random.Next(1000, 10000);
                checknumber = db.Employees.Where(p => p.IdCheck == randomNumber.ToString()).FirstOrDefault();
            } while (checknumber != null);
            var jsonData = new
            {
                Name = $"{infor.LastName} {infor.FirstName}",
                Email = infor.Email,
                IdCheck=randomNumber,
            };
            string jsonString = JsonConvert.SerializeObject(jsonData);
            // Bước 3: Tạo QR Code
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(jsonString, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(10))
                    {
                        string filePath = Server.MapPath("~/UploadedFiles/QRCode/") + $"{infor.Email}.png";
                        qrCodeImage.Save(filePath);
                        qr = $"/UploadedFiles/QRCode/{infor.Email}.png";
                        ViewBag.QRCodeImagePath = $"/UploadedFiles/QRCode/{infor.Email}.png";
                    }
                }
            }
            var e = new Employee()
            {
                FirstName = infor.FirstName,
                LastName = infor.LastName,
                Email = infor.Email,
                Coe = 1.2,
                StartDate = DateTime.Now,
                Avatar = "/UploadedFiles/avatar.png",
                IdCheck = randomNumber.ToString(),
                QRCode = qr

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
            return PartialView();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAccountForm infor)
        {
            Random random = new Random();
            string qr;
            int randomNumber = 0001;
            var checknumber = db.Employees.Where(p => p.IdCheck == randomNumber.ToString()).FirstOrDefault();
            do
            {
                randomNumber = random.Next(1000, 10000);
                checknumber = db.Employees.Where(p => p.IdCheck == randomNumber.ToString()).FirstOrDefault();
            } while (checknumber != null);
            var jsonData = new
            {
                Name = $"{infor.LastName} {infor.FirstName}",
                Email = infor.Email,
                IdCheck = randomNumber,
            };
            string jsonString = JsonConvert.SerializeObject(jsonData);
            // Bước 3: Tạo QR Code
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(jsonString, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(10))
                    {
                        string filePath = Server.MapPath("~/UploadedFiles/QRCode/") + $"{infor.Email}.png";
                        qrCodeImage.Save(filePath);
                        qr = $"/UploadedFiles/QRCode/{infor.Email}.png";
                        ViewBag.QRCodeImagePath = $"/UploadedFiles/QRCode/{infor.Email}.png";
                    }
                }
            }
            var e = new Employee()
            {
                FirstName = infor.FirstName,
                LastName = infor.LastName,
                Email = infor.Email,
                Coe = 1.2,
                StartDate = DateTime.Now,
                Avatar = "/UploadedFiles/avatar.png",
                IdCheck = randomNumber.ToString(),
                QRCode = qr

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
                PositionId = db.Positions.Where(p => p.Name == "Employee").FirstOrDefault().Id,
            };
            db.Account_Positions.Add(r);
            db.SaveChanges();
            return View();
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
            return PartialView(account);
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
     

        // POST: Accounts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            Employee employee = db.Employees.Find(id);
            db.Accounts.Remove(account);
            db.Employees.Remove(employee);
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
        public ActionResult EditInfor(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfor([Bind(Include = "Id,FirstName,LastName,Age,Address,Phone,Gender,StartDate,Email,Coe,Description,AccountId,CCCD,BHYT")] Employee employee, HttpPostedFileBase file)
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
                    Session["avatar"] = existingEmployee.Avatar; 
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

                return RedirectToAction("AccountInfor");
            }

            ViewBag.Id = new SelectList(db.Accounts, "Id", "Username", employee.Id);
            ViewBag.Id = new SelectList(db.Salaries, "Id", "Id", employee.Id);

            return View(employee);
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
