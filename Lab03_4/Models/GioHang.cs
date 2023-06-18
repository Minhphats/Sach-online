using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab03_4.Models
{
    public class GioHang
    {
        QLBanSachEntities1 db = new QLBanSachEntities1();
        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien { get { return dDonGia * iSoLuong; } }
        public GioHang(int ms)
        {
            iMaSach = ms;
            SACH s = db.SACH.Single(n => n.MaSach == iMaSach);
            sTenSach = s.TenSach;
            sAnhBia = s.HinhMinhHoa;
            dDonGia = double.Parse(s.DonGia.ToString());
            iSoLuong = 1;

        }
    }
}