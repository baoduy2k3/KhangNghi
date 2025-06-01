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
    public partial class frmNhaCungCap : Form
    {
        NhaCungCapBUS bus = new NhaCungCapBUS();
        DiaChiAPIHelper diaChiHelper = new DiaChiAPIHelper();
        ToolTip toolTip = new ToolTip();

        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private async void frmNhaCungCap_Load(object sender, EventArgs e)
        {          
            LoadNhaCungCap();
            await LoadTinhThanh();
            FormatDataGridView();
            toolTip.SetToolTip(txtTimKiem, "Nhập tên hoặc mã nhà cung cấp để tìm kiếm");
            toolTip.SetToolTip(btnTimKiem, "Nhấn để thực hiện tìm kiếm");
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
            if (dt.Rows.Count == 0) return "NCC01";
            string lastMa = dt.Rows[dt.Rows.Count - 1]["MaNCC"].ToString();
            int number = int.Parse(lastMa.Substring(3)) + 1;
            return "NCC" + number.ToString("D2");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thêm nhà cung cấp này không?", "Xác nhận thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

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
                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhaCungCap();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            if (dgvDSNCC.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn sửa thông tin nhà cung cấp này không?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

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
                MessageBox.Show("Sửa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhaCungCap();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Sửa nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSNCC.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNCC = dgvDSNCC.CurrentRow.Cells["MaNCC"].Value.ToString();

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp mã {maNCC} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            if (bus.XoaNhaCungCap(maNCC))
            {
                MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhaCungCap();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Xóa nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTimKiem.Clear();
            LoadNhaCungCap();
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

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email phải có dạng [Tên email] + [@] + [Tên miền].", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, @"^0\d{9,11}$"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu bằng 0 và có độ dài từ 10 đến 12 chữ số.");
                return false;
            }

            return true;
        }

        private async void dgvDSNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSNCC.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();

                string diaChiFull = row.Cells["DiaChi"].Value.ToString();
                string[] diaChiParts = diaChiFull.Split(new[] { ", " }, StringSplitOptions.None);

                if (diaChiParts.Length == 4)
                {
                    txtSNTD.Text = diaChiParts[0].Trim();
                    string tenXP = diaChiParts[1].Trim();
                    string tenQH = diaChiParts[2].Trim();
                    string tenTT = diaChiParts[3].Trim();

                    // Load Tỉnh/Thành
                    var tinhList = await diaChiHelper.GetTinhThanh();
                    cbTT.DataSource = tinhList;
                    cbTT.DisplayMember = "name";
                    cbTT.ValueMember = "code";

                    var selectedTinh = tinhList.FirstOrDefault(t => t.name == tenTT);
                    if (selectedTinh != null)
                    {
                        cbTT.SelectedValue = selectedTinh.code;

                        // Load Quận/Huyện
                        var huyenList = await diaChiHelper.GetQuanHuyen(selectedTinh.code);
                        cbQH.DataSource = huyenList;
                        cbQH.DisplayMember = "name";
                        cbQH.ValueMember = "code";

                        var selectedHuyen = huyenList.FirstOrDefault(h => h.name == tenQH);
                        if (selectedHuyen != null)
                        {
                            cbQH.SelectedValue = selectedHuyen.code;

                            // Load Xã/Phường
                            var phuongList = await diaChiHelper.GetXaPhuong(selectedHuyen.code);
                            cbXP.DataSource = phuongList;
                            cbXP.DisplayMember = "name";
                            cbXP.ValueMember = "code";

                            var selectedPhuong = phuongList.FirstOrDefault(p => p.name == tenXP);
                            if (selectedPhuong != null)
                            {
                                cbXP.SelectedValue = selectedPhuong.code;
                            }
                            else
                            {
                                cbXP.SelectedIndex = -1;
                            }
                        }
                        else
                        {
                            cbQH.DataSource = null;
                            cbXP.DataSource = null;
                        }
                    }
                    else
                    {
                        cbTT.SelectedIndex = -1;
                        cbQH.DataSource = null;
                        cbXP.DataSource = null;
                    }
                }
                else
                {
                    txtSNTD.Clear();
                    cbTT.SelectedIndex = -1;
                    cbQH.DataSource = null;
                    cbXP.DataSource = null;
                }
            }
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

            DataTable dt = bus.LayDanhSachNhaCungCap();
            var filtered = dt.AsEnumerable()
                             .Where(row => row["MaNCC"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                           row["TenNCC"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (filtered.Any())
            {
                dgvDSNCC.DataSource = filtered.CopyToDataTable();
            }
            else
            {
                dgvDSNCC.DataSource = null;
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
