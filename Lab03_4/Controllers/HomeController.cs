using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab03_4.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace Lab03_4.Controllers
{
    public class HomeController : Controller
    {
        QLBanSachEntities1 db = new QLBanSachEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["PassWord"];
            ADMIN ad = db.ADMIN.SingleOrDefault(n => n.TenDNAdmin == sTenDN && n.MatKhauAdmin == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
               
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult List(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.SACH.ToList().OrderBy(n => n.MaSach).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy Chọn Ảnh";
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.Mota = f["sMota"];
                ViewBag.SoLuongBan = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = int.Parse(f["mGiaBan"]);
                ViewBag.DonViTinh = f["iDVT"];
                ViewBag.SoLanXem = int.Parse(f["iSoLanXenm"]);
                ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Sach/"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMota"].Replace("<p>", "").Replace("</p>", "\n");
                    sach.DonGia = int.Parse(f["mGiaBan"]);
                    sach.DonViTinh = f["iDVT"];
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    sach.HinhMinhHoa = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    db.SACH.Add(sach);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            
        }
        public ActionResult Details(int id)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            return View(sach);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteCon(int id,FormCollection f)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CTDATHANG.Where(n => n.MaSach == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sản Phẩm này đang có trong chi tiết đặt hàng<br>" + "Nếu muốn xóa thì phải xóa hết mã này trong chi tiết đặt hàng";
                return View(ctdh);
            }
            var vietsach = db.VIETSACH.Where(n => n.MaSach == id).ToList();
            if(vietsach != null)
            {
                db.VIETSACH.RemoveRange(vietsach);
                db.SaveChanges();
            }
            db.SACH.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe",sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB",sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SACH sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe",sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB",sach.MaNXB);
            
                if (ModelState.IsValid)
                {
                if (fFileUpload != null)
                 {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Sach/"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.HinhMinhHoa = sFileName;
                    
                 }
                sach.TenSach = f["sTenSach"];
                sach.MoTa = f["sMota"].Replace("<p>", "").Replace("</p>", "\n");
                sach.DonGia = int.Parse(f["mGiaBan"]);
                sach.DonViTinh = f["iDVT"];
                sach.MaCD = int.Parse(f["MaCD"]);
                sach.MaNXB = int.Parse(f["MaNXB"]);
                sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                db.SACH.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

                return View();
            

        }
    }
}