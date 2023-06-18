using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab03_4.Models;
using PagedList;
using PagedList.Mvc;
namespace Lab03_4.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        private QLBanSachEntities1 data = new QLBanSachEntities1();
        
        public ActionResult Index()
        {
            return View(data.SACH.Take(9));
        }
        public ActionResult Chude()
        {
            var chude = from CD in data.CHUDE select CD;
            return PartialView(chude);
        }
        public ActionResult SachTheoChuDe(int iMaCD, int ? page)
        {
            ViewBag.MaCD = iMaCD;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var sach = from s in data.SACH where s.MaCD == iMaCD select s;
            return View(sach.ToPagedList(iPageNum, iSize));
        }
        public ActionResult NXB()
        {
            var NXB = from Nhaxuatban in data.NHAXUATBAN select Nhaxuatban;
            return PartialView(NXB);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACH where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Chitietsach(int id)
        {
            var sach = from s in data.SACH
                       where s.MaSach == id
                       select s;
            return View(sach.Single());
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogout");
        }
    }
}