using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_KhangNghi;
using DTO_KhangNghi;

namespace BUS_KhangNghi
{
    public class NhanVienBUS
    {

        private NhanVienDAL dal = new NhanVienDAL();

        public DataTable LayDanhSachNhanVien() => dal.LayDanhSachNhanVien();
        public bool ThemNhanVien(NhanVienDTO nv) => dal.ThemNhanVien(nv);
        public bool XoaNhanVien(string maNV) => dal.XoaNhanVien(maNV);
        public bool SuaNhanVien(NhanVienDTO nv) => dal.SuaNhanVien(nv);
        public string LayMaTuDong() => dal.LayMaNVMoiNhat();

        public DataTable LayDanhSachChucVu() => dal.LayDanhSachChucVu();
        public DataTable LayDanhSachPhongBan() => dal.LayDanhSachPhongBan();

    }
}
