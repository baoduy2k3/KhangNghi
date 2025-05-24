using DAL_KhangNghi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_KhangNghi;
namespace BUS_KhangNghi
{
    public class TonKhoBUS
    {
        private TonKhoDAL _tonKhoDAL;

        public TonKhoBUS()
        {
            _tonKhoDAL = new TonKhoDAL();
        }

        public List<TonKhoDTO> LayDanhSachTonKho(int thang, int nam)
        {
            if (thang < 1 || thang > 12)
            {
                throw new ArgumentOutOfRangeException("Tháng không hợp lệ.");
            }
            if (nam < 1900 || nam > System.DateTime.Now.Year + 5) 
            {
                throw new ArgumentOutOfRangeException("Năm không hợp lệ.");
            }
            return _tonKhoDAL.LayDanhSachTonKho(thang, nam);
        }
    }
}
