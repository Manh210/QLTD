using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;
using System.Data.Entity;

namespace VD.Areas.Admin.Controllers
{
    public class TK_UngVienController : Controller
    {
        private readonly QLTDEntities db;
        public TK_UngVienController()
        {
            db = new QLTDEntities();
        }
        // GET: Admin/TK_UngVien
        public ActionResult Index(string timTK_UV = null)
        {
            List<TaiKhoan> taiKhoans = db.TaiKhoans.Where(t => db.UngViens.Any(u => u.ID_TK == t.ID_TK)).ToList();
            if (!string.IsNullOrEmpty(timTK_UV))
            {
                taiKhoans = db.TaiKhoans.Where(dd => dd.TenDN.Contains(timTK_UV)).ToList();
            }
            return View(taiKhoans);
        }

        public ActionResult ChiTietTK(int id)
        {
            var ungVien = db.UngViens.Include(nn => nn.DiaDiem).FirstOrDefault(u => u.ID_TK == id);
            if (ungVien == null)
            {
                return HttpNotFound();
            }
            return View(ungVien);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Tìm tất cả các UngVien có ID_TK tương ứng
                    var ungViens = db.UngViens.Where(u => u.ID_TK == id).ToList();
                    if (ungViens.Any())
                    {
                        db.UngViens.RemoveRange(ungViens);
                    }

                    // Tìm TaiKhoan cần xóa
                    var taiKhoan = db.TaiKhoans.Find(id);
                    if (taiKhoan != null)
                    {
                        db.TaiKhoans.Remove(taiKhoan);
                    }

                    // Lưu thay đổi
                    db.SaveChanges();

                    // Commit transaction
                    transaction.Commit();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi xảy ra
                    transaction.Rollback();
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }
    }
}