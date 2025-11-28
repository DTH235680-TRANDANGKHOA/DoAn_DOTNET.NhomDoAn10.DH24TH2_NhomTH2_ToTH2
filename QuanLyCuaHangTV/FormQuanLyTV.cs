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
        DataTable tblTV; // Nó được dùng để lưu trữ dữ liệu được lấy về từ cơ sở dữ liệu (Database) hoặc được tạo ra cục bộ trong ứng dụng.
                         // Nó chứa các hàng (DataRow) và cột (DataColumn).
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
            //vô hiệu hóa các nút Lưu, Sửa, Xóa lúc khởi động
            txtMaTIVI.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

           

            // Tải dữ liệu chính
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            //Hàm này chịu trách nhiệm thực hiện các bước kết nối với cơ sở dữ liệu
            //chạy câu lệnh SQL (sql) đã cung cấp, và trả về dữ liệu kết quả dưới dạng một đối tượng DataTable.
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

            dgvQuanLyTV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //lấp đầy toàn bộ chiều rộng
            dgvQuanLyTV.AllowUserToAddRows = false; //loại bỏ hàng trống cuối cùng
            dgvQuanLyTV.EditMode = DataGridViewEditMode.EditProgrammatically; //không cho sửa trực tiếp
        }

       


        // --- HÀM RESET ---
        private void ResetValues()
        {
            // chức năng để xóa sạch các giá trị hiện có trên các điều khiển nhập liệu (TextBox, ComboBox, v.v.)
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
            ResetValues(); // xóa trắng các ô nhập liệu
            txtMaTIVI.Enabled = true;//cho phép nhập Mã TIVI
            txtMaTIVI.Focus();//đưa con trỏ đến ô Mã TIVI
            btnLuu.Enabled = true;//bật nút Lưu
            btnThem.Enabled = false;//tắt nút Thêm
            btnSua.Enabled = false;//tắt nút Sửa
            btnXoa.Enabled = false;//tắt nút Xóa
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTIVI.Text == "") { MessageBox.Show("Bạn chưa chọn Tivi nào"); return; }//kiểm tra chọn Tivi
            txtMaTIVI.Enabled = false;//không cho sửa Mã TIVI
            btnLuu.Enabled = true;//bật nút Lưu
            btnThem.Enabled = false;//tắt nút Thêm
            btnSua.Enabled = false;//tắt nút Sửa
            btnXoa.Enabled = false;//tắt nút Xóa
            txtTenTIVI.Focus();//đưa con trỏ đến ô Tên TIVI
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            // 1. Validation
            if (txtMaTIVI.Text.Trim() == "") { MessageBox.Show("Mã TIVI không rỗng!"); txtMaTIVI.Focus(); return; }//kiểm tra mã TIVI.Phương thức này loại bỏ tất cả khoảng trắng thừa ở đầu và cuối chuỗi. ngăn chặn nhâp khoảng trắng.
            if (txtTenTIVI.Text.Trim() == "") { MessageBox.Show("Tên TIVI không rỗng!"); txtTenTIVI.Focus(); return; }

            // Lấy giá trị từ ComboBox (dùng .Text cho phép thêm hãng mới)
            string hangSX = cmbHangSX.Text.Trim();
            if (hangSX == "") { MessageBox.Show("Hãng sản xuất không rỗng!"); cmbHangSX.Focus(); return; }


            // Validate số
            int soluong;
            decimal gia;//kiểu số thập phân
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
             
                ResetValues();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void dgvDanhSachTIVI_CellClick(object sender, DataGridViewCellEventArgs e)
        {//lý do dùng CellClick thay vì CellContentClick là vì CellClick được kích hoạt khi người dùng nhấp vào bất kỳ vị trí nào trong ô của DataGridView,


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

        }
    }
}
