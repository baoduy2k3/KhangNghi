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
            using (SqlCommand cmd = new SqlCommand("SELECT MaNCC FROM NhaCungCap", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                int max = 0;
                while (reader.Read())
                {
                    string ma = reader.GetString(0); // ví dụ "NCC01"
                    if (ma.StartsWith("NCC"))
                    {
                        string soStr = ma.Substring(3); // phần số
                        if (int.TryParse(soStr, out int so) && so <= 99)
                        {
                            max = Math.Max(max, so);
                        }
                    }
                }

                int maMoi = max + 1;
                if (maMoi > 99)
                    throw new Exception("Đã vượt quá giới hạn mã NCC99 với CHAR(5).");

                // Không dùng D3 vì nó thành NCC011. Dùng D2 để giữ mã 5 ký tự.
                return $"NCC{maMoi.ToString("D2")}";
            }

        }

    }
}
