using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;

namespace VD.Areas.Admin.Controllers
{
    public class QLTinTuyenDungController : Controller
    {
        private readonly QLTDEntities db;
        public QLTinTuyenDungController()
        {
            db = new QLTDEntities();
        }
        // GET: Admin/QLTinTuyenDung
        public ActionResult Home_QLTTD(string timTTD = null)
        {
            List<TinTuyenDung> tinTuyenDungs = db.TinTuyenDungs.ToList();
            if (!string.IsNullOrEmpty(timTTD))
            {
                tinTuyenDungs = db.TinTuyenDungs.Where(dd => dd.TieuDe.Contains(timTTD)).ToList();
            }
            return View(tinTuyenDungs);
        }
        private void LoadViewBags()
        {
            ViewBag.NhaTuyenDungs = db.NhaTuyenDungs.ToList();
            ViewBag.CongViecs = db.CongViecs.ToList();
        }
        public ActionResult Edit_TT(int id)
        {
            var tinTuyenDung = db.TinTuyenDungs.Find(id);
            LoadViewBags();
            return View(tinTuyenDung);
        }
        [HttpPost]
        public ActionResult Edit_TT(TinTuyenDung tinTuyenDung)
        {
            db.TinTuyenDungs.Attach(tinTuyenDung);
            db.Entry(tinTuyenDung).Property(model => model.TrangThaiTTD).IsModified = true;
            LoadViewBags();
            db.SaveChanges();
            return RedirectToAction("Home_QLTTD");
        }
        [HttpPost]
        public JsonResult UpdateStatus(int id, string status)
        {
            try
            {
                var tinTuyenDung = db.TinTuyenDungs.Find(id);
                if (tinTuyenDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng." });
                }

                tinTuyenDung.TrangThaiTTD = status;
                tinTuyenDung.TgianCapNhatTT = DateTime.Now;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}