using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_KhangNghi
{
    public class ChiTietNhapHangDTO
    {
        public string MaPhieuNhap { get; set; } 
        public string MaSP { get; set; }      
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        public ChiTietNhapHangDTO() { }

        public ChiTietNhapHangDTO(string maPhieuNhap, string maSP, int soLuong, decimal donGia, decimal thanhTien)
        {
            MaPhieuNhap = maPhieuNhap;
            MaSP = maSP;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = soLuong * donGia;
        }
    }
}
