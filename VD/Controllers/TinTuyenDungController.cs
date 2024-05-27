using System;
using System.Collections.Generic;
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
        public ActionResult Home_TTD()
        {
            return View();
        }
    }
}