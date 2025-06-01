using Guna.Charts.WinForms;
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
    public partial class frmThongKe : Form
    {
        public frmThongKe()
        {
            InitializeComponent();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            FormatDataGirdView();

            cbLoaiTK.Items.AddRange(new object[]
            {
                "Tất cả",
                "Doanh thu",
                "Công nợ",
                "Sản phẩm bán chạy",
                "Tồn kho",
                "Hợp đồng"
            });
            cbLoaiTK.SelectedIndex = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("Ngày");
            dt.Columns.Add("Mã HĐ");
            dt.Columns.Add("Khách hàng");
            dt.Columns.Add("Doanh thu", typeof(decimal));

            dt.Rows.Add("01/05/2025", "HD001", "Nguyễn Văn An", 1200000);
            dt.Rows.Add("02/05/2025", "HD002", "Trần Thị Bình", 2350000);
            dt.Rows.Add("03/05/2025", "HD003", "Phạm Văn Cường", 1750000);

            dgvThongKe.DataSource = dt;

            chartThongKe.Datasets.Clear(); // Xoá series cũ nếu có

            // Tạo dataset dạng cột (Column)
            var dataset = new GunaBarDataset
            {
                Label = "Doanh thu"
            };

            // Thêm dữ liệu mẫu
            dataset.DataPoints.Add("01/05", 1200000);
            dataset.DataPoints.Add("02/05", 2350000);
            dataset.DataPoints.Add("03/05", 1750000);
            dataset.DataPoints.Add("04/05", 2000000);
            dataset.DataPoints.Add("05/05", 2800000);

            // Thêm dataset vào chart
            chartThongKe.Datasets.Add(dataset);

            // Cập nhật hiển thị
            chartThongKe.Update();

        }

        private void FormatDataGirdView()
        {
            dgvThongKe.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvThongKe.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvThongKe.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvThongKe.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }
    }
}
