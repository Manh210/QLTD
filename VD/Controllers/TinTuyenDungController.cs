using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;

namespace VD.Controllers
{
    public class TinTuyenDungController : Controller
    {
        private readonly QLTDEntities db;
        public TinTuyenDungController()
        {
            db = new QLTDEntities();
        }
        // GET: TinTuyenDung
        public ActionResult Index_TTD(int id)
        {
            var nhaTuyenDung = db.NhaTuyenDungs.Find(id);
            var tinTuyenDungs = from ttd in db.TinTuyenDungs
                                join ntd in db.NhaTuyenDungs on ttd.ID_NTD equals ntd.ID_NTD
                                join tk in db.TaiKhoans on ntd.ID_TK equals tk.ID_TK
                                where ttd.ID_NTD == ntd.ID_NTD && ntd.ID_TK == id
                                select ttd;
            return View(tinTuyenDungs.ToList());
        }
        private void LoadViewBags()
        {
            ViewBag.NhaTuyenDungs = db.NhaTuyenDungs.ToList();
            ViewBag.CongViecs = db.CongViecs.ToList();
        }
        public ActionResult Add_TTD()
        {
            LoadViewBags();
            return View();
        }
        [HttpPost]
        public ActionResult Add_TTD(TinTuyenDung tinTuyenDung)
        {
            var taiKhoan = Session["TaiKhoan"] as VD.Models.TaiKhoan;
            try
            {
                if (ModelState.IsValid)
                {
                    tinTuyenDung.ID_Admin = 1;
                    tinTuyenDung.TgDang = DateTime.Now;
                    tinTuyenDung.TgianCapNhatTT = DateTime.Now;
                    tinTuyenDung.TrangThaiTTD = "Chờ xác nhận";
                    db.TinTuyenDungs.Add(tinTuyenDung);
                    db.SaveChanges();
                    return RedirectToAction("Index_TTD", "TinTuyenDung", new { id = taiKhoan.ID_TK });
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        // Log the error details
                        System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log general exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
            }
            LoadViewBags();
            return View(tinTuyenDung);
        }
        public ActionResult Edit_TTD(int id)
        {
            var tinTuyenDung = db.TinTuyenDungs.Find(id);
            LoadViewBags();
            return View(tinTuyenDung);
        }
        [HttpPost]
        public ActionResult Edit_TTD(TinTuyenDung tinTuyenDung)
        {
            var taiKhoan = Session["TaiKhoan"] as VD.Models.TaiKhoan;
            db.TinTuyenDungs.Attach(tinTuyenDung);
            db.Entry(tinTuyenDung).Property(model => model.TieuDe).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.YeuCau).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.TgLam).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.HanUT).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.SoLuongTD).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.PhucLoi).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.MucLuong).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.CapBac).IsModified = true;
            db.Entry(tinTuyenDung).Property(model => model.ID_CV).IsModified = true;
            LoadViewBags();
            db.SaveChanges();
            return RedirectToAction("Index_TTD", "TinTuyenDung", new { id = taiKhoan.ID_TK });
        }
    }
}