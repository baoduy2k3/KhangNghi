using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DTO_KhangNghi;

namespace DAL_KhangNghi
{
    public class NhanVienDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["KhangNghiDB"].ConnectionString;

    public DataTable LayDanhSachNhanVien()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_LayNhanVien", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }
    }

    public bool ThemNhanVien(NhanVienDTO nv)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_ThemNhanVien", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
            cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
            cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
            cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
            cmd.Parameters.AddWithValue("@Email", nv.Email);
            cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
            cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
            cmd.Parameters.AddWithValue("@MaChucVu", nv.MaChucVu);
            cmd.Parameters.AddWithValue("@MaPB", nv.MaPB);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool XoaNhanVien(string maNV)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_XoaNhanVien", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaNV", maNV);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool SuaNhanVien(NhanVienDTO nv)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_SuaNhanVien", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
            cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
            cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
            cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
            cmd.Parameters.AddWithValue("@Email", nv.Email);
            cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
            cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
            cmd.Parameters.AddWithValue("@MaChucVu", nv.MaChucVu);
            cmd.Parameters.AddWithValue("@MaPB", nv.MaPB);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public string LayMaNVMoiNhat()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 MaNV FROM NhanVien ORDER BY MaNV DESC", conn))
        {
            conn.Open();
            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                string maCu = result.ToString().Substring(2);
                int so = int.Parse(maCu) + 1;
                return $"NV{so:D3}";
            }
            return "NV001";
        }
    }

        public DataTable LayDanhSachChucVu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaChucVu, TenChucVu FROM ChucVu", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable LayDanhSachPhongBan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaPB, TenPB FROM PhongBan", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }

}
