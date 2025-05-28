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
    public partial class frmTrangChu : Form
    {
        public frmTrangChu()
        {
            InitializeComponent();
        }

        private void frmTrangChu_Load(object sender, EventArgs e)
        {
            // Xóa dataset cũ (không phải Series)
            gunaChart1.Datasets.Clear();

            // Tạo dataset cột
            var dataset = new GunaBarDataset
            {
                Label = "Doanh thu dịch vụ"
            };

            // Thêm dữ liệu
            dataset.DataPoints.Add("Sửa điện", 1000000);
            dataset.DataPoints.Add("Thay linh kiện", 1500000);
            dataset.DataPoints.Add("Bảo trì", 800000);
            dataset.DataPoints.Add("HMI", 2000000);
            dataset.DataPoints.Add("Nâng cấp", 2000000);
            dataset.DataPoints.Add("Biến tần", 1800000);

            // Thêm dataset vào biểu đồ
            gunaChart1.Datasets.Add(dataset);

            // Cập nhật biểu đồ
            gunaChart1.Update();
        }
    }
}
