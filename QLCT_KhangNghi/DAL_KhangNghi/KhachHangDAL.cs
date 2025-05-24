using DTO_KhangNghi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DAL_KhangNghi
{
    public class KhachHangDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["KhangNghiDB"].ConnectionString;

        public DataTable LayDanhSachKhachHang()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LayKhachHang", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable LayDanhSachLoaiKH()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaLoaiKH, TenLoaiKH FROM LoaiKhachHang";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ThemKhachHang", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaKH", kh.MaKH);
                cmd.Parameters.AddWithValue("@TenKH", kh.TenKH);
                cmd.Parameters.AddWithValue("@MaLoaiKH", kh.MaLoaiKH);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool SuaKhachHang(KhachHangDTO kh)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SuaKhachHang", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaKH", kh.MaKH);
                cmd.Parameters.AddWithValue("@TenKH", kh.TenKH);
                cmd.Parameters.AddWithValue("@MaLoaiKH", kh.MaLoaiKH);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool XoaKhachHang(string maKH)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_XoaKhachHang", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaKH", maKH);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
