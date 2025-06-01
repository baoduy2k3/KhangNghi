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
    public partial class frmKhachHang : Form
    {
        KhachHangBUS bus = new KhachHangBUS();
        DiaChiAPIHelper diaChiHelper = new DiaChiAPIHelper();
        ToolTip toolTip = new ToolTip();


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
            toolTip.SetToolTip(txtTimKiem, "Nhập họ tên hoặc mã khách hàng để tìm kiếm");
            toolTip.SetToolTip(btnTimKiem, "Nhấn để thực hiện tìm kiếm");
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
           
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thêm khách hàng này?",
                "Xác nhận thêm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

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
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhachHang();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSKH.CurrentRow != null)
            {
                string maKH = dgvDSKH.CurrentRow.Cells["MaKH"].Value.ToString();

                var confirm = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này không?",
                                              "Xác nhận xóa",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
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
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var confirm = MessageBox.Show("Bạn có chắc muốn sửa thông tin khách hàng này không?",
                                          "Xác nhận sửa",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

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

        private async void dgvDSKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDSKH.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
                txtHoTen.Text = row.Cells["TenKh"].Value.ToString();
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
                    txtSNTD.Clear();
                    cbTT.SelectedIndex = -1;
                    cbQH.DataSource = null;
                    cbXP.DataSource = null;
                }

                // Loại khách hàng
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
            txtTimKiem.Clear();
            LoadKhachHang();
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

            // Kiểm tra định dạng email
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email phải có dạng [Tên email] + [@] + [Tên miền].");
                return false;
            }

            // Kiểm tra số điện thoại (bắt đầu bằng 0, 10-12 chữ số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, @"^0\d{9,11}$"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu bằng 0 và có độ dài từ 10 đến 12 chữ số.");
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

            DataTable dt = bus.LayDanhSachKhachHang();
            var filtered = dt.AsEnumerable()
                             .Where(row => row["MaKH"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                           row["TenKH"].ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (filtered.Any())
            {
                dgvDSKH.DataSource = filtered.CopyToDataTable();
            }
            else
            {
                dgvDSKH.DataSource = null;
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
