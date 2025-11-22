using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyCuaHangTV
{
    public partial class formmain : Form
    {
        public formmain()
        {
            InitializeComponent();
        }

        private void formmain_Load(object sender, EventArgs e)
        {
            QuanLyCuaHangTV.Functions.Connect();

           
            // CODE ĐỂ CHÈN HÌNH ẢNH VÀO FULL MÀN HÌNH 
            // 1. Tải ảnh từ Resources của dự án
            // thêm ảnh 'dienmayxanh' vào Resources của dự án
            this.BackgroundImage = Properties.Resources.dienmayxanh;

            // 2. Cấu hình cách hiển thị ảnh nền: Stretch để lấp đầy toàn bộ Form
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // 3. Đặt Form ở chế độ Maximize (phóng to)
            this.WindowState = FormWindowState.Maximized;

            // muốn loại bỏ thanh tiêu đề và viền của Form
            // (Chế độ Fullscreen thực sự)
            this.FormBorderStyle = FormBorderStyle.None; 
            

        }

        private void tồnKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTonKho formTonKho = new FormTonKho(); //Khởi tạo đối tượng
            formTonKho.ShowDialog(); //Hiển thị dưới dạng hộp thoại
        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNhanVien formNhanVien = new FormNhanVien(); //Khởi tạo đối tượng
            formNhanVien.ShowDialog(); //Hiển thị dưới dạng hộp thoại
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyCuaHangTV.Functions.Disconnect(); //Đóng kết nối
            Application.Exit(); //Thoát
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKhachHang formKhachHang = new FormKhachHang(); //Khởi tạo đối tượng
            formKhachHang.ShowDialog(); //Hiển thị dưới dạng hộp thoại
        }

        private void hãngTIVIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuanLyTV formQuanLyTV = new FormQuanLyTV(); //Khởi tạo đối tượng
            formQuanLyTV.ShowDialog(); //Hiển thị dưới dạng hộp thoại

        }

        private void mnuHoaDon_Click(object sender, EventArgs e)
        {
            FormHoaDon formHoaDon = new FormHoaDon(); //Khởi tạo đối tượng
            formHoaDon.ShowDialog(); //Hiển thị dưới dạng hộp thoại
        }

        private void mnuChiTietHoaDon_Click(object sender, EventArgs e)
        {
            FormChiTietHoaDon formChiTietHoaDon = new FormChiTietHoaDon(); //Khởi tạo đối tượng
            formChiTietHoaDon.ShowDialog(); //Hiển thị dưới dạng hộp thoại
        }

       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Sự kiện này hiện không làm gì cả, có thể bỏ qua hoặc xóa nếu không dùng PictureBox.
        }
    }
}