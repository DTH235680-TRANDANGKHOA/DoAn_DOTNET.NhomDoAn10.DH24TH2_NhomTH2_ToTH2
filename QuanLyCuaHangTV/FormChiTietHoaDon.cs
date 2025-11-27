using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangTV
{
    public partial class FormChiTietHoaDon : Form
    {
        DataTable tblCTHD;
        public FormChiTietHoaDon()
        {
            InitializeComponent();
        }

        private void FormChiTietHoaDon_Load(object sender, EventArgs e)
        {
            // CODE THÊM MỚI ĐỂ PHÓNG TO TOÀN MÀN HÌNH 
            this.WindowState = FormWindowState.Maximized;
        

            // 1. Thiết lập trạng thái ban đầu
            txtMaChiTietHD.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // 2. Tải các ComboBox (Phải tải trước)
            LoadComboBoxMaHoaDon();
            LoadComboBoxMaTIVI(); 

            // 3. Tải Data Grid
            LoadDataGridView();

            // 4. Đặt mặc định ComboBox
            cmbMaHoaDon.SelectedIndex = -1;
            cmbMaTIVI.SelectedIndex = -1;
        }
        // --- HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW ---
        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM FormChiTietHoaDon";
            tblCTHD = Functions.GetDataToTable(sql); // Giả định Functions.GetDataToTable đã được định nghĩa
            dgvChiTietHoaDon.DataSource = tblCTHD;

            // Đặt tên cột
            dgvChiTietHoaDon.Columns["MaChiTietHoaDon"].HeaderText = "Mã Chi Tiết";
            dgvChiTietHoaDon.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
            dgvChiTietHoaDon.Columns["MaTIVI"].HeaderText = "Mã TIVI";
            dgvChiTietHoaDon.Columns["SoLuongMua"].HeaderText = "Số Lượng";
            dgvChiTietHoaDon.Columns["DonGia"].HeaderText = "Đơn Giá";

            dgvChiTietHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// Tự động điều chỉnh kích thước cột
            dgvChiTietHoaDon.AllowUserToAddRows = false;// Không cho người dùng thêm dữ liệu trực tiếp
            dgvChiTietHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;// Không cho sửa dữ liệu trực tiếp
        }

       


        private void LoadComboBoxMaHoaDon()
        {
            // Hiển thị Mã Hóa Đơn
            string sql = "SELECT MaHoaDon FROM FormHoaDon";
            Functions.FillCombo(sql, cmbMaHoaDon, "MaHoaDon", "MaHoaDon");// Hiển thị Mã Hóa Đơn
        }

        private void LoadComboBoxMaTIVI()
        {
            // Hiển thị Mã TIVI thay vì Tên TIVI
            string sql = "SELECT MaTIVI, TenTIVI FROM FormQuanLyTV";
            Functions.FillCombo(sql, cmbMaTIVI, "MaTIVI", "MaTIVI"); // Thay "TenTIVI" bằng "MaTIVI"
        }


        // --- HÀM RESET CÁC Ô NHẬP ---
        private void ResetValues()
        {
            txtMaChiTietHD.Text = "";
            cmbMaHoaDon.SelectedIndex = -1; // Xóa chọn ComboBox
            cmbMaTIVI.SelectedIndex = -1;  // Xóa chọn ComboBox
            txtSoLuongMua.Text = "";
            txtDonGia.Text = "";
        }

        private void dgvChiTietHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            txtMaChiTietHD.Enabled = true; // Cho phép nhập mã CTHD mới
            txtMaChiTietHD.Focus();// Focus vào ô nhập mã CTHD
            btnLuu.Enabled = true;// Cho phép nút Lưu
            btnThem.Enabled = false;// Chặn không cho bấm thêm nữa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaChiTietHD.Text == "") { MessageBox.Show("Bạn chưa chọn mục nào"); return; }
            txtMaChiTietHD.Enabled = false; // Không cho sửa khóa chính
            btnLuu.Enabled = true;// Cho phép nút Lưu
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;// Chặn không cho bấm sửa nữa
            cmbMaHoaDon.Focus(); // Focus vào control đầu tiên
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            //  Validation (Kiểm tra dữ liệu)
            if (txtMaChiTietHD.Text.Trim() == "") { MessageBox.Show("Mã chi tiết HĐ không rỗng!"); txtMaChiTietHD.Focus(); return; }
            if (cmbMaHoaDon.SelectedIndex == -1) { MessageBox.Show("Bạn phải chọn Hóa đơn!"); cmbMaHoaDon.Focus(); return; }
            if (cmbMaTIVI.SelectedIndex == -1) { MessageBox.Show("Bạn phải chọn TIVI!"); cmbMaTIVI.Focus(); return; }

            int soLuong;
            decimal donGia;
            if (!int.TryParse(txtSoLuongMua.Text, out soLuong)) { MessageBox.Show("Số lượng phải là số nguyên!"); txtSoLuongMua.Focus(); return; }
            if (!decimal.TryParse(txtDonGia.Text, out donGia)) { MessageBox.Show("Đơn giá phải là số!"); txtDonGia.Focus(); return; }

            // Lấy giá trị từ các controls
            string maCTHD = txtMaChiTietHD.Text.Trim();
            string maHD = cmbMaHoaDon.SelectedValue.ToString(); // Lấy giá trị ẩn (MaHoaDon)
            string maTIVI = cmbMaTIVI.SelectedValue.ToString(); // Lấy giá trị ẩn (MaTIVI)

            //  Phân biệt Thêm / Sửa
            if (txtMaChiTietHD.Enabled == true) 
            {
                //  Kiểm tra trùng Mã
                sql = "SELECT MaChiTietHoaDon FROM FormChiTietHoaDon WHERE MaChiTietHoaDon=N'" + maCTHD + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã chi tiết hóa đơn này đã tồn tại!");
                    txtMaChiTietHD.Focus();
                    return;
                }

                //  INSERT
                sql = "INSERT INTO FormChiTietHoaDon(MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) " +
                      "VALUES (N'" + maCTHD + "', N'" + maHD + "', N'" + maTIVI + "', " +
                      soLuong + ", " + donGia.ToString().Replace(',', '.') + ")"; // Định dạng số thập phân cho SQL
            }
            else // SỬA
            {
                // 2C. UPDATE
                sql = "UPDATE FormChiTietHoaDon SET " +
                      "MaHoaDon=N'" + maHD + "', " +
                      "MaTIVI=N'" + maTIVI + "', " +
                      "SoLuongMua=" + soLuong + ", " +
                      "DonGia=" + donGia.ToString().Replace(',', '.') + " " + // Định dạng số thập phân cho SQL
                      "WHERE MaChiTietHoaDon=N'" + maCTHD + "'";
            }

            //  Thực thi
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

            //  Reset nút
            btnLuu.Enabled = false;
            txtMaChiTietHD.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaChiTietHD.Text == "") { MessageBox.Show("Bạn chưa chọn mục nào"); return; }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa chi tiết hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FormChiTietHoaDon WHERE MaChiTietHoaDon=N'" + txtMaChiTietHD.Text + "'";
                Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvChiTietHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {//lý do dùng CellClick thay vì CellContentClick là vì CellClick sẽ phản hồi khi người dùng nhấp vào bất kỳ đâu trong ô, trong khi CellContentClick chỉ phản hồi khi người dùng nhấp vào nội dung bên trong ô (như văn bản hoặc hình ảnh).
         //Điều này giúp cải thiện trải nghiệm người dùng khi chọn hàng trong DataGridView.
            if (btnThem.Enabled == false) { return; }
            if (tblCTHD.Rows.Count == 0) { return; }
            if (e.RowIndex < 0) { return; } // Tránh click vào header

            // Gán dữ liệu lên controls
            txtMaChiTietHD.Text = dgvChiTietHoaDon.CurrentRow.Cells["MaChiTietHoaDon"].Value.ToString();

            // Gán giá trị cho ComboBox (dùng SelectedValue)
            cmbMaHoaDon.SelectedValue = dgvChiTietHoaDon.CurrentRow.Cells["MaHoaDon"].Value.ToString();
            cmbMaTIVI.SelectedValue = dgvChiTietHoaDon.CurrentRow.Cells["MaTIVI"].Value.ToString();

            txtSoLuongMua.Text = dgvChiTietHoaDon.CurrentRow.Cells["SoLuongMua"].Value.ToString();
            txtDonGia.Text = dgvChiTietHoaDon.CurrentRow.Cells["DonGia"].Value.ToString();

            // Cập nhật nút
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaChiTietHD.Enabled = false; // Tắt khóa chính
        }
    }
}
