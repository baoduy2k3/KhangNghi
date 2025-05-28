using DTO_KhangNghi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_KhangNghi
{
    public class NhaCungCapDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["KhangNghiDB"].ConnectionString;

        public DataTable LayDanhSachNhaCungCap()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LayNhaCungCap", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        public bool ThemNhaCungCap(NhaCungCapDTO ncc)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ThemNhaCungCap", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNCC", ncc.MaNCC);
                cmd.Parameters.AddWithValue("@TenNCC", ncc.TenNCC);
                cmd.Parameters.AddWithValue("@Email", ncc.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", ncc.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaNhaCungCap(string maNCC)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_XoaNhaCungCap", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaNhaCungCap(NhaCungCapDTO ncc)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SuaNhaCungCap", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNCC", ncc.MaNCC);
                cmd.Parameters.AddWithValue("@TenNCC", ncc.TenNCC);
                cmd.Parameters.AddWithValue("@Email", ncc.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", ncc.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string LayMaNCCMoiNhat()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 MaNCC FROM NhaCungCap ORDER BY MaNCC DESC", conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string maCu = result.ToString().Substring(2); // Giả sử mã dạng "NC001"
                    int so = int.Parse(maCu) + 1;
                    return $"NC{so:D3}";
                }
                return "NC001";
            }
       }
    
}
}
