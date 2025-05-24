using DevExpress.XtraPrinting.Native.WebClientUIControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GUI_KhangNghi
{
    public class DiaChiAPIHelper
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<Tinh>> GetTinhThanh()
        {
            string json = await client.GetStringAsync("https://provinces.open-api.vn/api/?depth=1");
            return JsonConvert.DeserializeObject<List<Tinh>>(json);
        }

        public async Task<List<Huyen>> GetQuanHuyen(string maTinh)
        {
            string json = await client.GetStringAsync($"https://provinces.open-api.vn/api/p/{maTinh}?depth=2");
            var tinh = JsonConvert.DeserializeObject<Tinh>(json);
            return tinh.districts;
        }

        public async Task<List<Phuong>> GetXaPhuong(string maHuyen)
        {
            string json = await client.GetStringAsync($"https://provinces.open-api.vn/api/d/{maHuyen}?depth=2");
            var huyen = JsonConvert.DeserializeObject<Huyen>(json);
            return huyen.wards;
        }

        public class Tinh
        {
            public string code { get; set; }
            public string name { get; set; }
            public List<Huyen> districts { get; set; }
        }

        public class Huyen
        {
            public string code { get; set; }
            public string name { get; set; }
            public List<Phuong> wards { get; set; }
        }

        public class Phuong
        {
            public string code { get; set; }
            public string name { get; set; }
        }
    }
}
