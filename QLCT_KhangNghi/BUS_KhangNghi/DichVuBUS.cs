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
    public class DichVuBUS
    {
        private DichVuDAL dal = new DichVuDAL();

        public DataTable LayDanhSachDichVu() => dal.LayDanhSachDichVu();

        public bool ThemDichVu(DichVuDTO dv) => dal.ThemDichVu(dv);

        public bool XoaDichVu(string maDV) => dal.XoaDichVu(maDV);

        public bool SuaDichVu(DichVuDTO dv) => dal.SuaDichVu(dv);

        public string LayMaTuDong() => dal.LayMaDVMoiNhat();
    }
}
