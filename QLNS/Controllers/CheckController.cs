using Newtonsoft.Json;
using QLNS.App_Start;
using QLNS.Models;
using QLNS.ViewsModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ZXing.QrCode;

namespace QLNS.Controllers
{
    [RoleAuthorization("Admin")]

    public class CheckController : Controller
    {
        private QLNSContext db = new QLNSContext();
        public ActionResult Index()
        {
            
            return View(db.CheckInOuts.ToList());
        }
        
        public ActionResult CheckIn() 
        {
            return View();
        }
        

        public ActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        public ActionResult test(string qrText)
        {
            Byte[] byteArray;
            var width = 250; // width of the Qr Code
            var height = 250; // height of the Qr Code
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write(qrText);

            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    // save to stream as PNG
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = ms.ToArray();
                }
            }
            return View(byteArray);
        }
        [HttpPost]
        public JsonResult CheckIn(string qrData)
        {
            try
            {
                if (string.IsNullOrEmpty(qrData))
                {
                    return Json(new { success = false, message = "Dữ liệu QR không hợp lệ." });
                }

                
                CheckForm checkForm;
                try
                {
                    
                    checkForm = JsonConvert.DeserializeObject<CheckForm>(qrData);
                   
                    
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi khi parse JSON: " + ex.Message });
                }
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);
                DateTime localToday = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone).Date;
                if (localTime != null)
                {
                    var inc = db.CheckInOuts
                                .Where(i => i.IdCheck == checkForm.IdCheck
                                         && DbFunctions.TruncateTime(i.CheckInTime) == localToday)
                                .FirstOrDefault();
                    if (inc != null)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Bạn đã check in hôm nay",
                            data = checkForm
                        });
                    }
                    var check = new CheckInOut()
                    {
                        IdCheck = checkForm.IdCheck,
                        Name = checkForm.Name,
                        CheckInTime = localTime,
                        CheckOutTime = localToday,
                        Coe=0

                    };
                    db.CheckInOuts.Add(check);
                    db.SaveChanges();
                }
                
                return Json(new
                {
                    success = true,
                    message = "CheckIn thành công!",
                    data = checkForm
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xử lý: " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult CheckOut(string qrData)
        {
            try
            {
                if (string.IsNullOrEmpty(qrData))
                {
                    return Json(new { success = false, message = "Dữ liệu QR không hợp lệ." });
                }


                CheckForm checkForm;
                try
                {

                    checkForm = JsonConvert.DeserializeObject<CheckForm>(qrData);


                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi khi parse JSON: " + ex.Message });
                }
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);
                DateTime localToday = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone).Date;
                if (localTime != null)
                {
                    var cout = db.CheckInOuts
                                .Where(i => i.IdCheck == checkForm.IdCheck
                                         && DbFunctions.TruncateTime(i.CheckInTime) == localToday)
                                .FirstOrDefault();
                    if (cout != null && cout.CheckOutTime==localToday)
                    {
                        cout.CheckOutTime = localTime;
                        TimeSpan workDuration = cout.CheckOutTime - cout.CheckInTime;

                        double totalHoursWorked = workDuration.TotalHours;



                        if (totalHoursWorked >= 8)
                        {
                            cout.Coe = 1.0;
                        }
                        else
                        {
                            cout.Coe = Math.Round(totalHoursWorked / 8.0, 2);
                        }
                        
                        db.Entry(cout).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else if(cout == null) 
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Hôm nay bạn chưa check-in!",
                            data = checkForm
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Hôm nay bạn đã check-out!",
                            data = checkForm
                        });
                    }

                   
                }

                return Json(new
                {
                    success = true,
                    message = "CheckOut thành công!",
                    data = checkForm
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xử lý: " + ex.Message });
            }
        }
    }
}