using DTO_KhangNghi;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_KhangNghi
{
    public class TonKhoDAL
    {
        private string connectionString = @"Data Source=MSI;Initial Catalog=KhangNghiDB;Persist Security Info=True;User ID=sa;Password=123";
        private SqlConnection conn;
        public List<TonKhoDTO> LayDanhSachTonKho(int thang, int nam)
        {
            List<TonKhoDTO> danhSachTonKho = new List<TonKhoDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_LayTonKhoTheoThang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TonKhoDTO tk = new TonKhoDTO();
                                tk.MaTonKho = reader["MaTonKho"].ToString();
                                tk.MaSP = reader["MaSP"].ToString();
                                tk.TenSP = reader["TenSP"].ToString();
                                tk.DonViTinh = reader["DonViTinh"].ToString();
                                tk.Ngay = Convert.ToDateTime(reader["Ngay"]);
                                tk.SoLuongTonDauKy = Convert.ToInt32(reader["SoLuongTonDauKy"]);
                                tk.SoLuongTonCuoiKy = Convert.ToInt32(reader["SoLuongTonCuoiKy"]);
                                tk.SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]);
                                danhSachTonKho.Add(tk);
                            }
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
            return danhSachTonKho;
        }
    }
}
