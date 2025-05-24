using DAL_KhangNghi;
using DTO_KhangNghi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_KhangNghi
{
    public class SanPhamBUS
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public DataTable LayDanhSachSanPham() => dal.LayDanhSachSanPham();

        public bool ThemSanPham(SanPhamDTO sp) => dal.ThemSanPham(sp);

        public bool XoaSanPham(string maSP) => dal.XoaSanPham(maSP);

        public bool SuaSanPham(SanPhamDTO sp) => dal.SuaSanPham(sp);

        public string LayMaTuDong() => dal.LayMaSanPhamMoiNhat();

        public DataTable LayDanhSachLoaiSanPham() => dal.LayDanhSachLoaiSanPham();
    }
}
