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
    public partial class frmDichVu : Form
    {
        DichVuBUS bus = new DichVuBUS();

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

            DichVuDTO dv = new DichVuDTO
            {
                MaDV = txtMaDV.Text,
                TenDV = txtTenDV.Text,
                GiaDichVu = decimal.Parse(txtGiaDV.Text),
                MoTa = txtMoTa.Text
            };

            if (bus.ThemDichVu(dv))
            {
                MessageBox.Show("Thêm dịch vụ thành công");
                LoadDichVu();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm dịch vụ thất bại");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSDV.CurrentRow != null)
            {
                string maDV = dgvDSDV.CurrentRow.Cells["MaDV"].Value.ToString();
                if (bus.XoaDichVu(maDV))
                {
                    MessageBox.Show("Xóa thành công");
                    LoadDichVu();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            DichVuDTO dv = new DichVuDTO
            {
                MaDV = txtMaDV.Text,
                TenDV = txtTenDV.Text,
                GiaDichVu = decimal.Parse(txtGiaDV.Text),
                MoTa = txtMoTa.Text
            };

            if (bus.SuaDichVu(dv))
            {
                MessageBox.Show("Sửa dịch vụ thành công");
                LoadDichVu();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Sửa dịch vụ thất bại");
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
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenDV.Text) ||
                string.IsNullOrWhiteSpace(txtGiaDV.Text) ||
                !decimal.TryParse(txtGiaDV.Text, out _) ||
                string.IsNullOrWhiteSpace(txtMoTa.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và hợp lệ thông tin.");
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
