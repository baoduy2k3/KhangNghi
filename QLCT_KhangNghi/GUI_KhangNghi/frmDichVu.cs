using BUS_KhangNghi;
using DevExpress.Data.NetCompatibility.Extensions;
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
    public partial class frmDichVu : Form
    {
        DichVuBUS bus = new DichVuBUS();
        ToolTip toolTip = new ToolTip();

        public frmDichVu()
        {
            InitializeComponent();
        }

        private void frmDichVu_Load(object sender, EventArgs e)
        {
            LoadDichVu();
            FormatDataGirdView();
            ResetForm();
            txtMaDV.Text = GenerateMaDV();
            txtMaDV.ReadOnly = true;
            toolTip.SetToolTip(txtTimKiem, "Nhập tên hoặc mã dịch vụ để tìm kiếm");
            toolTip.SetToolTip(btnTimKiem, "Nhấn để thực hiện tìm kiếm");
        }

        private void LoadDichVu()
        {
            dgvDSDV.DataSource = bus.LayDanhSachDichVu();
            if (dgvDSDV.Columns.Contains("MaDV"))
                dgvDSDV.Columns["MaDV"].HeaderText = "Mã dịch vụ";

            if (dgvDSDV.Columns.Contains("TenDV"))
                dgvDSDV.Columns["TenDV"].HeaderText = "Tên dịch vụ";

            if (dgvDSDV.Columns.Contains("GiaDichVu"))
                dgvDSDV.Columns["GiaDichVu"].HeaderText = "Giá dịch vụ";

            if (dgvDSDV.Columns.Contains("MoTa"))
                dgvDSDV.Columns["MoTa"].HeaderText = "Mô tả";
        }

        private void FormatDataGirdView()
        {
            dgvDSDV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDSDV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDSDV.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvDSDV.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private string GenerateMaDV()
        {
            DataTable dt = bus.LayDanhSachDichVu();
            if (dt.Rows.Count == 0) return "DV001";
            string lastMa = dt.Rows[dt.Rows.Count - 1]["MaDV"].ToString();
            int number = int.Parse(lastMa.Substring(2)) + 1;
            return "DV" + number.ToString("D3");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thêm dịch vụ mới này không?",
                "Xác nhận thêm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            DichVuDTO dv = new DichVuDTO
            {
                MaDV = txtMaDV.Text,
                TenDV = txtTenDV.Text,
                GiaDichVu = decimal.Parse(txtGiaDV.Text),
                MoTa = txtMoTa.Text
            };

            if (bus.ThemDichVu(dv))
            {
                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm dịch vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSDV.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maDV = dgvDSDV.CurrentRow.Cells["MaDV"].Value.ToString();
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa dịch vụ có mã: {maDV} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (bus.XoaDichVu(maDV))
                {
                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa dịch vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            if (dgvDSDV.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn sửa thông tin dịch vụ này không?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            DichVuDTO dv = new DichVuDTO
            {
                MaDV = txtMaDV.Text,
                TenDV = txtTenDV.Text,
                GiaDichVu = decimal.Parse(txtGiaDV.Text),
                MoTa = txtMoTa.Text
            };

            if (bus.SuaDichVu(dv))
            {
                MessageBox.Show("Sửa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Sửa dịch vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDSDV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSDV.Rows[e.RowIndex];
                txtMaDV.Text = row.Cells["MaDV"].Value.ToString();
                txtTenDV.Text = row.Cells["TenDV"].Value.ToString();
                txtGiaDV.Text = row.Cells["GiaDichVu"].Value.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value.ToString();
            }
        }

        private void ResetForm()
        {
            txtMaDV.Text = GenerateMaDV();
            txtTenDV.Clear();
            txtGiaDV.Clear();
            txtMoTa.Clear();
            txtTimKiem.Clear();
            LoadDichVu();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Tên dịch vụ không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGiaDV.Text) || !decimal.TryParse(txtGiaDV.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Giá dịch vụ phải là số và lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMoTa.Text))
            {
                MessageBox.Show("Mô tả không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem.PerformClick(); // giả lập nhấn nút Tìm kiếm
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt = bus.LayDanhSachDichVu();
            var filtered = dt.AsEnumerable()
                             .Where(row => row["MaDV"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                           row["TenDV"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (filtered.Any())
            {
                dgvDSDV.DataSource = filtered.CopyToDataTable();
            }
            else
            {
                dgvDSDV.DataSource = null;
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
