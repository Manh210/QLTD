using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;

namespace VD.Areas.Admin.Controllers
{
    public class TK_NhaTuyenDungController : Controller
    {
        QLTDEntities db = new QLTDEntities();
        // GET: Admin/TK_NhaTuyenDung
        public ActionResult Index()
        {
            List<TaiKhoan> taiKhoans = db.TaiKhoans.Where(t => db.NhaTuyenDungs.Any(u => u.ID_TK == t.ID_TK)).ToList();
            return View(taiKhoans);
        }
        public ActionResult ChiTietTK_NTD(int id)
        {
            var nhaTuyenDung = db.NhaTuyenDungs.FirstOrDefault(u => u.ID_TK == id);
            if (nhaTuyenDung == null)
            {
                return HttpNotFound();
            }
            return View(nhaTuyenDung);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Tìm tất cả các NhaTuyenDung có ID_TK tương ứng
                    var nhaTuyenDungs = db.NhaTuyenDungs.Where(u => u.ID_TK == id).ToList();
                    if (nhaTuyenDungs.Any())
                    {
                        db.NhaTuyenDungs.RemoveRange(nhaTuyenDungs);
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