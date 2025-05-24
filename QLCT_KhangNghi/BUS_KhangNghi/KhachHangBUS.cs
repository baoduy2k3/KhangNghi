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
    public class KhachHangBUS
    {
        KhachHangDAL dal = new KhachHangDAL();

        public DataTable LayDanhSachKhachHang()
        {
            return dal.LayDanhSachKhachHang();
        }

        public DataTable LayDanhSachLoaiKH()
        {
            return dal.LayDanhSachLoaiKH();
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            // Có thể thêm validate nghiệp vụ ở đây nếu cần
            return dal.ThemKhachHang(kh);
        }

        public bool SuaKhachHang(KhachHangDTO kh)
        {
            // Có thể thêm validate nghiệp vụ ở đây nếu cần
            return dal.SuaKhachHang(kh);
        }

        public bool XoaKhachHang(string maKH)
        {
            return dal.XoaKhachHang(maKH);
        }
    }
}
