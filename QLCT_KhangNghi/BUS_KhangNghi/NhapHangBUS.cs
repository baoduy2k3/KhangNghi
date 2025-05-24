using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_KhangNghi;
using DTO_KhangNghi;
using System.Data;
using System.Data.SqlClient;

namespace BUS_KhangNghi
{
    public class NhapHangBUS
    {
        private NhapHangDAL dalNhapHang;
        private NhapHangDAL dalChiTietNhapHang;

        public NhapHangBUS()
        {
            dalNhapHang = new NhapHangDAL();
            dalChiTietNhapHang = new NhapHangDAL();
        }

        public List<NhapHangDTO> LayToanBoDanhSachPhieuNhap()
        {
            try
            {
                return dalNhapHang.LayDanhSachPhieuNhapDTO();
            }
            catch (Exception)
            {
                throw; 
            }
        }

        public bool ThemThongTinPhieuNhap(NhapHangDTO phieuNhap)
        {
            if (string.IsNullOrWhiteSpace(phieuNhap.MaPhieuNhap))
            {
                Console.WriteLine(" Mã phiếu nhập không được để trống.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuNhap.MaNCC))
            {
                Console.WriteLine(" Mã nhà cung cấp không được để trống.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuNhap.MaNV))
            {
                Console.WriteLine(" Mã nhân viên không được để trống.");
                return false;
            }

            try
            {
                return dalNhapHang.ThemPhieuNhapChinh(phieuNhap);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (ThemThongTinPhieuNhap): " + ex.Message);
                return false;
            }
        }

        public bool XoaPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
            {
                Console.WriteLine("BUS Error: Mã phiếu nhập không được để trống khi xóa.");
                return false;
            }


            try
            {
                return dalNhapHang.XoaPhieuNhapFull(maPhieuNhap);
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine("Lỗi SQL BUS (XoaPhieuNhap): " + sqlEx.Message);
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (XoaPhieuNhap): " + ex.Message);
                throw;
            }
        }

        public bool SuaThongTinPhieuNhap(NhapHangDTO phieuNhapDaSua)
        {
            if (string.IsNullOrWhiteSpace(phieuNhapDaSua.MaPhieuNhap))
            {
                Console.WriteLine("BUS Error: Mã phiếu nhập không được để trống khi sửa.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuNhapDaSua.MaNCC))
            {
                Console.WriteLine("BUS Error: Mã nhà cung cấp không được để trống khi sửa.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phieuNhapDaSua.MaNV))
            {
                Console.WriteLine("BUS Error: Mã nhân viên không được để trống khi sửa.");
                return false;
            }
            try
            {
                return dalNhapHang.SuaThongTinPhieuNhapChinh(phieuNhapDaSua);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Lỗi SQL BUS (SuaThongTinPhieuNhap): " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (SuaThongTinPhieuNhap): " + ex.Message);
                throw;
            }
        }
        public List<ChiTietNhapHangDTO> LayChiTietTheoMaPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
            {
                return new List<ChiTietNhapHangDTO>(); 
            }

            try
            {
                return dalNhapHang.LayDanhSachChiTietTheoMaPN(maPhieuNhap);
            }
            catch (Exception)
            {
                return new List<ChiTietNhapHangDTO>(); 
            }
        }

        public decimal TinhTongTienChiTietPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
            {
                return 0;
            }

            decimal tongTien = 0;
            try
            {
                List<ChiTietNhapHangDTO> danhSachChiTiet = LayChiTietTheoMaPhieuNhap(maPhieuNhap); 

                if (danhSachChiTiet != null && danhSachChiTiet.Any())
                {
                    tongTien = danhSachChiTiet.Sum(ct => ct.ThanhTien);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return tongTien;
        }

        public bool ThemChiTietVaoPhieuNhap(ChiTietNhapHangDTO chiTiet)
        {
            if (string.IsNullOrWhiteSpace(chiTiet.MaPhieuNhap))
            {
                Console.WriteLine("BUS Error: Mã phiếu nhập trong chi tiết không được để trống.");
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
                return dalChiTietNhapHang.ThemMotChiTiet(chiTiet);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Lỗi SQL BUS (ThemChiTietVaoPhieuNhap): " + sqlEx.Message);
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BUS (ThemChiTietVaoPhieuNhap): " + ex.Message);
                throw;
            }
        }

        public bool XoaChiTietKhoiPhieuNhap(string maPhieuNhap, string maSP)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap) || string.IsNullOrWhiteSpace(maSP))
            {
                Console.WriteLine("BUS Error: Mã phiếu nhập và Mã sản phẩm không được để trống khi xóa chi tiết.");
                return false;
            }

            try
            {
                return dalChiTietNhapHang.XoaMotChiTiet(maPhieuNhap, maSP);
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

        public bool SuaChiTietTrongPhieuNhap(ChiTietNhapHangDTO chiTietDaSua)
        {
            if (string.IsNullOrWhiteSpace(chiTietDaSua.MaPhieuNhap) || string.IsNullOrWhiteSpace(chiTietDaSua.MaSP))
            {
                Console.WriteLine("BUS Error: Mã phiếu nhập và Mã sản phẩm không được để trống khi sửa chi tiết.");
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
                return dalChiTietNhapHang.SuaMotChiTiet(chiTietDaSua);
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

        public List<NhapHangDTO> TimKiemPhieuNhap(string tuKhoaChung) 
        {
            try
            {
                return dalNhapHang.TimKiemPhieuNhapDAL(tuKhoaChung);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
