using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;


namespace QuanLyCuaHangTV
{
    internal class Functions
    {
        public static SqlConnection Con;  //Khai báo đối tượng kết nối        

        public static void Connect()
        {


            // Cấu hình chuỗi kết nối 
            // Đã cập nhật chuỗi kết nối theo thông tin máy chủ của bạn
            string connectionString = @"Data Source=DESKTOP-77A826O;Initial Catalog=QuanLyCuaHangTV;Integrated Security=True;TrustServerCertificate=True";

            //  Khởi tạo đối tượng SqlConnection 
            Con = new SqlConnection(connectionString);
            
            //  Bổ sung kiểm tra để tránh lỗi 
            if (Con.State != ConnectionState.Open)
            {
                try
                {
                    Con.Open();
                    MessageBox.Show("Kết nối cơ sở dữ liệu thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kết nối thất bại: " + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void Disconnect()
        {
            if (Con != null && Con.State == ConnectionState.Open)
            {
                Con.Close();    //Đóng kết nối
                Con.Dispose();  //Giải phóng tài nguyên
                Con = null;
                // Bạn có thể bỏ thông báo này đi nếu không muốn nó hiện lên mỗi lần
                // MessageBox.Show("Đã ngắt kết nối cơ sở dữ liệu.", "Thông báo",
                //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Tạo đối tượng thuộc lớp SqlCommand
            dap.SelectCommand = new SqlCommand();
            dap.SelectCommand.Connection = Functions.Con; //Kết nối cơ sở dữ liệu
            dap.SelectCommand.CommandText = sql; //Lệnh SQL
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table);
            return table;
        }
        // Đặt hàm này trong class Functions của bạn (ví dụ: public static class Functions)

        public static string GetFieldValues(string sql)
        {
            string ma = ""; // Biến để lưu trữ giá trị duy nhất (Đơn giá, Tên,...)

            // Mở kết nối nếu chưa mở (Giả định bạn có hàm Connect / Close trong Functions)
            // Functions.Connect(); 

            SqlCommand cmd = new SqlCommand(sql, Con); // Giả định Con là SqlConnection object trong Functions
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Lấy giá trị của cột đầu tiên (index 0)
                ma = reader.GetValue(0).ToString();
            }

            reader.Close();
            // Functions.Disconnect(); // Đóng kết nối nếu cần

            return ma;
        }

        //Hàm kiểm tra khoá trùng
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, Con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chạy SQL: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }
        // Dán hàm này vào bên trong class Functions (file Functions.cs)

        /// <summary>
        /// Hàm nạp dữ liệu vào ComboBox
        /// </summary>
        /// <param name="sql">Câu lệnh SQL (SELECT Ma, Ten)</param>
        /// <param name="cmb">Đối tượng ComboBox cần nạp</param>
        /// <param name="valueMember">Tên cột giá trị (VD: MaNhanVien)</param>
        /// <param name="displayMember">Tên cột hiển thị (VD: HoTen)</param>
        public static void FillCombo(string sql, ComboBox cmb, string valueMember, string displayMember)
        {
            DataTable dt = GetDataToTable(sql);
            cmb.DataSource = dt;
            cmb.ValueMember = valueMember;     // Giá trị ẩn (để lưu)
            cmb.DisplayMember = displayMember; // Giá trị hiển thị
            cmb.SelectedIndex = -1; // Bỏ chọn lúc đầu
        }
    }
}