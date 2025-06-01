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
    public class SanPhamDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["KhangNghiDB"].ConnectionString;

        public DataTable LayDanhSachSanPham()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LaySanPham", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        public bool ThemSanPham(SanPhamDTO sp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ThemSanPham", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@DonViTinh", sp.DonViTinh);
                cmd.Parameters.AddWithValue("@GiaBan", sp.GiaBan);
                cmd.Parameters.AddWithValue("@MaLoai", sp.MaLoai);
                cmd.Parameters.AddWithValue("@MoTa", sp.MoTa);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaSanPham(SanPhamDTO sp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SuaSanPham", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@DonViTinh", sp.DonViTinh);
                cmd.Parameters.AddWithValue("@GiaBan", sp.GiaBan);
                cmd.Parameters.AddWithValue("@MaLoai", sp.MaLoai);
                cmd.Parameters.AddWithValue("@MoTa", sp.MoTa);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaSanPham(string maSP)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_XoaSanPham", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSP", maSP);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string LayMaSanPhamMoiNhat()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 MaSP FROM SanPham ORDER BY MaSP DESC", conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string maCu = result.ToString().Substring(2); // giả sử mã như SP001
                    int so = int.Parse(maCu) + 1;
                    return $"SP{so:D3}";
                }
                return "SP001";
            }
        }

        public DataTable LayDanhSachLoaiSanPham()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachLoaiSanPham", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
    }
}
