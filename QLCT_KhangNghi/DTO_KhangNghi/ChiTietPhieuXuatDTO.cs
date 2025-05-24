using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_KhangNghi
{
    public class ChiTietXuatHangDTO
    {
        public string MaPhieuXuat { get; set; } 
        public string MaSP { get; set; }        
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        public ChiTietXuatHangDTO() { }

        public ChiTietXuatHangDTO(string maPhieuXuat, string maSP, int soLuong, decimal donGia, decimal thanhTien)
        {
            MaPhieuXuat = maPhieuXuat;
            MaSP = maSP;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = soLuong * donGia;
        }
    }
}
