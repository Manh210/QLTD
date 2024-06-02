using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace VD.Controllers
{
    public class QLCVController : Controller
    {
        // GET: QLCV
        //QLTDEntities db = new QLTDEntities();
        private QLTDEntities db;
        public QLCVController()
        {
            db = new QLTDEntities();
        }
        public ActionResult Index_CV(int id, string timCV = null)
        {
            var nhaTuyenDung = db.NhaTuyenDungs.Find(id);
            var congViecs = from cv in db.CongViecs
                            join ntd in db.NhaTuyenDungs on cv.ID_NTD equals ntd.ID_NTD
                            join tk in db.TaiKhoans on ntd.ID_TK equals tk.ID_TK
                            where cv.ID_NTD == ntd.ID_NTD && ntd.ID_TK == id
                            select cv;
            if (!string.IsNullOrEmpty(timCV))
            {
                congViecs = db.CongViecs.Where(dd => dd.TenCV.Contains(timCV));
            }
            return View(congViecs.ToList());
        }
        public ActionResult Add_CV()
        {
            LoadViewBags();
            return View();
        }
        [HttpPost]
        public ActionResult Add_CV(CongViec congViec)
        {
            var taiKhoan = Session["TaiKhoan"] as VD.Models.TaiKhoan;
            try
            {
                if (ModelState.IsValid)
                {
                    congViec.ID_Admin = 1;
                    db.CongViecs.Add(congViec);
                    db.SaveChanges();
                    //return RedirectToAction("QLCV/Index_CV/iD_TK");
                    return RedirectToAction("Index_CV", "QLCV", new { id = taiKhoan.ID_TK });
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
            return View(congViec);
        }
        private void LoadViewBags()
        {
            ViewBag.NganhNghes = db.NganhNghes.ToList();
            ViewBag.NhaTuyenDungs = db.NhaTuyenDungs.ToList();
            ViewBag.DiaDiems = db.DiaDiems.ToList();
            ViewBag.LinhVucs = db.LinhVucs.ToList();
        }
        public ActionResult Edit_CV(int id)
        {
            var congViec = db.CongViecs.Find(id);
            LoadViewBags();
            return View(congViec);
        }
        [HttpPost]
        public ActionResult Edit_CV(CongViec congViec)
        {
            var taiKhoan = Session["TaiKhoan"] as VD.Models.TaiKhoan;
            db.CongViecs.Attach(congViec);
            db.Entry(congViec).Property(model => model.TenCV).IsModified = true;
            db.Entry(congViec).Property(model => model.MoTaCV).IsModified = true;
            db.Entry(congViec).Property(model => model.LoaiViec).IsModified = true;
            db.Entry(congViec).Property(model => model.ID_NN).IsModified = true;
            db.Entry(congViec).Property(model => model.ID_DD).IsModified = true;
            db.Entry(congViec).Property(model => model.ID_LV).IsModified = true;
            db.Entry(congViec).Property(model => model.ID_NTD).IsModified = true;
            LoadViewBags();
            db.SaveChanges();
            return RedirectToAction("Index_CV", "QLCV", new { id = taiKhoan.ID_TK });
        }
    }
}