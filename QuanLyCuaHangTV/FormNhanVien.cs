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
    public partial class FormNhanVien : Form
    {
        DataTable tblNhanVien; // Biến lưu trữ bảng nhân viên
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void dgvDanhSachNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            // CODE THÊM MỚI ĐỂ PHÓNG TO TOÀN MÀN HÌNH 
            this.WindowState = FormWindowState.Maximized;
            
            // Thiết lập trạng thái ban đầu
            txtMaNV.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Tải dữ liệu
            LoadDataGridView();

        }

        // Ghi chú: Hàm tải dữ liệu từ CSDL lên DataGridView
        private void LoadDataGridView()
        {
            string sql = "SELECT MaNhanVien, HoTen, ChucVu, Luong FROM FormNhanVien";// Lệnh SQL lấy toàn bộ dữ liệu
            tblNhanVien = Functions.GetDataToTable(sql);// Đọc dữ liệu từ CSDL
            dgvNhanVien.DataSource = tblNhanVien;// Đưa dữ liệu lên DataGridView

            // Đặt tên cột
            dgvNhanVien.Columns[0].HeaderText = "Mã Nhân Viên";
            dgvNhanVien.Columns[1].HeaderText = "Họ Tên";
            dgvNhanVien.Columns[2].HeaderText = "Chức Vụ";
            dgvNhanVien.Columns[3].HeaderText = "Lương";

            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// Tự động điều chỉnh kích thước cột
            dgvNhanVien.AllowUserToAddRows = false;// Không cho người dùng thêm dữ liệu trực tiếp
            dgvNhanVien.EditMode = DataGridViewEditMode.EditProgrammatically;// Không cho sửa dữ liệu trực tiếp
        }

        // --- HÀM RESET CÁC Ô NHẬP ---
        private void ResetValues()
        {
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            txtChucVu.Text = "";
            txtLuong.Text = "";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
             ResetValues();
            txtMaNV.Enabled = true; // Cho phép nhập mã mới
            txtMaNV.Focus();

            // Cập nhật trạng thái nút
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "") { MessageBox.Show("Bạn chưa chọn nhân viên nào"); return; }

            txtMaNV.Enabled = false; // Không cho sửa Khóa chính
            btnLuu.Enabled = true;// Kích hoạt nút Lưu
            btnThem.Enabled = false;// Vô hiệu nút Thêm
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtHoTen.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            // 1. Validation
            if (txtMaNV.Text.Trim() == "") { MessageBox.Show("Mã nhân viên không được rỗng!"); txtMaNV.Focus(); return; }
            if (txtHoTen.Text.Trim() == "") { MessageBox.Show("Họ tên không được rỗng!"); txtHoTen.Focus(); return; }

            //  chuyển đổi Lương sang số
            decimal luong = 0;
            if (!decimal.TryParse(txtLuong.Text, out luong))
            {
                MessageBox.Show("Lương phải là một con số!");
                txtLuong.Focus();
                return;
            }


            // 2. Phân biệt Thêm / Sửa
            if (txtMaNV.Enabled == true) // Chế độ THÊM MỚI
            {
                // 2A. Kiểm tra trùng Mã
                sql = "SELECT MaNhanVien FROM FormNhanVien WHERE MaNhanVien=N'" + txtMaNV.Text.Trim() + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã nhân viên này đã tồn tại!");
                    txtMaNV.Focus();
                    return;
                }

                // 2B. INSERT
                sql = "INSERT INTO FormNhanVien(MaNhanVien, HoTen, ChucVu, Luong) " +
                      "VALUES (N'" + txtMaNV.Text.Trim() + "', N'" + txtHoTen.Text.Trim() + "', N'" +
                      txtChucVu.Text.Trim() + "', " + luong + ")";
            }
            else // Chế độ SỬA
            {
                // 2C. UPDATE
                sql = "UPDATE FormNhanVien SET " +
                      "HoTen=N'" + txtHoTen.Text.Trim() + "', " +
                      "ChucVu=N'" + txtChucVu.Text.Trim() + "', " +
                      "Luong=" + luong + " " +
                      "WHERE MaNhanVien=N'" + txtMaNV.Text + "'";
            }

            // 3. Thực thi và tải lại
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

            // Đặt lại trạng thái nút
            btnLuu.Enabled = false;
            txtMaNV.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn nhân viên nào");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FormNhanVien WHERE MaNhanVien=N'" + txtMaNV.Text + "'";
                Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        { //lý do tạo hàm này là để khi bấm vào 1 ô bất kỳ trong DataGridView thì nó sẽ hiện thị dữ liệu lên các TextBox tương ứng
            // còn dgvDanhSachNhanVien_CellContentClick thì chỉ khi bấm vào nội dung trong ô thì mới hiện thị dữ liệu lên các TextBox
            // Lấy code từ hàm "dgvDanhSachNhanVien_CellContentClick" cũ 
            if (btnThem.Enabled == false) { return; } // Đang ở chế độ Thêm
            if (tblNhanVien.Rows.Count == 0) { return; } // Không có dữ liệu

            // Hiển thị dữ liệu lên controls
            // GHI CHÚ: Đảm bảo TextBox của bạn tên là txtMaNV
            txtMaNV.Text = dgvNhanVien.CurrentRow.Cells["MaNhanVien"].Value.ToString();
            txtHoTen.Text = dgvNhanVien.CurrentRow.Cells["HoTen"].Value.ToString();
            txtChucVu.Text = dgvNhanVien.CurrentRow.Cells["ChucVu"].Value.ToString();
            txtLuong.Text = dgvNhanVien.CurrentRow.Cells["Luong"].Value.ToString();

            // Kích hoạt các nút Sửa, Xóa
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNV.Enabled = false;
        }
    }
}
