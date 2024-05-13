using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;

namespace VD.Controllers
{
    public class HomeController : Controller
    {
        QLTDEntities db = new QLTDEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainPage(TaiKhoan taiKhoan)
        {

            return View();
        }

        public ActionResult DangKyNTD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKyNTD(NhaTuyenDung nhaTuyenDung, string tenDangNhap, string matKhau)
        {
            // Tạo một đối tượng mới của TaiKhoan và gán giá trị từ đầu vào
            TaiKhoan taiKhoan = new TaiKhoan
            {
                ID_Admin = 1,
                TenDN = tenDangNhap,
                MK = matKhau,
                NgayTao = DateTime.Now
                // Các thuộc tính khác của tài khoản có thể được gán tại đây
            };

            // Thêm tài khoản vào cơ sở dữ liệu
            db.TaiKhoans.Add(taiKhoan);
            db.SaveChanges();

            // Sau khi thêm tài khoản thành công, lấy ID_TK của tài khoản mới được thêm vào
            int taiKhoanId = taiKhoan.ID_TK;

            // Gán ID_TK vào đối tượng NhaTuyenDung
            nhaTuyenDung.ID_TK = taiKhoanId;

            // Thêm đối tượng NhaTuyenDung vào cơ sở dữ liệu
            db.NhaTuyenDungs.Add(nhaTuyenDung);
            db.SaveChanges();

            // Sau khi thêm thành công, chuyển hướng đến action DangNhapNTD
            return RedirectToAction("MainPage", "Home");
        }
        public ActionResult DangNhapNTD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapNTD(TaiKhoan taiKhoan)
        {
            var tendnForm = taiKhoan.TenDN;
            var matkhauForm = taiKhoan.MK;
            var userCheck = db.TaiKhoans.Where(t => db.NhaTuyenDungs.Any(u => u.ID_TK == t.ID_TK)).SingleOrDefault(x => x.TenDN.Equals(tendnForm) && x.MK.Equals(matkhauForm));
            if (userCheck != null)
            {
                Session["TaiKhoan"] = userCheck;
                return RedirectToAction("MainPage", "Home");
            }
            else
            {
                ViewBag.LoginFail = "Đăng nhập thất bại";
                return View("DangNhapNTD");
            }
        }
        public ActionResult DangKyUV()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKyUV(UngVien ungVien, string tenDangNhap, string matKhau, HttpPostedFileBase cv, HttpPostedFileBase syll)
        {
            // Tạo một đối tượng mới của TaiKhoan và gán giá trị từ đầu vào
            TaiKhoan taiKhoan = new TaiKhoan
            {
                ID_Admin = 1,
                TenDN = tenDangNhap,
                MK = matKhau,
                NgayTao = DateTime.Now
                // Các thuộc tính khác của tài khoản có thể được gán tại đây
            };

            // Thêm tài khoản vào cơ sở dữ liệu
            db.TaiKhoans.Add(taiKhoan);
            db.SaveChanges();

            // Sau khi thêm tài khoản thành công, lấy ID_TK của tài khoản mới được thêm vào
            int taiKhoanId = taiKhoan.ID_TK;

            // Gán ID_TK vào đối tượng NhaTuyenDung
            ungVien.ID_TK = taiKhoanId;

            //Xứ lý lưu file CV,SYLL
            if(cv.ContentLength > 0)
            {
                string rootFolder = Server.MapPath("/CV/");
                string pathFile = rootFolder + cv.FileName;
                cv.SaveAs(pathFile);
                ungVien.CV ="/CV/" + cv.FileName;
            }
            if (syll.ContentLength > 0)
            {
                string rootFolder = Server.MapPath("/SYLL/");
                string pathFile = rootFolder + cv.FileName;
                cv.SaveAs(pathFile);
                ungVien.SYLL = "/SYLL/" + syll.FileName;
            }
            // Thêm đối tượng NhaTuyenDung vào cơ sở dữ liệu
            db.UngViens.Add(ungVien);
            db.SaveChanges();

            // Sau khi thêm thành công, chuyển hướng đến action DangNhapNTD
            return RedirectToAction("MainPage", "Home");
        }

        public ActionResult DangNhapUV(TaiKhoan taiKhoan)
        {
            var tenDNForm = taiKhoan.TenDN;
            var matKhauForm = taiKhoan.MK;
            var userCheck = db.TaiKhoans.Where(t => db.UngViens.Any(u => u.ID_TK == t.ID_TK)).SingleOrDefault(x => x.TenDN.Equals(tenDNForm) && x.MK.Equals(matKhauForm));
            if (userCheck != null)
            {
                Session["TaiKhoan"] = userCheck;
                return RedirectToAction("MainPage", "Home");
            }
            else
            {
                ViewBag.LoginFail = "Đăng nhập thất bại";
                return View("DangNhapUV");
            }
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
        public ActionResult LogOut()
        { 
            return View("DangNhapNTD");
        }
        public ActionResult DoiUV(int id)
        {
            var ungVien = db.UngViens.Find(id);
            return View(ungVien);
        }
        [HttpPost]
        public ActionResult DoiUV(UngVien ungVien/*, TaiKhoan taiKhoan*/)
        {
            //db.TaiKhoans.Attach(taiKhoan);
            //db.Entry(taiKhoan).Property(model=>model.TenDN).IsModified = true;
            //db.Entry(taiKhoan).Property(model=>model.MK).IsModified = true;
            //db.SaveChanges();

            db.UngViens.Attach(ungVien);
            db.Entry(ungVien).Property(model => model.HoTen).IsModified = true;
            db.Entry(ungVien).Property(model => model.SDT).IsModified = true;
            db.Entry(ungVien).Property(model => model.DOB).IsModified = true;
            db.Entry(ungVien).Property(model => model.GioiTinh).IsModified = true;
            db.Entry(ungVien).Property(model => model.Email).IsModified = true;
            db.Entry(ungVien).Property(model => model.CV).IsModified = true;
            db.Entry(ungVien).Property(model => model.SYLL).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("MainPage", "Home");
        }
    }
}