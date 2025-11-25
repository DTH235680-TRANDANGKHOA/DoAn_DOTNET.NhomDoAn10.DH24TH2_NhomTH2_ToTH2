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
    public partial class FormHoaDon : Form
    {
        DataTable tblHoaDon;
        public FormHoaDon()
        {
            InitializeComponent();
        }

        private void FormHoaDon_Load(object sender, EventArgs e)
        {
            // CODE THÊM MỚI ĐỂ PHÓNG TO TOÀN MÀN HÌNH 
            this.WindowState = FormWindowState.Maximized;
          

            // 1. Thiết lập trạng thái ban đầu
            txtMaHoaDon.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            // 💡 CHỈNH SỬA: BỎ txtTongTien.Enabled = false; để cho phép nhập

            // 2. Tải các ComboBox (Phải tải trước)
            LoadComboBoxMaNhanVien();
            LoadComboBoxMaKhachHang();

            // 3. Tải Data Grid
            LoadDataGridView();

            // 4. Đặt mặc định ComboBox
            cmbMaNhanVien.SelectedIndex = -1;
            cmbMaKhachHang.SelectedIndex = -1;
        }
        // --- HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW ---
        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM FormHoaDon";
            tblHoaDon = Functions.GetDataToTable(sql);
            dgvHoaDon.DataSource = tblHoaDon;

            // Đặt tên cột
            dgvHoaDon.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
            dgvHoaDon.Columns["MaNhanVien"].HeaderText = "Mã Nhân Viên";
            dgvHoaDon.Columns["MaKhachHang"].HeaderText = "Mã Khách Hàng";
            dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
            dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";

            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // --- CÁC HÀM TẢI COMBOBOX (ĐÃ FIX HIỂN THỊ MÃ) ---
        private void LoadComboBoxMaNhanVien()
        {
            
            string sql = "SELECT MaNhanVien, HoTen FROM FormNhanVien";
            Functions.FillCombo(sql, cmbMaNhanVien, "MaNhanVien", "MaNhanVien");
        }

        private void LoadComboBoxMaKhachHang()
        {
           
            string sql = "SELECT MaKhachHang, HoTen FROM FormKhachHang";
            Functions.FillCombo(sql, cmbMaKhachHang, "MaKhachHang", "MaKhachHang");
        }


        // --- HÀM RESET CÁC Ô NHẬP ---
        private void ResetValues()
        {
            txtMaHoaDon.Text = "";
            dtpNgayLap.Value = DateTime.Now;
            cmbMaNhanVien.SelectedIndex = -1;
            cmbMaKhachHang.SelectedIndex = -1;
            txtTongTien.Text = "0";
            //Cho phép nhập Tổng tiền
            txtTongTien.Enabled = true;
        }
        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            txtMaHoaDon.Enabled = true; // Cho phép nhập mã HĐ mới
            txtMaHoaDon.Focus();
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTongTien.Enabled = true; // Bật nhập Tổng tiền khi thêm
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaHoaDon.Text == "") { MessageBox.Show("Bạn chưa chọn hóa đơn nào"); return; }
            txtMaHoaDon.Enabled = false; // Không cho sửa khóa chính
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            dtpNgayLap.Focus();
            txtTongTien.Enabled = true; // Bật nhập Tổng tiền khi sửa
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            // 1. Validation (Kiểm tra dữ liệu)
            if (txtMaHoaDon.Text.Trim() == "") { MessageBox.Show("Mã hóa đơn không rỗng!"); txtMaHoaDon.Focus(); return; }
            if (cmbMaNhanVien.SelectedIndex == -1) { MessageBox.Show("Bạn phải chọn nhân viên!"); cmbMaNhanVien.Focus(); return; }
            if (cmbMaKhachHang.SelectedIndex == -1) { MessageBox.Show("Bạn phải chọn khách hàng!"); cmbMaKhachHang.Focus(); return; }

            //Kiểm tra Tổng tiền có phải là số hợp lệ không
            decimal tongTien;
            if (txtTongTien.Text.Trim() == "")
            {
                MessageBox.Show("Tổng tiền không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTongTien.Focus();
                return;
            }
            if (!decimal.TryParse(txtTongTien.Text.Replace('.', ','), out tongTien)) // Thử Parse với định dạng thập phân
            {
                MessageBox.Show("Tổng tiền phải là một giá trị số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTongTien.Focus();
                return;
            }


            // Lấy giá trị từ các controls
            string maHD = txtMaHoaDon.Text.Trim();
            string maNV = cmbMaNhanVien.SelectedValue.ToString();
            string maKH = cmbMaKhachHang.SelectedValue.ToString();
            DateTime ngayLap = dtpNgayLap.Value;

            // Định dạng ngày giờ chuẩn cho SQL Server
            string ngaySQL = ngayLap.ToString("yyyy-MM-dd HH:mm:ss");

            // 2. Phân biệt Thêm / Sửa
            if (txtMaHoaDon.Enabled == true) // THÊM MỚI
            {
                // 2A. Kiểm tra trùng Mã
                sql = "SELECT MaHoaDon FROM FormHoaDon WHERE MaHoaDon=N'" + maHD + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã hóa đơn này đã tồn tại!");
                    txtMaHoaDon.Focus();
                    return;
                }

                // 2B. INSERT
                sql = "INSERT INTO FormHoaDon(MaHoaDon, MaNhanVien, MaKhachHang, NgayLap, TongTien) " +
                      "VALUES (N'" + maHD + "', N'" + maNV + "', N'" + maKH + "', '" +
                      ngaySQL + "', " + tongTien.ToString().Replace(',', '.') + ")";
            }
            else // SỬA
            {
                // 2C. UPDATE
                sql = "UPDATE FormHoaDon SET " +
                      "MaNhanVien=N'" + maNV + "', " +
                      "MaKhachHang=N'" + maKH + "', " +
                      "NgayLap='" + ngaySQL + "', " +
                      "TongTien=" + tongTien.ToString().Replace(',', '.') + " " +
                      "WHERE MaHoaDon=N'" + maHD + "'";
            }

            // 3. Thực thi
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

            // 4. Reset nút
            btnLuu.Enabled = false;
            txtMaHoaDon.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTongTien.Enabled = false; // Tắt nhập sau khi lưu
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaHoaDon.Text == "") { MessageBox.Show("Bạn chưa chọn hóa đơn nào"); return; }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?\n(LƯU Ý: Nên xóa chi tiết hóa đơn trước)", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string sql = "DELETE FormHoaDon WHERE MaHoaDon=N'" + txtMaHoaDon.Text + "'";
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

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false) { return; }
            if (tblHoaDon.Rows.Count == 0) { return; }
            if (e.RowIndex < 0) { return; } // Tránh click vào header

            // Gán dữ liệu lên controls
            txtMaHoaDon.Text = dgvHoaDon.CurrentRow.Cells["MaHoaDon"].Value.ToString();

            // Gán giá trị cho ComboBox (dùng SelectedValue)
            cmbMaNhanVien.SelectedValue = dgvHoaDon.CurrentRow.Cells["MaNhanVien"].Value.ToString();
            cmbMaKhachHang.SelectedValue = dgvHoaDon.CurrentRow.Cells["MaKhachHang"].Value.ToString();

            // Gán giá trị cho DateTimePicker (cần ép kiểu)
            if (dgvHoaDon.CurrentRow.Cells["NgayLap"].Value != DBNull.Value)
            {
                dtpNgayLap.Value = (DateTime)dgvHoaDon.CurrentRow.Cells["NgayLap"].Value;
            }
            else
            {
                dtpNgayLap.Value = DateTime.Now;
            }

            // Dữ liệu Tổng tiền hiển thị từ cột TongTien trong DataGridView
            txtTongTien.Text = dgvHoaDon.CurrentRow.Cells["TongTien"].Value.ToString();

            // Cập nhật nút
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaHoaDon.Enabled = false;
            txtTongTien.Enabled = false; // Tắt nhập khi chỉ xem
        }
    }
}
