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
    public partial class FormTonKho : Form
    {
        DataTable tblTonKho;
        public FormTonKho()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormTonKho_Load(object sender, EventArgs e)
        {
            // CODE THÊM MỚI ĐỂ PHÓNG TO TOÀN MÀN HÌNH 
            this.WindowState = FormWindowState.Maximized;
          

            // 1. Thiết lập trạng thái ban đầu
            txtMaTonKho.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // 2. Tải ComboBox MaTIVI (Phải tải trước)
            LoadComboBoxMaTIVI();

            // 3. Tải Data Grid
            LoadDataGridView();

            // 4. Đặt giá trị mặc định cho ComboBox (để tránh lỗi)
            cmbMaTIVI.SelectedIndex = -1;
        }
        // --- HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW ---
        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM FormTonKho";
            tblTonKho = Functions.GetDataToTable(sql);
            dgvTonKho.DataSource = tblTonKho;

            // Đặt tên cột
            dgvTonKho.Columns["MaTonKho"].HeaderText = "Mã Tồn Kho";
            dgvTonKho.Columns["MaTIVI"].HeaderText = "Mã TIVI";
            dgvTonKho.Columns["NgayCapNhat"].HeaderText = "Ngày Cập Nhật";
            dgvTonKho.Columns["SoLuongTon"].HeaderText = "Số Lượng Tồn";
            dgvTonKho.Columns["GhiChu"].HeaderText = "Ghi Chú";

            dgvTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTonKho.AllowUserToAddRows = false;
            dgvTonKho.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // --- HÀM TẢI DỮ LIỆU LÊN COMBOBOX MATIVI ---
        private void LoadComboBoxMaTIVI()
        {
            // Lấy cả MaTIVI và TenTIVI
            string sql = "SELECT MaTIVI, TenTIVI FROM FormQuanLyTV";
            DataTable dt = Functions.GetDataToTable(sql);

            // Thiết lập cho ComboBox
            cmbMaTIVI.DataSource = dt;

            // ValueMember: Giá trị ẩn (dùng để lưu vào CSDL)
            cmbMaTIVI.ValueMember = "MaTIVI";

            // DisplayMember: Giá trị hiển thị (người dùng nhìn thấy)
            
            cmbMaTIVI.DisplayMember = "MaTIVI";
        }

        // --- HÀM RESET CÁC Ô NHẬP ---
        private void ResetValues()
        {
            txtMaTonKho.Text = "";
            cmbMaTIVI.SelectedIndex = -1; // Xóa chọn ComboBox
            dtpNgayCapNhat.Value = DateTime.Now; // Đặt lại ngày hiện tại
            txtSoLuongTon.Text = "0"; // Mặc định là 0
            txtGhiChu.Text = "";
        }
        private void dgvDanhSachTonKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        
       
        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            txtMaTonKho.Enabled = true; // Cho phép nhập mã Tồn kho mới
            txtMaTonKho.Focus();
            btnLuu.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTonKho.Text == "") { MessageBox.Show("Bạn chưa chọn mục nào"); return; }

            txtMaTonKho.Enabled = false; // Không cho sửa khóa chính
            btnLuu.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtSoLuongTon.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            // 1. Validation (Kiểm tra dữ liệu)
            if (txtMaTonKho.Text.Trim() == "") { MessageBox.Show("Mã tồn kho không rỗng!"); txtMaTonKho.Focus(); return; }

            // Kiểm tra ComboBox đã được chọn chưa
            if (cmbMaTIVI.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn phải chọn Tivi!");
                cmbMaTIVI.Focus();
                return;
            }

            // Kiểm tra Số lượng tồn là số nguyên
            int soLuongTon;
            if (!int.TryParse(txtSoLuongTon.Text, out soLuongTon))
            {
                MessageBox.Show("Số lượng tồn phải là số!");
                txtSoLuongTon.Focus();
                return;
            }

            // Lấy giá trị từ các controls
            string maTonKho = txtMaTonKho.Text.Trim();
            string maTIVI = cmbMaTIVI.SelectedValue.ToString();
            DateTime ngayCapNhat = dtpNgayCapNhat.Value;

            // 2. Phân biệt Thêm / Sửa
            if (txtMaTonKho.Enabled == true) // THÊM MỚI
            {
                // 2A. Kiểm tra trùng Mã
                sql = "SELECT MaTonKho FROM FormTonKho WHERE MaTonKho=N'" + maTonKho + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã tồn kho này đã tồn tại!");
                    txtMaTonKho.Focus();
                    return;
                }

                // 2B. INSERT
                string ngaySQL = ngayCapNhat.ToString("yyyy-MM-dd");

                sql = "INSERT INTO FormTonKho(MaTonKho, MaTIVI, NgayCapNhat, SoLuongTon, GhiChu) " +
                      "VALUES (N'" + maTonKho + "', N'" + maTIVI + "', '" + ngaySQL + "', " +
                      soLuongTon + ", N'" + txtGhiChu.Text.Trim() + "')";
            }
            else // SỬA
            {
                // 2C. UPDATE
                string ngaySQL = ngayCapNhat.ToString("yyyy-MM-dd");

                sql = "UPDATE FormTonKho SET " +
                      "MaTIVI=N'" + maTIVI + "', " +
                      "NgayCapNhat='" + ngaySQL + "', " +
                      "SoLuongTon=" + soLuongTon + ", " +
                      "GhiChu=N'" + txtGhiChu.Text.Trim() + "' " +
                      "WHERE MaTonKho=N'" + maTonKho + "'";
            }

            // 3. Thực thi
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

            // 4. Reset nút
            btnLuu.Enabled = false;
            txtMaTonKho.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaTonKho.Text == "") { MessageBox.Show("Bạn chưa chọn mục nào"); return; }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa mục này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FormTonKho WHERE MaTonKho=N'" + txtMaTonKho.Text + "'";
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

        private void txtMaTIVI_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNgayCapNhat_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvTonKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false) { return; }
            if (tblTonKho.Rows.Count == 0) { return; }
            if (e.RowIndex < 0) { return; } // Tránh click vào header

            // Lấy dữ liệu từ Grid
            txtMaTonKho.Text = dgvTonKho.CurrentRow.Cells["MaTonKho"].Value.ToString();

            // GÁN GIÁ TRỊ CHO COMBOBOX (Dùng SelectedValue là Mã TIVI)
            string maTIVI_TuGrid = dgvTonKho.CurrentRow.Cells["MaTIVI"].Value.ToString();
            cmbMaTIVI.SelectedValue = maTIVI_TuGrid;

            // Gán giá trị cho DateTimePicker
            dtpNgayCapNhat.Value = (DateTime)dgvTonKho.CurrentRow.Cells["NgayCapNhat"].Value;

            txtSoLuongTon.Text = dgvTonKho.CurrentRow.Cells["SoLuongTon"].Value.ToString();
            txtGhiChu.Text = dgvTonKho.CurrentRow.Cells["GhiChu"].Value.ToString();

            // Cập nhật nút
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaTonKho.Enabled = false;
        }
    }
}
