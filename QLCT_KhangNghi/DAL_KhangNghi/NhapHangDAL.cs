using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_KhangNghi;

namespace DAL_KhangNghi
{
    public class NhapHangDAL
    {
        private string connectionString = @"Data Source=MSI;Initial Catalog=KhangNghiDB;Persist Security Info=True;User ID=sa;Password=123";
        private SqlConnection conn;
        public List<NhapHangDTO> LayDanhSachPhieuNhapDTO()
        {
            List<NhapHangDTO> danhSachPhieuNhap = new List<NhapHangDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_LayDanhSachPhieuNhap", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NhapHangDTO phieuNhap = new NhapHangDTO();
                            phieuNhap.MaPhieuNhap = reader["MaPhieuNhap"].ToString();
                            phieuNhap.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                            phieuNhap.MaNCC = reader["MaNCC"].ToString();
                            phieuNhap.MaNV = reader["MaNV"].ToString();
                            phieuNhap.TongTien = Convert.ToDecimal(reader["TongTien"]);

                            danhSachPhieuNhap.Add(phieuNhap);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL DAL: " + ex.Message);
                    throw; 
                }
            }
            return danhSachPhieuNhap;
        }

        public bool ThemPhieuNhapChinh(NhapHangDTO phieuNhap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmdPN = new SqlCommand("sp_ThemPhieuNhap", conn))
                    {
                        cmdPN.CommandType = CommandType.StoredProcedure;
                        cmdPN.Parameters.AddWithValue("@MaPhieuNhap", phieuNhap.MaPhieuNhap);
                        cmdPN.Parameters.AddWithValue("@NgayNhap", phieuNhap.NgayNhap);
                        cmdPN.Parameters.AddWithValue("@MaNCC", phieuNhap.MaNCC);
                        cmdPN.Parameters.AddWithValue("@MaNV", phieuNhap.MaNV);
                        cmdPN.Parameters.AddWithValue("@TongTien", phieuNhap.TongTien); 

                        int rowsAffected = cmdPN.ExecuteNonQuery();
                        return rowsAffected > 0; 
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

        public bool XoaPhieuNhapFull(string maPhieuNhap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Xóa tất cả Chi Tiết Phiếu Nhập
                    using (SqlCommand cmdXoaChiTiet = new SqlCommand("sp_XoaTatCaChiTietPhieuNhap", conn, transaction))
                    {
                        cmdXoaChiTiet.CommandType = CommandType.StoredProcedure;
                        cmdXoaChiTiet.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        cmdXoaChiTiet.ExecuteNonQuery();
                    }

                    // 2. Xóa Phiếu Nhập chính
                    using (SqlCommand cmdXoaPN = new SqlCommand("sp_XoaPhieuNhap", conn, transaction))
                    {
                        cmdXoaPN.CommandType = CommandType.StoredProcedure;
                        cmdXoaPN.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        int rowsAffected = cmdXoaPN.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else 
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL DAL (XoaPhieuNhapFull): " + ex.Message);
                    throw; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (XoaPhieuNhapFull): " + ex.Message);
                    throw;
                }
            }
        }

        public bool SuaThongTinPhieuNhapChinh(NhapHangDTO phieuNhapDaSua)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmdSuaPN = new SqlCommand("sp_SuaPhieuNhap", conn))
                    {
                        cmdSuaPN.CommandType = CommandType.StoredProcedure;
                        cmdSuaPN.Parameters.AddWithValue("@MaPhieuNhap", phieuNhapDaSua.MaPhieuNhap);
                        cmdSuaPN.Parameters.AddWithValue("@NgayNhap", phieuNhapDaSua.NgayNhap);
                        cmdSuaPN.Parameters.AddWithValue("@MaNCC", phieuNhapDaSua.MaNCC);
                        cmdSuaPN.Parameters.AddWithValue("@MaNV", phieuNhapDaSua.MaNV);
                        cmdSuaPN.Parameters.AddWithValue("@TongTien", phieuNhapDaSua.TongTien);

                        int rowsAffected = cmdSuaPN.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL DAL (SuaThongTinPhieuNhapChinh): " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi chung DAL (SuaThongTinPhieuNhapChinh): " + ex.Message);
                    throw;
                }
            }
        }
        public List<ChiTietNhapHangDTO> LayDanhSachChiTietTheoMaPN(string maPhieuNhap)
        {
            List<ChiTietNhapHangDTO> danhSachChiTiet = new List<ChiTietNhapHangDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_LayChiTietPhieuNhap", conn); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChiTietNhapHangDTO chiTiet = new ChiTietNhapHangDTO();
                            chiTiet.MaPhieuNhap = reader["MaPhieuNhap"].ToString(); 
                            chiTiet.MaSP = reader["MaSP"].ToString();
                            chiTiet.SoLuong = Convert.ToInt32(reader["SoLuong"]);
                            chiTiet.DonGia = Convert.ToDecimal(reader["DonGia"]);

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
                catch (SqlException ex)
                {
            
                    Console.WriteLine("Lỗi SQL DAL (ChiTietNhapHang): " + ex.Message);
                    throw; 
                }
            }
            return danhSachChiTiet;
        }

        public bool ThemMotChiTiet(ChiTietNhapHangDTO chiTiet)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemChiTietPhieuNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", chiTiet.MaPhieuNhap);
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

        public bool XoaMotChiTiet(string maPhieuNhap, string maSP)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaMotChiTietPhieuNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
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

        public bool SuaMotChiTiet(ChiTietNhapHangDTO chiTietDaSua)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SuaMotChiTietPhieuNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", chiTietDaSua.MaPhieuNhap);
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

        public List<NhapHangDTO> TimKiemPhieuNhapDAL(string tuKhoaChung) 
        {
            List<NhapHangDTO> danhSachKetQua = new List<NhapHangDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_TimKiemPhieuNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TuKhoaChung", string.IsNullOrWhiteSpace(tuKhoaChung) ? (object)DBNull.Value : tuKhoaChung);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NhapHangDTO phieuNhap = new NhapHangDTO
                                {
                                    MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                    NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                                    MaNCC = reader["MaNCC"].ToString(),
                                    MaNV = reader["MaNV"].ToString(),
                                    TongTien = Convert.ToDecimal(reader["TongTien"])
                                };
                                danhSachKetQua.Add(phieuNhap);
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
