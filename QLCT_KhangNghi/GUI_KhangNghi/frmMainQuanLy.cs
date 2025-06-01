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
    public partial class frmMainQuanLy : Form
    {
        public frmMainQuanLy()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            custom();
            OpenChildForm(new frmTrangChu());
            lblTieuDe.Text = "CÔNG TY TNHH THƯƠNG MẠI VÀ DỊCH VỤ KHANG NGHỊ";
            hideSubMenu();
        }
       
        public void custom()
        {
            panelSubMenu.Visible = false;
        }

        private void hideSubMenu()
        {
            if (panelSubMenu.Visible)
            {
                panelSubMenu.Visible = false;
            }
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private Form currentFormChild;

        public void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelBody.Controls.Add(childForm);
            panelBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmTrangChu(this));
            lblTieuDe.Text = "CÔNG TY TNHH THƯƠNG MẠI VÀ DỊCH VỤ KHANG NGHỊ";
            hideSubMenu();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmNhanVien());
            lblTieuDe.Text = btnNhanVien.Text;
            hideSubMenu();
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmKhachHang());
            lblTieuDe.Text = btnKhachHang.Text;
            hideSubMenu();
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmSanPham());
            lblTieuDe.Text = btnSanPham.Text;
            hideSubMenu();
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmDichVu());
            lblTieuDe.Text = btnDichVu.Text;
            hideSubMenu();
        }

        private void btnHopDong_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmHopDong());
            lblTieuDe.Text = btnHopDong.Text;
            hideSubMenu();
        }

        private void btnTonKho_Click(object sender, EventArgs e)
        {

        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmThongKe());
            lblTieuDe.Text = btnThongKe.Text;
            hideSubMenu();
        }

        private void btnLLV_Click(object sender, EventArgs e)
        {

        }

        private void btnCongNo_Click(object sender, EventArgs e)
        {

        }

        private void btnNhapXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnDMK_Click(object sender, EventArgs e)
        {

        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmNhaCungCap());
            lblTieuDe.Text = btnNCC.Text;
            hideSubMenu();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            ctxMenuTaiKhoan.Show(btnTaiKhoan, new Point(0, btnTaiKhoan.Height));
        }

        private void frmMainQuanLy_Load(object sender, EventArgs e)
        {
            frmTrangChu frm = new frmTrangChu(this); // Truyền this
            OpenChildForm(frm);
        }
    }
}
