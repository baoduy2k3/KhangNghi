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
    public partial class frmSanPham : Form
    {
        SanPhamBUS bus = new SanPhamBUS();
        ToolTip toolTip = new ToolTip();

        public frmSanPham()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadSanPham();
            LoadLoaiSanPham();
            FormatDataGridView();
            LoadDonViTinh();
            toolTip.SetToolTip(txtTimKiem, "Nhập tên hoặc mã sản phẩm để tìm kiếm");
            toolTip.SetToolTip(btnTimKiem, "Nhấn để thực hiện tìm kiếm");
        }

        private void LoadDonViTinh()
        {
            List<string> donViTinh = new List<string>()
            {
                "Cái",
                "Chiếc",
                "Kg",
                "Lít",
                "Hộp",
                "Thùng"
            };
            cbDVT.DataSource = donViTinh;
            cbDVT.SelectedIndex = -1;
        }


        private void FormatDataGridView()
        {
            dgvDSSP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDSSP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDSSP.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvDSSP.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void LoadSanPham()
        {
            dgvDSSP.DataSource = bus.LayDanhSachSanPham();
            // Đặt tiêu đề cột
            if (dgvDSSP.Columns.Contains("MaSP"))
                dgvDSSP.Columns["MaSP"].HeaderText = "Mã sản phẩm";
            if (dgvDSSP.Columns.Contains("TenSP"))
                dgvDSSP.Columns["TenSP"].HeaderText = "Tên sản phẩm";
            if (dgvDSSP.Columns.Contains("DonViTinh"))
                dgvDSSP.Columns["DonViTinh"].HeaderText = "Đơn vị tính";
            if (dgvDSSP.Columns.Contains("MaLoai"))
                dgvDSSP.Columns["MaLoai"].Visible = false;
            if (dgvDSSP.Columns.Contains("GiaBan"))
                dgvDSSP.Columns["GiaBan"].HeaderText = "Giá bán";
            if (dgvDSSP.Columns.Contains("TenLoaiSanPham"))
                dgvDSSP.Columns["TenLoaiSanPham"].HeaderText = "Loại sản phẩm";
            if (dgvDSSP.Columns.Contains("MoTa"))
                dgvDSSP.Columns["MoTa"].HeaderText = "Mô tả";          
            txtMaSP.Text = GenerateMaSP();
            txtMaSP.ReadOnly = true;
            txtMaSP.TabStop = false;
        }

        private void LoadLoaiSanPham()
        {
            cbLoaiSP.DataSource = bus.LayDanhSachLoaiSanPham();
            cbLoaiSP.DisplayMember = "TenLoai";   // Hiển thị tên cho người dùng chọn
            cbLoaiSP.ValueMember = "MaLoai";      // Giá trị thực sự dùng để lưu vào DB
            cbLoaiSP.SelectedIndex = -1;
        }

        private string GenerateMaSP()
        {
            DataTable dt = bus.LayDanhSachSanPham();
            if (dt.Rows.Count == 0) return "SP001";
            string lastMa = dt.Rows[dt.Rows.Count - 1]["MaSP"].ToString();
            int number = int.Parse(lastMa.Substring(2)) + 1;
            return "SP" + number.ToString("D3");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thêm sản phẩm này?",
                "Xác nhận thêm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            try
            {
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    DonViTinh = cbDVT.SelectedItem.ToString(),
                    MaLoai = cbLoaiSP.SelectedValue.ToString(),
                    GiaBan = decimal.Parse(txtGiaBan.Text),
                    MoTa = txtMoTa.Text
                };

                if (bus.ThemSanPham(sp))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSanPham();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSSP.CurrentRow != null)
            {
                string maSP = dgvDSSP.CurrentRow.Cells["MaSP"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sản phẩm có mã \"{maSP}\"?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No) return;

                try
                {
                    if (bus.XoaSanPham(maSP))
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSanPham();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn sửa thông tin sản phẩm này?",
                "Xác nhận sửa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            try
            {
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    DonViTinh = cbDVT.SelectedItem.ToString(),
                    MaLoai = cbLoaiSP.SelectedValue.ToString(),
                    GiaBan = decimal.Parse(txtGiaBan.Text),
                    MoTa = txtMoTa.Text
                };

                if (bus.SuaSanPham(sp))
                {
                    MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSanPham();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sửa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDSSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSSP.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
                cbLoaiSP.Text = row.Cells["TenLoaiSanPham"].Value.ToString();
                txtGiaBan.Text = row.Cells["GiaBan"].Value.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value.ToString();
                cbLoaiSP.SelectedValue = row.Cells["MaLoai"].Value.ToString();

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaSP.Text = GenerateMaSP();
            txtTenSP.Clear();
            cbLoaiSP.SelectedIndex = -1;
            txtGiaBan.Clear();
            txtMoTa.Clear();
            txtTimKiem.Clear();
            LoadSanPham();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.");
                return false;
            }
            if (cbLoaiSP.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtGiaBan.Text))
            {
                MessageBox.Show("Vui lòng nhập giá bán.");
                return false;
            }
            if (!decimal.TryParse(txtGiaBan.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Giá bán phải là một số dương hợp lệ.");
                return false;
            }
            return true;
        }
       

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt = bus.LayDanhSachSanPham();
            var filtered = dt.AsEnumerable()
                             .Where(row => row["MaSP"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                           row["TenSP"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (filtered.Any())
            {
                dgvDSSP.DataSource = filtered.CopyToDataTable();
            }
            else
            {
                dgvDSSP.DataSource = null;
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem.PerformClick(); // giả lập nhấn nút Tìm kiếm
            }
        }
    }
}
