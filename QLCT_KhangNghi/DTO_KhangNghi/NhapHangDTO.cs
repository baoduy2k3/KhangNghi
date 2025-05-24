using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_KhangNghi
{
    public class NhapHangDTO
    {
        public string MaPhieuNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string MaNCC { get; set; } 
        public string MaNV { get; set; } 
        public decimal TongTien { get; set; }

        public NhapHangDTO() { }

        public NhapHangDTO(string maPhieuNhap, DateTime ngayNhap, string maNCC, string maNV, decimal tongTien)
        {
            MaPhieuNhap = maPhieuNhap;
            NgayNhap = ngayNhap;
            MaNCC = maNCC;
            MaNV = maNV;
            TongTien = tongTien;
        }
    }
}
