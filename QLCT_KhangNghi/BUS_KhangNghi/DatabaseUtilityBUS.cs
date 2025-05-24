using DAL_KhangNghi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BUS_KhangNghi
{
    public class DatabaseUtilityBUS
    {
        private DatabaseUtilityDAL dalDatabaseUtility = new DatabaseUtilityDAL();
        private string databaseNameToBackup = "KhangNghiDB";

        public string PerformBackupOperation(string fullBackupFilePath)
        {
            if (string.IsNullOrWhiteSpace(fullBackupFilePath))
            {
                return "Đường dẫn file sao lưu không được cung cấp hoặc không hợp lệ.";
            }

            try
            {
                dalDatabaseUtility.BackupDatabase(databaseNameToBackup, fullBackupFilePath);
                string successMessage = $"Sao lưu CSDL '{databaseNameToBackup}' thành công!\nĐã lưu tại: {fullBackupFilePath}";
                Console.WriteLine(successMessage);
                return successMessage;
            }
            catch (Exception ex) 
            {
                return $"Sao lưu thất bại: {ex.Message}";
            }
        }

        public string PerformRestoreOperation(string backupFilePath)
        {
            if (string.IsNullOrWhiteSpace(backupFilePath))
            {
                return "Đường dẫn file phục hồi không được cung cấp hoặc không hợp lệ.";
            }

            if (!File.Exists(backupFilePath))
            {
                return $"File sao lưu không tồn tại tại đường dẫn: {backupFilePath}";
            }

            try
            {
                Console.WriteLine($"Bắt đầu quá trình phục hồi CSDL '{databaseNameToBackup}' từ file: {backupFilePath}");
                dalDatabaseUtility.RestoreDatabase(databaseNameToBackup, backupFilePath);
                string successMessage = $"Phục hồi CSDL '{databaseNameToBackup}' thành công từ file:\n{backupFilePath}";
                Console.WriteLine(successMessage);
                return successMessage;
            }
            catch (Exception ex)
            {
                return $"Phục hồi thất bại: {ex.Message}";
            }
        }
    }
}
