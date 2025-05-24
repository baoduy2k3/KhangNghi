using DTO_KhangNghi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_KhangNghi
{
    public class XuatHangDAL
    {
        private string connectionString = @"Data Source=MSI;Initial Catalog=KhangNghiDB;Persist Security Info=True;User ID=sa;Password=123";
        private SqlConnection conn;
        public List<XuatHangDTO> LayDanhSachPhieuXuatDTO()
        {
            List<XuatHangDTO> danhSachPhieuXuat = new List<XuatHangDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_LayDanhSachPhieuXuat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            XuatHangDTO phieuXuat = new XuatHangDTO();
                            phieuXuat.MaPhieuXuat = reader["MaPhieuXuat"].ToString();
                            phieuXuat.NgayXuat = Convert.ToDateTime(reader["NgayXuat"]);
                            phieuXuat.MaKH = reader["MaKH"].ToString();
                            phieuXuat.MaNV = reader["MaNV"].ToString();
                            phieuXuat.TongTien = Convert.ToDecimal(reader["TongTien"]);
                            danhSachPhieuXuat.Add(phieuXuat);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return danhSachPhieuXuat;
        }

        public bool ThemPhieuXuat(XuatHangDTO phieuXuat)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuXuat", phieuXuat.MaPhieuXuat);
                        cmd.Parameters.AddWithValue("@NgayXuat", phieuXuat.NgayXuat);
                        cmd.Parameters.AddWithValue("@MaKH", phieuXuat.MaKH);
                        cmd.Parameters.AddWithValue("@MaNV", phieuXuat.MaNV);
                        cmd.Parameters.AddWithValue("@TongTien", phieuXuat.TongTien); 

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (ThemPhieuXuatChinh): " + ex.Message);
                    throw;
                }
            }
        }

        public bool XoaPhieuXuatFull(string maPhieuXuat)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Xóa tất cả Chi Tiết Phiếu Xuất
                    using (SqlCommand cmdXoaChiTiet = new SqlCommand("sp_XoaTatCaChiTietPhieuXuat", conn, transaction))
                    {
                        cmdXoaChiTiet.CommandType = CommandType.StoredProcedure;
                        cmdXoaChiTiet.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                        cmdXoaChiTiet.ExecuteNonQuery();
                    }

                    // 2. Xóa Phiếu Xuất chính
                    using (SqlCommand cmdXoaPX = new SqlCommand("sp_XoaPhieuXuat", conn, transaction))
                    {
                        cmdXoaPX.CommandType = CommandType.StoredProcedure;
                        cmdXoaPX.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                        cmdXoaPX.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (XoaPhieuXuatFull): " + ex.Message);
                    throw;
                }
            }
        }

        public bool SuaThongTinPhieuXuatChinh(XuatHangDTO phieuXuatDaSua)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SuaPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuXuat", phieuXuatDaSua.MaPhieuXuat);
                        cmd.Parameters.AddWithValue("@NgayXuat", phieuXuatDaSua.NgayXuat);
                        cmd.Parameters.AddWithValue("@MaKH", phieuXuatDaSua.MaKH);
                        cmd.Parameters.AddWithValue("@MaNV", phieuXuatDaSua.MaNV);
                        cmd.Parameters.AddWithValue("@TongTien", phieuXuatDaSua.TongTien);
                        cmd.ExecuteNonQuery();
                        return true; 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (SuaThongTinPhieuXuatChinh): " + ex.Message);
                    throw;
                }
            }
        }

        public List<ChiTietXuatHangDTO> LayDanhSachChiTietTheoMaPX(string maPhieuXuat)
        {
            List<ChiTietXuatHangDTO> danhSachChiTiet = new List<ChiTietXuatHangDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LayChiTietPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ChiTietXuatHangDTO chiTiet = new ChiTietXuatHangDTO
                                {
                                    MaPhieuXuat = reader["MaPhieuXuat"].ToString(),
                                    MaSP = reader["MaSP"].ToString(),
                                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                    DonGia = Convert.ToDecimal(reader["DonGia"])
                                };

                                if (reader.GetOrdinal("ThanhTien") >= 0 && reader["ThanhTien"] != DBNull.Value)
                                {
                                    chiTiet.ThanhTien = Convert.ToDecimal(reader["ThanhTien"]);
                                }
                                else
                                {
                                    chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                                }
                                danhSachChiTiet.Add(chiTiet);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (LayDanhSachChiTietTheoMaPX): " + ex.Message);
                    throw;
                }
            }
            return danhSachChiTiet;
        }

        public bool ThemMotChiTiet(ChiTietXuatHangDTO chiTiet)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemChiTietPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuXuat", chiTiet.MaPhieuXuat);
                        cmd.Parameters.AddWithValue("@MaSP", chiTiet.MaSP);
                        cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                        cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool XoaMotChiTiet(string maPhieuXuat, string maSP)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaMotChiTietPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool SuaMotChiTiet(ChiTietXuatHangDTO chiTietDaSua)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SuaMotChiTietPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuXuat", chiTietDaSua.MaPhieuXuat);
                        cmd.Parameters.AddWithValue("@MaSP", chiTietDaSua.MaSP);
                        cmd.Parameters.AddWithValue("@SoLuongMoi", chiTietDaSua.SoLuong);
                        cmd.Parameters.AddWithValue("@DonGiaMoi", chiTietDaSua.DonGia);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL DAL (SuaMotChiTiet): " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (SuaMotChiTiet): " + ex.Message);
                    throw;
                }
            }
        }

        public List<XuatHangDTO> TimKiemPhieuXuatDAL(string tuKhoaChung)
        {
            List<XuatHangDTO> danhSachKetQua = new List<XuatHangDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_TimKiemPhieuXuat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TuKhoaChung", string.IsNullOrWhiteSpace(tuKhoaChung) ? (object)DBNull.Value : tuKhoaChung);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                XuatHangDTO phieuxuat = new XuatHangDTO
                                {
                                    MaPhieuXuat = reader["MaPhieuXuat"].ToString(),
                                    NgayXuat = Convert.ToDateTime(reader["NgayXuat"]),
                                    MaKH = reader["MaKH"].ToString(),
                                    MaNV = reader["MaNV"].ToString(),
                                    TongTien = Convert.ToDecimal(reader["TongTien"])
                                };
                                danhSachKetQua.Add(phieuxuat);
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
            }
            return danhSachKetQua;
        }
    }


}
