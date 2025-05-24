using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_KhangNghi
{
    public class XuatHangDTO
    {
        public string MaPhieuXuat { get; set; }
        public DateTime NgayXuat { get; set; }
        public string MaKH { get; set; }
        public string MaNV { get; set; }
        public decimal TongTien { get; set; }

        public XuatHangDTO() { }

        public XuatHangDTO(string maPhieuXuat, DateTime ngayXuat, string maKH, string maNV, decimal tongTien)
        {
            MaPhieuXuat = maPhieuXuat;
            NgayXuat = ngayXuat;
            MaKH = maKH;
            MaNV = maNV;
            TongTien = tongTien;
        }
    }
}
