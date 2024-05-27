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
        public ActionResult Home_QLTTD()
        {
            List<TinTuyenDung> tinTuyenDungs = db.TinTuyenDungs.ToList();
            return View(tinTuyenDungs);
        }
    }
}