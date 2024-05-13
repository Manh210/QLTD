using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;

namespace VD.Areas.Admin.Controllers
{
    public class TK_UngVienController : Controller
    {
        QLTDEntities db = new QLTDEntities();
        // GET: Admin/TK_UngVien
        public ActionResult Index()
        {
            List<TaiKhoan> taiKhoans = db.TaiKhoans.Where(t => db.UngViens.Any(u => u.ID_TK == t.ID_TK)).ToList();
            return View(taiKhoans);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan != null)
            {
                db.TaiKhoans.Remove(taiKhoan);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}