using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_KhangNghi
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // TODO: Thêm kiểm tra tài khoản/mật khẩu

            frmMainQuanLy frmMain = new frmMainQuanLy();
            frmMain.Show();
            this.Hide(); // Ẩn form đăng nhập

            // Đăng ký sự kiện khi form chính đóng thì form đăng nhập cũng sẽ đóng
            frmMain.FormClosed += (s, args) => this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
