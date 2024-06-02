using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;
using System.Data.Entity;

namespace VD.Areas.Admin.Controllers
{
    public class QLNganhNgheController : Controller
    {
        private readonly QLTDEntities db;
        public QLNganhNgheController()
        {
            db = new QLTDEntities();
        }
        // GET: Admin/QLNganhNghe
        public ActionResult Home_NN(string timNN = null)
        {
            var nganhNghes = db.NganhNghes.Include(nn => nn.LinhVuc).ToList();
            if (!string.IsNullOrEmpty(timNN))
            {
                nganhNghes = db.NganhNghes.Where(dd => dd.TenNN.Contains(timNN)).ToList();
            }
            return View(nganhNghes);
        }
        public ActionResult Add_NN()
        {
            ViewBag.LinhVucs = db.LinhVucs.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Add_NN(NganhNghe nganhNghe)
        {
            if (ModelState.IsValid)
            {
                db.NganhNghes.Add(nganhNghe);
                db.SaveChanges();
                return RedirectToAction("Home_NN");
            }
            ViewBag.LinhVucs = db.LinhVucs.ToList();
            return View(nganhNghe);
        }
        public ActionResult Edit_NN(int id)
        {
            var nganhNghe = db.NganhNghes.Find(id);
            ViewBag.LinhVucs = db.LinhVucs.ToList();
            return View(nganhNghe);
        }
        [HttpPost]
        public ActionResult Edit_NN(NganhNghe nganhNghe)
        {
            db.NganhNghes.Attach(nganhNghe);
            db.Entry(nganhNghe).Property(model => model.TenNN).IsModified = true;
            db.Entry(nganhNghe).Property(model => model.MoTaNN).IsModified = true;
            db.Entry(nganhNghe).Property(model => model.ID_LV).IsModified = true;
            ViewBag.LinhVucs = db.LinhVucs.ToList();
            db.SaveChanges();
            return RedirectToAction("Home_NN");
        }
        public ActionResult Delete_NN(int id)
        {
            var nganhNghe = db.NganhNghes.Find(id);
            if (nganhNghe != null)
            {
                db.NganhNghes.Remove(nganhNghe);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}