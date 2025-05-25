using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_KhangNghi;
namespace GUI_KhangNghi
{
    public partial class SaoLuuDuLieu : Form
    {
        private DatabaseUtilityBUS busDatabaseUtil;
        public SaoLuuDuLieu()
        {
            InitializeComponent();
            busDatabaseUtil = new DatabaseUtilityBUS();
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnChonThuMuc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục để lưu file sao lưu";
                if (!string.IsNullOrWhiteSpace(txtDuongDan.Text) && Directory.Exists(txtDuongDan.Text))
                {
                    folderDialog.SelectedPath = txtDuongDan.Text;
                }
                else
                {
                    folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtDuongDan.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnThucHienSaoLuu_Click(object sender, EventArgs e)
        {
            string selectedFolderPath = txtDuongDan.Text.Trim();

            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                MessageBox.Show("Vui lòng chọn một thư mục để lưu file sao lưu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChonThuMuc.Focus();
                return;
            }

            if (!Directory.Exists(selectedFolderPath))
            {
                MessageBox.Show($"Thư mục '{selectedFolderPath}' không tồn tại. Vui lòng chọn một thư mục hợp lệ.", "Thư mục không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnChonThuMuc.Focus();
                return;
            }

            string databaseName = "KhangNghiDB";
            string fileName = $"{databaseName}_Full_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string fullBackupFilePath = Path.Combine(selectedFolderPath, fileName);

            DialogResult confirmBackup = MessageBox.Show(
                $"Bạn có chắc chắn muốn sao lưu CSDL {databaseName} vào:\n{fullBackupFilePath}\n\n" +
                "LƯU Ý: Nếu file sao lưu đã tồn tại, nó sẽ được ghi đè.",
                "Xác nhận Sao Lưu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmBackup == DialogResult.Yes)
            {
                try
                {
                    string ketQua = busDatabaseUtil.PerformBackupOperation(fullBackupFilePath);

                    if (ketQua.StartsWith("Sao lưu CSDL") && ketQua.Contains("thành công"))
                    {
                        MessageBox.Show(ketQua, "Sao Lưu Hoàn Tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(ketQua, "Thông Báo Sao Lưu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình sao lưu:\n\n{ex.Message}",
                                    "Lỗi Nghiêm Trọng",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnChonThuMucPhucHoi_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Backup Files (*.bak)|*.bak|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Chọn file sao lưu để phục hồi";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDuongDanPhucHoi.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnThucHienPhucHoi_Click(object sender, EventArgs e)
        {
            string backupFilePath = txtDuongDanPhucHoi.Text;

            if (string.IsNullOrWhiteSpace(backupFilePath))
            {
                MessageBox.Show("Vui lòng chọn một file sao lưu (.bak) để phục hồi.", "Chưa chọn file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(backupFilePath))
            {
                MessageBox.Show($"File sao lưu không tồn tại tại đường dẫn đã chọn:\n{backupFilePath}", "File không tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                $"Bạn có chắc chắn muốn phục hồi cơ sở dữ liệu từ file:\n{backupFilePath}\n\n" +
                "THAO TÁC NÀY SẼ GHI ĐÈ LÊN CƠ SỞ DỮ LIỆU HIỆN TẠI VÀ KHÔNG THỂ HOÀN TÁC!\n" +
                "Hãy đảm bảo bạn đã sao lưu dữ liệu quan trọng (nếu cần) trước khi tiếp tục.",
                "Xác nhận Phục Hồi CSDL",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmation == DialogResult.Yes)
            {
                string resultMessage = string.Empty;
                resultMessage = busDatabaseUtil.PerformRestoreOperation(backupFilePath);
                if (resultMessage.ToLower().Contains("thành công"))
                {
                    MessageBox.Show(resultMessage, "Phục hồi thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(resultMessage, "Phục hồi thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
