using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_KhangNghi;
using DTO_KhangNghi;

namespace GUI_KhangNghi
{
    public partial class frmTrangChu : Form
    {
        NhanVienBUS busNhanVien = new NhanVienBUS();
        KhachHangBUS busKhachHang = new KhachHangBUS();

        private frmMainQuanLy mainForm;

        public frmTrangChu()
        {
            InitializeComponent();
        }

        public frmTrangChu(frmMainQuanLy frm)
        {
            InitializeComponent();
            mainForm = frm;
        }

        private void frmTrangChu_Load(object sender, EventArgs e)
        {
            CapNhatSoLuong();

            tlpNhanVien.MouseEnter += tlp_MouseEnter;
            tlpNhanVien.MouseLeave += tlp_MouseLeave;
            tlpKhachHang.MouseEnter += tlp_MouseEnter;
            tlpKhachHang.MouseLeave += tlp_MouseLeave;


            // Xóa dataset cũ (không phải Series)
            gunaChart1.Datasets.Clear();

            // Tạo dataset cột
            var dataset = new GunaBarDataset
            {
                Label = "Doanh thu dịch vụ"
            };

            // Thêm dữ liệu
            dataset.DataPoints.Add("Sửa điện", 1000000);
            dataset.DataPoints.Add("Thay linh kiện", 1500000);
            dataset.DataPoints.Add("Bảo trì", 800000);
            dataset.DataPoints.Add("HMI", 2000000);
            dataset.DataPoints.Add("Nâng cấp", 2000000);
            dataset.DataPoints.Add("Biến tần", 1800000);

            // Thêm dataset vào biểu đồ
            gunaChart1.Datasets.Add(dataset);

            // Cập nhật biểu đồ
            gunaChart1.Update();
        }

        // Load số lượng nhân viên và khách hàng
        private void CapNhatSoLuong()
        {
            int soLuongNhanVien = busNhanVien.LayDanhSachNhanVien().Rows.Count;
            int soLuongKhachHang = busKhachHang.LayDanhSachKhachHang().Rows.Count;

            lblNhanVien.Text = $"Nhân viên: {soLuongNhanVien}";
            lblKhachHang.Text = $"Khách hàng: {soLuongKhachHang}";
        }

        private void tlpNhanVien_Click(object sender, EventArgs e)
        {
            if (mainForm != null)
            {
                mainForm.OpenChildForm(new frmNhanVien());
                // Cập nhật lbl nếu cần hoặc gọi lại CapNhatSoLuong()
                CapNhatSoLuong();
            }
        }

        private void tlpKhachHang_Click(object sender, EventArgs e)
        {
            if (mainForm != null)
            {
                mainForm.OpenChildForm(new frmKhachHang());
                CapNhatSoLuong();
            }
        }

        private void tlp_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void tlp_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

    }
}
