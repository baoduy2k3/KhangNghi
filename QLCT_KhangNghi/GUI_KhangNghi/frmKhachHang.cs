using BUS_KhangNghi;
using DTO_KhangNghi;
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
    public partial class frmKhachHang : Form
    {
        KhachHangBUS bus = new KhachHangBUS();
        DiaChiAPIHelper diaChiHelper = new DiaChiAPIHelper();

        public frmKhachHang()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private async void frmKhachHang_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            LoadLoaiKH();
            await LoadTinhThanh(); LoadTinhThanh();
            FormatDataGirdView();
        }

        private void FormatDataGirdView()
        {
            dgvDSKH.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDSKH.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;          
            dgvDSKH.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvDSKH.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void LoadKhachHang()
        {
            dgvDSKH.DataSource = bus.LayDanhSachKhachHang();
            // Đặt tiêu đề cột dễ hiểu
            dgvDSKH.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dgvDSKH.Columns["TenKH"].HeaderText = "Họ tên";
            dgvDSKH.Columns["TenLoaiKH"].HeaderText = "Loại khách hàng";
            dgvDSKH.Columns["Email"].HeaderText = "Email";
            dgvDSKH.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
            dgvDSKH.Columns["DiaChi"].HeaderText = "Địa chỉ";
            LoadTinhThanh();
            txtMaKH.Text = GenerateMaKH();
            txtMaKH.ReadOnly = true;
            txtMaKH.TabStop = false;
        }

        private void LoadLoaiKH()
        {
            cbLoaiKH.DataSource = bus.LayDanhSachLoaiKH();
            cbLoaiKH.DisplayMember = "TenLoaiKH";
            cbLoaiKH.ValueMember = "MaLoaiKH";
        }

        private async Task LoadTinhThanh()
        {
            var list = await diaChiHelper.GetTinhThanh();
            cbTT.DataSource = list;
            cbTT.DisplayMember = "name";
            cbTT.ValueMember = "code";
        }

        private async void cbTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTT.SelectedValue is string maTinh)
            {
                var list = await diaChiHelper.GetQuanHuyen(maTinh);
                cbQH.DataSource = list;
                cbQH.DisplayMember = "name";
                cbQH.ValueMember = "code";
            }
        }

        private async void cbQH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbQH.SelectedValue is string maHuyen)
            {
                var list = await diaChiHelper.GetXaPhuong(maHuyen);
                cbXP.DataSource = list;
                cbXP.DisplayMember = "name";
                cbXP.ValueMember = "code";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    MaKH = txtMaKH.Text,
                    TenKH = txtHoTen.Text,
                    MaLoaiKH = cbLoaiKH.SelectedValue.ToString(),
                    Email = txtEmail.Text,
                    SoDienThoai = txtSDT.Text,
                    DiaChi = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}"
                };

                if (bus.ThemKhachHang(kh))
                {
                    MessageBox.Show("Thêm khách hàng thành công!");
                    LoadKhachHang();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSKH.CurrentRow != null)
            {
                string maKH = dgvDSKH.CurrentRow.Cells["MaKH"].Value.ToString();
                if (bus.XoaKhachHang(maKH))
                {
                    MessageBox.Show("Xóa khách hàng thành công!");
                    LoadKhachHang();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại!");
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    MaKH = txtMaKH.Text,
                    TenKH = txtHoTen.Text,
                    MaLoaiKH = cbLoaiKH.SelectedValue.ToString(),
                    Email = txtEmail.Text,
                    SoDienThoai = txtSDT.Text,
                    DiaChi = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}"
                };

                if (bus.SuaKhachHang(kh))
                {
                    MessageBox.Show("Sửa khách hàng thành công!");
                    LoadKhachHang();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sửa khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvDSKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSKH.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
                txtHoTen.Text = row.Cells["TenKh"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();

                string[] diaChi = row.Cells["DiaChi"].Value.ToString().Split(',');
                if (diaChi.Length >= 4)
                {
                    txtSNTD.Text = diaChi[0].Trim();
                    cbXP.Text = diaChi[1].Trim();
                    cbQH.Text = diaChi[2].Trim();
                    cbTT.Text = diaChi[3].Trim();
                }

                cbLoaiKH.Text = row.Cells["TenLoaiKH"].Value.ToString();
            }
        }

        private string GenerateMaKH()
        {
            DataTable dt = bus.LayDanhSachKhachHang();
            if (dt.Rows.Count == 0) return "KH001";
            string lastMa = dt.Rows[dt.Rows.Count - 1]["MaKH"].ToString();
            int number = int.Parse(lastMa.Substring(2)) + 1;
            return "KH" + number.ToString("D3");
        }

        private void ResetForm()
        {
            txtMaKH.Text = GenerateMaKH();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtSNTD.Clear();
            cbLoaiKH.SelectedIndex = -1;
            cbTT.SelectedIndex = -1;
            cbQH.DataSource = null;
            cbXP.DataSource = null;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtSNTD.Text) ||
                cbLoaiKH.SelectedIndex == -1 ||
                cbTT.SelectedIndex == -1 ||
                cbQH.SelectedIndex == -1 ||
                cbXP.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return false;
            }
            return true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
    }
}
