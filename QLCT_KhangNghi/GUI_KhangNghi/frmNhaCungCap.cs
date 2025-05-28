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
    public partial class frmNhaCungCap : Form
    {
        NhaCungCapBUS bus = new NhaCungCapBUS();
        DiaChiAPIHelper diaChiHelper = new DiaChiAPIHelper();

        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private async void frmNhaCungCap_Load(object sender, EventArgs e)
        {          
            LoadNhaCungCap();
            await LoadTinhThanh();
            FormatDataGridView();
        }

        private void FormatDataGridView()
        {
            dgvDSNCC.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDSNCC.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDSNCC.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvDSNCC.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void LoadNhaCungCap()
        {
            dgvDSNCC.DataSource = bus.LayDanhSachNhaCungCap();
            // Đặt lại tên cột
            if (dgvDSNCC.Columns.Contains("MaNCC"))
                dgvDSNCC.Columns["MaNCC"].HeaderText = "Mã NCC";
            if (dgvDSNCC.Columns.Contains("TenNCC"))
                dgvDSNCC.Columns["TenNCC"].HeaderText = "Tên NCC";
            if (dgvDSNCC.Columns.Contains("Email"))
                dgvDSNCC.Columns["Email"].HeaderText = "Email";
            if (dgvDSNCC.Columns.Contains("SoDienThoai"))
                dgvDSNCC.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
            if (dgvDSNCC.Columns.Contains("DiaChi"))
                dgvDSNCC.Columns["DiaChi"].HeaderText = "Địa chỉ";
            txtMaNCC.Text = GenerateMaNCC();
            txtMaNCC.ReadOnly = true;
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

        private string GenerateMaNCC()
        {
            DataTable dt = bus.LayDanhSachNhaCungCap();
            if (dt.Rows.Count == 0) return "NCC001";
            string lastMa = dt.Rows[dt.Rows.Count - 1]["MaNCC"].ToString();
            int number = int.Parse(lastMa.Substring(3)) + 1;
            return "NCC" + number.ToString("D3");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            NhaCungCapDTO ncc = new NhaCungCapDTO
            {
                MaNCC = txtMaNCC.Text,
                TenNCC = txtTenNCC.Text,
                Email = txtEmail.Text,
                SoDienThoai = txtSDT.Text,
                DiaChi = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}"
            };

            if (bus.ThemNhaCungCap(ncc))
            {
                MessageBox.Show("Thêm nhà cung cấp thành công!");
                LoadNhaCungCap();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            NhaCungCapDTO ncc = new NhaCungCapDTO
            {
                MaNCC = txtMaNCC.Text,
                TenNCC = txtTenNCC.Text,
                Email = txtEmail.Text,
                SoDienThoai = txtSDT.Text,
                DiaChi = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}"
            };

            if (bus.SuaNhaCungCap(ncc))
            {
                MessageBox.Show("Sửa thành công!");
                LoadNhaCungCap();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSNCC.CurrentRow != null)
            {
                string maNCC = dgvDSNCC.CurrentRow.Cells["MaNCC"].Value.ToString();
                if (bus.XoaNhaCungCap(maNCC))
                {
                    MessageBox.Show("Xóa thành công");
                    LoadNhaCungCap();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaNCC.Text = GenerateMaNCC();
            txtTenNCC.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtSNTD.Clear();
            cbTT.SelectedIndex = -1;
            cbQH.DataSource = null;
            cbXP.DataSource = null;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtSNTD.Text) ||
                cbTT.SelectedIndex == -1 ||
                cbQH.SelectedIndex == -1 ||
                cbXP.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return false;
            }
            return true;
        }

        private void dgvDSNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSNCC.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value.ToString();
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
            }
        }
    }
}
