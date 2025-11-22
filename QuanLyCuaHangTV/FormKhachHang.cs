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

    public partial class FormKhachHang : Form
    {
        DataTable tblKhachHang;
        public FormKhachHang()
        {
            InitializeComponent();
        }

        private void FormKhachHang_Load(object sender, EventArgs e)
        {
            // CODE THÊM MỚI ĐỂ PHÓNG TO TOÀN MÀN HÌNH 
            this.WindowState = FormWindowState.Maximized;
           
            //Thiết lập trạng thái ban đầu
            txtMaKhachHang.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            //Tải dữ liệu
            LoadDataGridView();

            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        // --- HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW ---
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaKhachHang, HoTen, DiaChi, SoDienThoai FROM FormKhachHang";
            tblKhachHang = Functions.GetDataToTable(sql);
            dgvKhachHang.DataSource = tblKhachHang;

            dgvKhachHang.Columns[0].HeaderText = "Mã Khách Hàng";
            dgvKhachHang.Columns[1].HeaderText = "Họ Tên";
            dgvKhachHang.Columns[2].HeaderText = "Địa Chỉ";
            dgvKhachHang.Columns[3].HeaderText = "SĐT";

            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // --- HÀM RESET CÁC Ô NHẬP ---
        private void ResetValues()
        {
            txtMaKhachHang.Text = "";
            txtHoTen.Text = "";
            txtDiaChi.Text = "";
            txtSDT.Text = "";
        }
        private void dgvKhacHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false) { return; }
            if (tblKhachHang.Rows.Count == 0) { return; }

            txtMaKhachHang.Text = dgvKhachHang.CurrentRow.Cells["MaKhachHang"].Value.ToString();
            txtHoTen.Text = dgvKhachHang.CurrentRow.Cells["HoTen"].Value.ToString();
            txtDiaChi.Text = dgvKhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
            txtSDT.Text = dgvKhachHang.CurrentRow.Cells["SoDienThoai"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaKhachHang.Enabled = false;
        }

       
        
        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            txtMaKhachHang.Enabled = true;
            txtMaKhachHang.Focus();

            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaKhachHang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn khách hàng nào để sửa");
                return;
            }

            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtMaKhachHang.Enabled = false;
            txtHoTen.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            // 1. Validation
            if (txtMaKhachHang.Text.Trim().Length == 0) { MessageBox.Show("Bạn phải nhập mã khách hàng"); txtMaKhachHang.Focus(); return; }
            if (txtHoTen.Text.Trim().Length == 0) { MessageBox.Show("Bạn phải nhập tên khách hàng"); txtHoTen.Focus(); return; }

            // 2. Phân biệt Thêm / Sửa
            if (txtMaKhachHang.Enabled == true) // THÊM MỚI
            {
                // 2A. Kiểm tra trùng Mã
                sql = "SELECT MaKhachHang FROM FormKhachHang WHERE MaKhachHang=N'" + txtMaKhachHang.Text.Trim() + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã khách hàng này đã tồn tại!");
                    txtMaKhachHang.Focus();
                    return;
                }

                // 2B. INSERT
                sql = "INSERT INTO FormKhachHang(MaKhachHang, HoTen, DiaChi, SoDienThoai) " +
                      "VALUES (N'" + txtMaKhachHang.Text.Trim() + "', " +
                              "N'" + txtHoTen.Text.Trim() + "', " +
                              "N'" + txtDiaChi.Text.Trim() + "', '" +
                              txtSDT.Text.Trim() + "')";
            }
            else // SỬA
            {
                // 2C. UPDATE
                sql = "UPDATE FormKhachHang SET " +
                      "HoTen=N'" + txtHoTen.Text.Trim() + "', " +
                      "DiaChi=N'" + txtDiaChi.Text.Trim() + "', " +
                      "SoDienThoai='" + txtSDT.Text.Trim() + "' " +
                      "WHERE MaKhachHang=N'" + txtMaKhachHang.Text + "'";
            }

            // 3. Thực thi
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

            // 4. Reset nút
            btnLuu.Enabled = false;
            txtMaKhachHang.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaKhachHang.Text == "") { MessageBox.Show("Bạn chưa chọn khách hàng nào"); return; }
            if (tblKhachHang.Rows.Count == 0) { return; }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FormKhachHang WHERE MaKhachHang=N'" + txtMaKhachHang.Text + "'";
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

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra 1: Nếu đang ở chế độ THÊM (nút Thêm đang tắt) thì không làm gì
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ Thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra 2: Nếu bảng không có dữ liệu thì không làm gì
            if (tblKhachHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Gán dữ liệu từ dòng được chọn lên các TextBox
            txtMaKhachHang.Text = dgvKhachHang.CurrentRow.Cells["MaKhachHang"].Value.ToString();
            txtHoTen.Text = dgvKhachHang.CurrentRow.Cells["HoTen"].Value.ToString();
            txtDiaChi.Text = dgvKhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
            txtSDT.Text = dgvKhachHang.CurrentRow.Cells["SoDienThoai"].Value.ToString();

            // Cập nhật trạng thái các nút: BẬT Sửa, BẬT Xóa
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false; // Tắt nút Lưu
            txtMaKhachHang.Enabled = false; // Tắt ô Mã KH (không cho sửa Khóa chính)
        }
    }
}
