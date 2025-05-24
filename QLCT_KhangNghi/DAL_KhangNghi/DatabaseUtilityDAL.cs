using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DAL_KhangNghi
{
    public class DatabaseUtilityDAL
    {
        private string connectionString = @"Data Source=MSI;Initial Catalog=KhangNghiDB;Persist Security Info=True;User ID=sa;Password=123";
        private SqlConnection conn;
        public void BackupDatabase(string databaseName, string fullBackupFilePath)
        {
            string backupQuery = $"BACKUP DATABASE [{databaseName}] TO DISK = N'{fullBackupFilePath}' " +
                                 $"WITH FORMAT, NAME = N'{databaseName} - Full Backup {DateTime.Now:yyyy-MM-dd HH:mm:ss}'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(backupQuery, conn))
                {

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Lỗi sao lưu CSDL: {ex.Message} (SQL Mã lỗi: {ex.Number}). Đảm bảo đường dẫn hợp lệ và tài khoản dịch vụ SQL Server có quyền ghi vào thư mục đó.", ex);
                    }
                }
            }
        }

        public void RestoreDatabase(string databaseName, string backupFilePath)
        {
            SqlConnectionStringBuilder masterCsb = new SqlConnectionStringBuilder(this.connectionString);
            masterCsb.InitialCatalog = "master";
            string masterConnectionString = masterCsb.ConnectionString;

            using (SqlConnection conn = new SqlConnection(masterConnectionString))
            {
                conn.Open();

                string alterQuery = $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                using (SqlCommand alterCmd = new SqlCommand(alterQuery, conn))
                {
                    alterCmd.ExecuteNonQuery();
                }

                string restoreQuery = $"RESTORE DATABASE [{databaseName}] FROM DISK = N'{backupFilePath}' WITH REPLACE, RECOVERY";
                using (SqlCommand restoreCmd = new SqlCommand(restoreQuery, conn))
                {
                    restoreCmd.ExecuteNonQuery();

                }

                SetDatabaseMultiUser(conn, databaseName, "after successful restore");
            }
        }

        private void SetDatabaseMultiUser(SqlConnection masterConnection, string databaseName, string context)
        {
            if (masterConnection.State != System.Data.ConnectionState.Open)
            {
                return;
            }

            string multiUserQuery = $"ALTER DATABASE [{databaseName}] SET MULTI_USER";
            using (SqlCommand multiUserCmd = new SqlCommand(multiUserQuery, masterConnection))
            {
                multiUserCmd.ExecuteNonQuery();

            }
        }
    }


}