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
        public ActionResult Index_CV(int id)
        {
            /*var taiKhoan = Session["TaiKhoan"] as VD.Models.TaiKhoan;
            var nhaTuyenDung = Session["NhaTuyenDung"] as VD.Models.NhaTuyenDung;
            if (taiKhoan == null)
            {
                return RedirectToAction("Index_Home", "Home");
            }
            var congViecs = _db.CongViecs
                .Include(cv => cv.NganhNghe)
                .Include(cv => cv.ND_Admin)
                .Include(cv => cv.NhaTuyenDung)
                .Include(cv => cv.DiaDiem)
                .Include(cv => cv.LinhVuc)
                .Where(cv => cv.ID_NTD == nhaTuyenDung.ID_NTD)
                .Where(cv => nhaTuyenDung.ID_TK == taiKhoan.ID_TK)
                .ToList();
            return View(congViecs);*/
            var nhaTuyenDung = db.NhaTuyenDungs.Find(id);
            var congViecs = from cv in db.CongViecs
                            join ntd in db.NhaTuyenDungs on cv.ID_NTD equals ntd.ID_NTD
                            join tk in db.TaiKhoans on ntd.ID_TK equals tk.ID_TK
                            where cv.ID_NTD == ntd.ID_NTD && ntd.ID_TK == id
                            select cv;

            return View(congViecs.ToList());
        }
        public ActionResult Add_CV()
        {
            /*ViewBag.NganhNghes = db.NganhNghes.ToList();
            ViewBag.NhaTuyenDungs = db.NhaTuyenDungs.ToList();
            ViewBag.DiaDiems = db.DiaDiems.ToList();
            ViewBag.LinhVucs = db.LinhVucs.ToList();*/
            
            LoadViewBags();
            return View();
        }
        [HttpPost]
        public ActionResult Add_CV(CongViec congViec, int id)
        {
            var nhaTuyenDung = db.NhaTuyenDungs.Find(id);


            //try
            //{
                if (ModelState.IsValid)
                {
                //congViec.ID_Admin = 1;
                db.CongViecs.Add(congViec);
                db.SaveChanges();
                return RedirectToAction("Index_CV");
                }
            //}
            /*catch (DbEntityValidationException ex)
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
            }*/
            LoadViewBags();
            return View(congViec);


            /*ViewBag.NganhNghes = db.NganhNghes.ToList();
            ViewBag.NhaTuyenDungs = db.NhaTuyenDungs.ToList();
            ViewBag.DiaDiems = db.DiaDiems.ToList();
            ViewBag.LinhVucs = db.LinhVucs.ToList();*/

        }
        private void LoadViewBags()
        {
            ViewBag.NganhNghes = db.NganhNghes.ToList();
            ViewBag.NhaTuyenDungs = db.NhaTuyenDungs.ToList();
            ViewBag.DiaDiems = db.DiaDiems.ToList();
            ViewBag.LinhVucs = db.LinhVucs.ToList();
        }
    }
}