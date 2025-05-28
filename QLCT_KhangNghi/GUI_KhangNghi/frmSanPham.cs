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
    public partial class frmSanPham : Form
    {
        SanPhamBUS bus = new SanPhamBUS();

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
            if (dgvDSSP.Columns.Contains("MaLoaiSP"))
                dgvDSSP.Columns["MaLoaiSP"].Visible = false;
            if (dgvDSSP.Columns.Contains("GiaBan"))
                dgvDSSP.Columns["GiaBan"].HeaderText = "Giá bán";
            if (dgvDSSP.Columns.Contains("MoTa"))
                dgvDSSP.Columns["MoTa"].HeaderText = "Mô tả";
            if (dgvDSSP.Columns.Contains("TenLoaiSanPham"))
                dgvDSSP.Columns["TenLoaiSanPham"].HeaderText = "Loại sản phẩm";
            txtMaSP.Text = GenerateMaSP();
            txtMaSP.ReadOnly = true;
            txtMaSP.TabStop = false;
        }

        private void LoadLoaiSanPham()
        {
            cbLoaiSP.DataSource = bus.LayDanhSachLoaiSanPham();
            cbLoaiSP.DisplayMember = "TenLoaiSP";
            cbLoaiSP.ValueMember = "TenLoaiSanPham";
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

            SanPhamDTO sp = new SanPhamDTO
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                MaLoaiSP = cbLoaiSP.SelectedValue.ToString(),
                GiaBan = decimal.Parse(txtGiaBan.Text),
                MoTa = txtMoTa.Text
            };

            if (bus.ThemSanPham(sp))
            {
                MessageBox.Show("Thêm sản phẩm thành công!");
                LoadSanPham();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm sản phẩm thất bại!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSSP.CurrentRow != null)
            {
                string maSP = dgvDSSP.CurrentRow.Cells["MaSP"].Value.ToString();

                if (bus.XoaSanPham(maSP))
                {
                    MessageBox.Show("Xóa sản phẩm thành công!");
                    LoadSanPham();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại!");
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            SanPhamDTO sp = new SanPhamDTO
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                MaLoaiSP = cbLoaiSP.SelectedValue.ToString(),
                GiaBan = decimal.Parse(txtGiaBan.Text),
                MoTa = txtMoTa.Text
            };

            if (bus.SuaSanPham(sp))
            {
                MessageBox.Show("Sửa sản phẩm thành công!");
                LoadSanPham();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Sửa sản phẩm thất bại!");
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
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text) ||
                cbLoaiSP.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtGiaBan.Text) ||
                !decimal.TryParse(txtGiaBan.Text, out _))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng định dạng thông tin sản phẩm.");
                return false;
            }
            return true;
        }
    }
}
