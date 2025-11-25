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
    public partial class FormQuanLyTV : Form
    {
        DataTable tblTV;
        public FormQuanLyTV()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormQuanLyTV_Load(object sender, EventArgs e)
        {
            // CODE THÊM MỚI ĐỂ PHÓNG TO TOÀN MÀN HÌNH 
            this.WindowState = FormWindowState.Maximized;
            
            txtMaTIVI.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Tải danh sách Hãng SX vào ComboBox
            LoadComboBoxHangSX();

            // Tải dữ liệu chính
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM FormQuanLyTV";
            tblTV = Functions.GetDataToTable(sql);
            dgvQuanLyTV.DataSource = tblTV;

            // Đặt tên cột
            dgvQuanLyTV.Columns["MaTIVI"].HeaderText = "Mã TIVI";
            dgvQuanLyTV.Columns["TenTIVI"].HeaderText = "Tên TIVI";
            dgvQuanLyTV.Columns["KichThuoc"].HeaderText = "Kích Thước";
            dgvQuanLyTV.Columns["HangSanXuat"].HeaderText = "Hãng SX";
            dgvQuanLyTV.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvQuanLyTV.Columns["Gia"].HeaderText = "Giá";
            dgvQuanLyTV.Columns["BaoHanh"].HeaderText = "Bảo Hành";

            dgvQuanLyTV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuanLyTV.AllowUserToAddRows = false;
            dgvQuanLyTV.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // --- HÀM MỚI: TẢI DỮ LIỆU CHO COMBOBOX HÃNG SẢN XUẤT ---
        private void LoadComboBoxHangSX()
        {
            // Lấy danh sách các hãng (không trùng lặp)
            string sql = "SELECT DISTINCT HangSanXuat FROM FormQuanLyTV";
            DataTable dt = Functions.GetDataToTable(sql);

            // Gán nguồn dữ liệu cho ComboBox
            cmbHangSX.DataSource = dt;
            cmbHangSX.DisplayMember = "HangSanXuat"; // Cột để hiển thị
            cmbHangSX.ValueMember = "HangSanXuat";   // Cột để lấy giá trị
            cmbHangSX.SelectedIndex = -1; // Bỏ chọn lúc đầu
        }


        // --- HÀM RESET ---
        private void ResetValues()
        {
            txtMaTIVI.Text = "";
            txtTenTIVI.Text = "";
            txtKichThuoc.Text = "";

            // Dùng SelectedIndex = -1 để xóa chọn ComboBox
            cmbHangSX.SelectedIndex = -1;

            txtSoLuong.Text = "";
            txtGia.Text = "";
            txtBaoHanh.Text = "";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            txtMaTIVI.Enabled = true;
            txtMaTIVI.Focus();
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTIVI.Text == "") { MessageBox.Show("Bạn chưa chọn Tivi nào"); return; }
            txtMaTIVI.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTenTIVI.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            // 1. Validation
            if (txtMaTIVI.Text.Trim() == "") { MessageBox.Show("Mã TIVI không rỗng!"); txtMaTIVI.Focus(); return; }
            if (txtTenTIVI.Text.Trim() == "") { MessageBox.Show("Tên TIVI không rỗng!"); txtTenTIVI.Focus(); return; }

            // Lấy giá trị từ ComboBox (dùng .Text cho phép thêm hãng mới)
            string hangSX = cmbHangSX.Text.Trim();
            if (hangSX == "") { MessageBox.Show("Hãng sản xuất không rỗng!"); cmbHangSX.Focus(); return; }


            // Validate số
            int soluong;
            decimal gia;
            if (!int.TryParse(txtSoLuong.Text, out soluong)) { MessageBox.Show("Số lượng phải là số!"); txtSoLuong.Focus(); return; }
            if (!decimal.TryParse(txtGia.Text, out gia)) { MessageBox.Show("Giá phải là số!"); txtGia.Focus(); return; }


            // 2. Phân biệt Thêm / Sửa
            if (txtMaTIVI.Enabled == true) // THÊM MỚI
            {
                // 2A. Kiểm tra trùng Mã
                sql = "SELECT MaTIVI FROM FormQuanLyTV WHERE MaTIVI=N'" + txtMaTIVI.Text.Trim() + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã TIVI này đã tồn tại!");
                    txtMaTIVI.Focus();
                    return;
                }

                // 2B. INSERT (Dùng hangSX đã lấy ở trên)
                sql = "INSERT INTO FormQuanLyTV(MaTIVI, TenTIVI, KichThuoc, HangSanXuat, SoLuong, Gia, BaoHanh) " +
                      "VALUES (N'" + txtMaTIVI.Text.Trim() + "', N'" + txtTenTIVI.Text.Trim() + "', N'" +
                      txtKichThuoc.Text.Trim() + "', N'" + hangSX + "', " +
                      soluong + ", " + gia + ", N'" + txtBaoHanh.Text.Trim() + "')";
            }
            else 
            {
                // 2C. UPDATE (Dùng hangSX đã lấy ở trên)
                sql = "UPDATE FormQuanLyTV SET " +
                      "TenTIVI=N'" + txtTenTIVI.Text.Trim() + "', " +
                      "KichThuoc=N'" + txtKichThuoc.Text.Trim() + "', " +
                      "HangSanXuat=N'" + hangSX + "', " +
                      "SoLuong=" + soluong + ", " +
                      "Gia=" + gia + ", " +
                      "BaoHanh=N'" + txtBaoHanh.Text.Trim() + "' " +
                      "WHERE MaTIVI=N'" + txtMaTIVI.Text + "'";
            }

            // 3. Thực thi
            Functions.RunSQL(sql);

            // 4. Tải lại Grid và ComboBox (phòng trường hợp có hãng mới)
            LoadDataGridView();
            LoadComboBoxHangSX();
            ResetValues();

            // 5. Reset nút
            btnLuu.Enabled = false;
            txtMaTIVI.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaTIVI.Text == "") { MessageBox.Show("Bạn chưa chọn Tivi nào"); return; }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Tivi này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FormQuanLyTV WHERE MaTIVI=N'" + txtMaTIVI.Text + "'";
                Functions.RunSQL(sql);
                LoadDataGridView();
                LoadComboBoxHangSX(); // Tải lại ComboBox
                ResetValues();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void dgvDanhSachTIVI_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           

            if (btnThem.Enabled == false) { return; }
            if (tblTV.Rows.Count == 0) { return; }

            // Gán dữ liệu lên controls
            txtMaTIVI.Text = dgvQuanLyTV.CurrentRow.Cells["MaTIVI"].Value.ToString();
            txtTenTIVI.Text = dgvQuanLyTV.CurrentRow.Cells["TenTIVI"].Value.ToString();
            txtKichThuoc.Text = dgvQuanLyTV.CurrentRow.Cells["KichThuoc"].Value.ToString();

            // Gán giá trị cho ComboBox (dùng .Text là đúng)
            cmbHangSX.Text = dgvQuanLyTV.CurrentRow.Cells["HangSanXuat"].Value.ToString();

            txtSoLuong.Text = dgvQuanLyTV.CurrentRow.Cells["SoLuong"].Value.ToString();
            txtGia.Text = dgvQuanLyTV.CurrentRow.Cells["Gia"].Value.ToString();
            txtBaoHanh.Text = dgvQuanLyTV.CurrentRow.Cells["BaoHanh"].Value.ToString();

            // Bật nút Sửa / Xóa
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaTIVI.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void dgvDanhSachTIVI_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false) { return; }
            if (tblTV.Rows.Count == 0) { return; }

            txtMaTIVI.Text = dgvQuanLyTV.CurrentRow.Cells["MaTIVI"].Value.ToString();
            txtTenTIVI.Text = dgvQuanLyTV.CurrentRow.Cells["TenTIVI"].Value.ToString();
            txtKichThuoc.Text = dgvQuanLyTV.CurrentRow.Cells["KichThuoc"].Value.ToString();

            // Gán giá trị cho ComboBox (dùng .Text là cách đơn giản và đúng)
            cmbHangSX.Text = dgvQuanLyTV.CurrentRow.Cells["HangSanXuat"].Value.ToString();

            txtSoLuong.Text = dgvQuanLyTV.CurrentRow.Cells["SoLuong"].Value.ToString();
            txtGia.Text = dgvQuanLyTV.CurrentRow.Cells["Gia"].Value.ToString();
            txtBaoHanh.Text = dgvQuanLyTV.CurrentRow.Cells["BaoHanh"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaTIVI.Enabled = false;
        }
    }
}
