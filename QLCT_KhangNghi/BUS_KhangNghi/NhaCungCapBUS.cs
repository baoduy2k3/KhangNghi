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
    public class NhaCungCapBUS
    {
        private NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataTable LayDanhSachNhaCungCap() => dal.LayDanhSachNhaCungCap();
        public bool ThemNhaCungCap(NhaCungCapDTO ncc) => dal.ThemNhaCungCap(ncc);
        public bool XoaNhaCungCap(string maNCC) => dal.XoaNhaCungCap(maNCC);
        public bool SuaNhaCungCap(NhaCungCapDTO ncc) => dal.SuaNhaCungCap(ncc);
        public string LayMaTuDong() => dal.LayMaNCCMoiNhat();
    }
}
