using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VD.Models;

namespace VD.Controllers
{
    public class BaiDang_TTDController : Controller
    {
        private readonly QLTDEntities db;
        public BaiDang_TTDController()
        {
            db = new QLTDEntities();
        }
        // GET: BaiDang_TTD
        public ActionResult Index_BDTTD()
        {
            //var nhaTuyenDung = db.NhaTuyenDungs.Find(id);
            //var tinTuyenDungs = from ttd in db.TinTuyenDungs
            //                    join ntd in db.NhaTuyenDungs on ttd.ID_NTD equals ntd.ID_NTD
            //                    join tk in db.TaiKhoans on ntd.ID_TK equals tk.ID_TK
            //                    where ttd.ID_NTD == ntd.ID_NTD && ntd.ID_TK == id
            //                    select ttd;
            List<TinTuyenDung> tinTuyenDungs = db.TinTuyenDungs.ToList();
            return View(tinTuyenDungs.ToList());
        }
    }
}