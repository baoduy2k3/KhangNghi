using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_KhangNghi
{
    public class TonKhoDTO
    {
        public string MaTonKho { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string DonViTinh { get; set; }
        public DateTime Ngay { get; set; } 
        public int SoLuongTonDauKy { get; set; }
        public int SoLuongTonCuoiKy { get; set; }
        public int SoLuongTon { get; set; } 

        public TonKhoDTO() { }

        public TonKhoDTO(string maTonKho, string maSP, string tenSP, string donViTinh, DateTime ngay, int slTonDauKy, int slTonCuoiKy, int slTon)
        {
            MaTonKho = maTonKho;
            MaSP = maSP;
            TenSP = tenSP;
            DonViTinh = donViTinh;
            Ngay = ngay;
            SoLuongTonDauKy = slTonDauKy;
            SoLuongTonCuoiKy = slTonCuoiKy;
            SoLuongTon = slTon;
        }
    }
}
