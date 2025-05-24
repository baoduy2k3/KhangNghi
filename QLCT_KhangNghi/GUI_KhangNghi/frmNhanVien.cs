using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BUS_KhangNghi;
using DTO_KhangNghi;
using static GUI_KhangNghi.DiaChiAPIHelper;
using System.Threading.Tasks;
using System.Drawing;

namespace GUI_KhangNghi
{
    public partial class frmNhanVien : Form
    {
        NhanVienBUS bus = new NhanVienBUS();
        DiaChiAPIHelper diaChiHelper = new DiaChiAPIHelper();
        public frmNhanVien()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private async void frmNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            LoadChucVu_PhongBan();
            await LoadTinhThanh();
            FormatDataGirdView();
        }

        private void FormatDataGirdView()
        {
            dgvDSNV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDSNV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDSNV.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvDSNV.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void LoadNhanVien()
        {
            dgvDSNV.DataSource = bus.LayDanhSachNhanVien();           
            LoadChucVu_PhongBan();           
            LoadTinhThanh();
            txtMaNV.Text = GenerateMaNV();
            txtMaNV.ReadOnly = true;
            txtMaNV.TabStop = false;
        }

        private void LoadChucVu_PhongBan()
        {
            cbChucVu.DataSource = bus.LayDanhSachChucVu();
            cbChucVu.DisplayMember = "TenChucVu";
            cbChucVu.ValueMember = "MaChucVu";

            cbPhongBan.DataSource = bus.LayDanhSachPhongBan();
            cbPhongBan.DisplayMember = "TenPB";
            cbPhongBan.ValueMember = "MaPB";
        }

        private async Task LoadTinhThanh()
        {
            var list = await diaChiHelper.GetTinhThanh();
            cbTT.DataSource = list;
            cbTT.DisplayMember = "name";
            cbTT.ValueMember = "code";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            try
            {
                NhanVienDTO nv = new NhanVienDTO
                {
                    MaNV = txtMaNV.Text,
                    HoTen = txtHoTen.Text,
                    NgaySinh = dtpNgSinh.Value,
                    GioiTinh = crbNam.Checked ? "Nam" : "Nữ",
                    Email = txtEmail.Text,
                    SoDienThoai = txtSDT.Text,
                    DiaChi = txtSNTD.Text + ", " + cbXP.Text + ", " + cbQH.Text + ", " + cbTT.Text,
                    MaChucVu = cbChucVu.SelectedValue.ToString(),
                    MaPB = cbPhongBan.SelectedValue.ToString()
                };

                if (bus.ThemNhanVien(nv))
                {
                    MessageBox.Show("Thêm nhân viên thành công!");
                    LoadNhanVien();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSNV.CurrentRow != null)
            {
                string maNV = dgvDSNV.CurrentRow.Cells["MaNV"].Value.ToString();
                if (bus.XoaNhanVien(maNV))
                {
                    MessageBox.Show("Xóa thành công");
                    LoadNhanVien();
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
            try
            {
                NhanVienDTO nv = new NhanVienDTO
                {
                    MaNV = txtMaNV.Text,
                    HoTen = txtHoTen.Text,
                    NgaySinh = dtpNgSinh.Value,
                    GioiTinh = crbNam.Checked ? "Nam" : "Nữ",
                    Email = txtEmail.Text,
                    SoDienThoai = txtSDT.Text,
                    DiaChi = txtSNTD.Text + ", " + cbXP.Text + ", " + cbQH.Text + ", " + cbTT.Text,
                    MaChucVu = cbChucVu.SelectedValue.ToString(),
                    MaPB = cbPhongBan.SelectedValue.ToString()
                };

                if (bus.SuaNhanVien(nv))
                {
                    MessageBox.Show("Sửa nhân viên thành công!");
                    LoadNhanVien();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sửa nhân viên thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
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

        private string GenerateMaNV()
        {
            DataTable dt = bus.LayDanhSachNhanVien();
            if (dt.Rows.Count == 0) return "NV001";
            string lastMa = dt.Rows[dt.Rows.Count - 1]["MaNV"].ToString();
            int number = int.Parse(lastMa.Substring(2)) + 1;
            return "NV" + number.ToString("D3");
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    string diaChiDayDu = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}";
                    NhanVienDTO nv = new NhanVienDTO
                    {
                        MaNV = txtMaNV.Text,
                        HoTen = txtHoTen.Text,
                        NgaySinh = dtpNgSinh.Value,
                        GioiTinh = crbNam.Checked ? "Nam" : "Nữ",
                        Email = txtEmail.Text,
                        SoDienThoai = txtSDT.Text,
                        DiaChi = diaChiDayDu,
                        MaChucVu = cbChucVu.SelectedValue.ToString(),
                        MaPB = cbPhongBan.SelectedValue.ToString()
                    };

                    if (bus.SuaNhanVien(nv))
                    {
                        MessageBox.Show("Cập nhật thành công");
                        LoadNhanVien();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaNV.Text = GenerateMaNV();
            txtHoTen.Clear();
            dtpNgSinh.Value = DateTime.Today;
            crbNam.Checked = true;
            txtEmail.Clear();
            txtSDT.Clear();
            txtSNTD.Clear();
            cbTT.SelectedIndex = -1;
            cbQH.DataSource = null;
            cbXP.DataSource = null;
            cbChucVu.SelectedIndex = -1;
            cbPhongBan.SelectedIndex = -1;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtSNTD.Text) ||
                cbChucVu.SelectedIndex == -1 ||
                cbPhongBan.SelectedIndex == -1 ||
                cbTT.SelectedIndex == -1 ||
                cbQH.SelectedIndex == -1 ||
                cbXP.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return false;
            }
            return true;
        }

        private void dgvDSNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSNV.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                dtpNgSinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                string gioiTinh = row.Cells["GioiTinh"].Value.ToString();
                crbNam.Checked = gioiTinh == "Nam";
                crbNu.Checked = gioiTinh == "Nữ";
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

                cbChucVu.Text = row.Cells["TenChucVu"].Value.ToString();
                cbPhongBan.Text = row.Cells["TenPB"].Value.ToString();
            }
        }
    }
}
