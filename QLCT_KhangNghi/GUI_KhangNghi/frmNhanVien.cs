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
using DevExpress.Data.NetCompatibility.Extensions;

namespace GUI_KhangNghi
{
    public partial class frmNhanVien : Form
    {
        NhanVienBUS bus = new NhanVienBUS();
        DiaChiAPIHelper diaChiHelper = new DiaChiAPIHelper();
        ToolTip toolTip = new ToolTip();

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
            toolTip.SetToolTip(txtTimKiem, "Nhập họ tên hoặc mã nhân viên để tìm kiếm");
            toolTip.SetToolTip(btnTimKiem, "Nhấn để thực hiện tìm kiếm");

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
            // Đặt lại tiêu đề cột (HeaderText)
            dgvDSNV.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dgvDSNV.Columns["HoTen"].HeaderText = "Họ tên";
            dgvDSNV.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dgvDSNV.Columns["GioiTinh"].HeaderText = "Giới tính";
            dgvDSNV.Columns["Email"].HeaderText = "Email";
            dgvDSNV.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
            dgvDSNV.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvDSNV.Columns["TenChucVu"].HeaderText = "Chức vụ";
            dgvDSNV.Columns["TenPB"].HeaderText = "Phòng ban";
            // Các cột khóa chính/ngoại ẩn đi
            if (dgvDSNV.Columns.Contains("MaChucVu"))
                dgvDSNV.Columns["MaChucVu"].Visible = false;
            if (dgvDSNV.Columns.Contains("MaPB"))
                dgvDSNV.Columns["MaPB"].Visible = false;
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

            DialogResult result = MessageBox.Show("Bạn có muốn thêm nhân viên mới?", "Xác nhận thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

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
                    DiaChi = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}",
                    MaChucVu = cbChucVu.SelectedValue.ToString(),
                    MaPB = cbPhongBan.SelectedValue.ToString()
                };

                if (bus.ThemNhanVien(nv))
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhanVien();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSNV.CurrentRow != null)
            {
                string maNV = dgvDSNV.CurrentRow.Cells["MaNV"].Value.ToString();
                string tenNV = dgvDSNV.CurrentRow.Cells["HoTen"].Value.ToString();
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên \"{tenNV}\" (Mã: {maNV}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (bus.XoaNhanVien(maNV))
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                            ResetForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin nhân viên này?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

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
                    DiaChi = $"{txtSNTD.Text}, {cbXP.Text}, {cbQH.Text}, {cbTT.Text}",
                    MaChucVu = cbChucVu.SelectedValue.ToString(),
                    MaPB = cbPhongBan.SelectedValue.ToString()
                };

                if (bus.SuaNhanVien(nv))
                {
                    MessageBox.Show("Sửa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhanVien();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sửa nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTimKiem.Clear();
            LoadNhanVien();
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
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if ((DateTime.Today.Year - dtpNgSinh.Value.Year) < 18)
                {
                    MessageBox.Show("Nhân viên phải từ 18 tuổi trở lên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private async void dgvDSNV_CellClick(object sender, DataGridViewCellEventArgs e)
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

                string diaChi = row.Cells["DiaChi"].Value.ToString();

                // Tách địa chỉ
                string[] parts = diaChi.Split(new[] { ", " }, StringSplitOptions.None);
                if (parts.Length == 4)
                {
                    txtSNTD.Text = parts[0];
                    string tenXP = parts[1];
                    string tenQH = parts[2];
                    string tenTT = parts[3];

                    // Load tỉnh/thành
                    var tinhList = await diaChiHelper.GetTinhThanh();
                    cbTT.DataSource = tinhList;
                    cbTT.DisplayMember = "name";
                    cbTT.ValueMember = "code";

                    var selectedTinh = tinhList.FirstOrDefault(t => t.name == tenTT);
                    if (selectedTinh != null)
                    {
                        cbTT.SelectedValue = selectedTinh.code;

                        // Load quận/huyện theo mã tỉnh
                        var qhList = await diaChiHelper.GetQuanHuyen(selectedTinh.code);
                        cbQH.DataSource = qhList;
                        cbQH.DisplayMember = "name";
                        cbQH.ValueMember = "code";

                        var selectedQH = qhList.FirstOrDefault(q => q.name == tenQH);
                        if (selectedQH != null)
                        {
                            cbQH.SelectedValue = selectedQH.code;

                            // Load xã/phường theo mã huyện
                            var xpList = await diaChiHelper.GetXaPhuong(selectedQH.code);
                            cbXP.DataSource = xpList;
                            cbXP.DisplayMember = "name";
                            cbXP.ValueMember = "code";

                            var selectedXP = xpList.FirstOrDefault(x => x.name == tenXP);
                            if (selectedXP != null)
                            {
                                cbXP.SelectedValue = selectedXP.code;
                            }
                            else
                            {
                                cbXP.SelectedIndex = -1;
                            }
                        }
                        else
                        {
                            cbQH.SelectedIndex = -1;
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
                    // Nếu địa chỉ không đủ 4 phần, xóa trống dữ liệu địa chỉ
                    txtSNTD.Clear();
                    cbTT.SelectedIndex = -1;
                    cbQH.DataSource = null;
                    cbXP.DataSource = null;
                }

                // Chức vụ & phòng ban
                cbChucVu.Text = row.Cells["TenChucVu"].Value.ToString();
                cbPhongBan.Text = row.Cells["TenPB"].Value.ToString();
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

            DataTable dt = bus.LayDanhSachNhanVien();
            var filtered = dt.AsEnumerable()
                             .Where(row => row["MaNV"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                           row["HoTen"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (filtered.Any())
            {
                dgvDSNV.DataSource = filtered.CopyToDataTable();
            }
            else
            {
                dgvDSNV.DataSource = null;
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
