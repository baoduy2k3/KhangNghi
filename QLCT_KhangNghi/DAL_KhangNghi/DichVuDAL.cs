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
    public class DichVuDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["KhangNghiDB"].ConnectionString;

        public DataTable LayDanhSachDichVu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachDichVu", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        public bool ThemDichVu(DichVuDTO dv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ThemDichVu", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);
                cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
                cmd.Parameters.AddWithValue("@GiaDichVu", dv.GiaDichVu);
                cmd.Parameters.AddWithValue("@MoTa", dv.MoTa);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaDichVu(string maDV)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_XoaDichVu", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaDV", maDV);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaDichVu(DichVuDTO dv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SuaDichVu", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);
                cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
                cmd.Parameters.AddWithValue("@GiaDichVu", dv.GiaDichVu);
                cmd.Parameters.AddWithValue("@MoTa", dv.MoTa);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string LayMaDVMoiNhat()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 MaDV FROM DichVu ORDER BY MaDV DESC", conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string maCu = result.ToString().Substring(2); // Bỏ "DV"
                    int so = int.Parse(maCu) + 1;
                    return $"DV{so:D3}";
                }
                return "DV001";
            }
        }
    }
}
