using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_KhangNghi;
using DTO_KhangNghi;

namespace BUS_KhangNghi
{
    public class XuatHangBUS
    {
        private XuatHangDAL dalXuatHang;
        private XuatHangDAL dalChiTietXuatHang;
        public XuatHangBUS()
        {
            dalXuatHang = new XuatHangDAL();
            dalChiTietXuatHang = new XuatHangDAL();
        }

        public List<XuatHangDTO> LayToanBoDanhSachPhieuXuat()
        {
            try
            {
                return dalXuatHang.LayDanhSachPhieuXuatDTO();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ThemThongTinPhieuXuat(XuatHangDTO phieuXuat)
        {
            if (string.IsNullOrWhiteSpace(phieuXuat.MaPhieuXuat))
            {
                Console.WriteLine("BUS Error: Mã Phiếu Xuất không được để trống.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuXuat.MaKH))
            {
                Console.WriteLine("BUS Error: Mã Khách Hàng không được để trống.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuXuat.MaNV))
            {
                Console.WriteLine("BUS Error: Mã Nhân Viên không được để trống.");
                return false;
            }
            try
            {
                return dalXuatHang.ThemPhieuXuat(phieuXuat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (ThemThongTinPhieuXuat): " + ex.Message);
                throw;
            }
        }

        public bool XoaPhieuXuat(string maPhieuXuat)
        {
            if (string.IsNullOrWhiteSpace(maPhieuXuat))
            {
                Console.WriteLine("BUS Error: Mã Phiếu Xuất không được để trống khi xóa.");
                return false; 
            }

            try
            {
                return dalXuatHang.XoaPhieuXuatFull(maPhieuXuat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (XoaPhieuXuat): " + ex.Message);
                throw;
            }
        }


        public bool SuaThongTinPhieuXuat(XuatHangDTO phieuXuatDaSua)
        {
            if (string.IsNullOrWhiteSpace(phieuXuatDaSua.MaPhieuXuat))
            {
                Console.WriteLine("BUS Error: Mã Phiếu Xuất không được để trống khi sửa.");
                return false; 
            }
            if (string.IsNullOrWhiteSpace(phieuXuatDaSua.MaKH))
            {
                Console.WriteLine("BUS Error: Mã Khách Hàng không được để trống khi sửa.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuXuatDaSua.MaNV))
            {
                Console.WriteLine("BUS Error: Mã Nhân Viên không được để trống khi sửa.");
                return false;
            }

            try
            {
                return dalXuatHang.SuaThongTinPhieuXuatChinh(phieuXuatDaSua);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (SuaThongTinPhieuXuat): " + ex.Message);
                throw;
            }
        }

        public List<ChiTietXuatHangDTO> LayChiTietPhieuXuat(string maPhieuXuat)
        {
            if (string.IsNullOrWhiteSpace(maPhieuXuat))
            {
                return new List<ChiTietXuatHangDTO>(); 
            }
            try
            {
                return dalXuatHang.LayDanhSachChiTietTheoMaPX(maPhieuXuat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (LayChiTietPhieuXuat): " + ex.Message);
                throw;
            }
        }

        public bool ThemChiTietVaoPhieuXuat(ChiTietXuatHangDTO chiTiet)
        {
            if (string.IsNullOrWhiteSpace(chiTiet.MaPhieuXuat))
            {
                Console.WriteLine("BUS Error: Mã phiếu xuất trong chi tiết không được để trống.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(chiTiet.MaSP))
            {
                Console.WriteLine("BUS Error: Mã sản phẩm trong chi tiết không được để trống.");
                return false;
            }
            if (chiTiet.SoLuong <= 0)
            {
                Console.WriteLine("BUS Error: Số lượng trong chi tiết phải lớn hơn 0.");
                return false;
            }
            if (chiTiet.DonGia < 0)
            {
                Console.WriteLine("BUS Error: Đơn giá trong chi tiết không được âm.");
                return false;
            }
            try
            {
                return dalChiTietXuatHang.ThemMotChiTiet(chiTiet);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Lỗi SQL BUS (ThemChiTietVaoPhieuXuat): " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (ThemChiTietVaoPhieuXuat): " + ex.Message);
                throw;
            }
        }

        public bool XoaChiTietKhoiPhieuXuat(string maPhieuXuat, string maSP)
        {
            if (string.IsNullOrWhiteSpace(maPhieuXuat) || string.IsNullOrWhiteSpace(maSP))
            {
                Console.WriteLine("BUS Error: Mã phiếu xuất và Mã sản phẩm không được để trống khi xóa chi tiết.");
                return false;
            }

            try
            {
                return dalChiTietXuatHang.XoaMotChiTiet(maPhieuXuat, maSP);
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SuaChiTietTrongPhieuXuat(ChiTietXuatHangDTO chiTietDaSua)
        {
            if (string.IsNullOrWhiteSpace(chiTietDaSua.MaPhieuXuat) || string.IsNullOrWhiteSpace(chiTietDaSua.MaSP))
            {
                Console.WriteLine("BUS Error: Mã phiếu xuất và Mã sản phẩm không được để trống khi sửa chi tiết.");
                return false;
            }
            if (chiTietDaSua.SoLuong <= 0)
            {
                Console.WriteLine("BUS Error: Số lượng mới trong chi tiết phải lớn hơn 0.");
                return false;
            }
            if (chiTietDaSua.DonGia < 0)
            {
                Console.WriteLine("BUS Error: Đơn giá mới trong chi tiết không được âm.");
                return false;
            }

            try
            {
                return dalChiTietXuatHang.SuaMotChiTiet(chiTietDaSua);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Lỗi SQL BUS (SuaChiTietTrongPhieuNhap): " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (SuaChiTietTrongPhieuNhap): " + ex.Message);
                throw;
            }
        }

        public List<XuatHangDTO> TimKiemPhieuXuat(string tuKhoaChung)
        {
            try
            {
                return dalXuatHang.TimKiemPhieuXuatDAL(tuKhoaChung);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
