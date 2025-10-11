using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.IO;

namespace THỰC_HÀNH_CSDL_VỚI_C_
{
    public partial class frmHang : Form
    {
        public static string userName = "";
        private string selectedImagePath = "";

        public frmHang()
        {
            InitializeComponent();
        }

        private void frmHang_Load(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Hiển thị form đăng nhập
                frmDangNhap frmDN = new frmDangNhap();
                frmDN.ShowDialog();

                // Bước 2: Kiểm tra đăng nhập thành công
                if (string.IsNullOrEmpty(userName))
                {
                    MessageBox.Show("Bạn cần đăng nhập để sử dụng hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                    return;
                }

                // Bước 3: Hiển thị thông tin người dùng
                lblDangNhap.Text = "Xin chào: " + userName;
                
                // Bước 4: Khởi tạo giao diện
                LoadComboBox();    // Tải danh sách chất liệu
                LoadData();        // Tải danh sách hàng hóa
                
                MessageBox.Show("Đăng nhập thành công! Chào mừng bạn đến với hệ thống quản lý hàng hóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo ứng dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        void LoadData()
        {
            try
            {
                // Bước 1: Lấy connection string từ App.config
                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                
                // Bước 2: Tạo kết nối và truy vấn dữ liệu
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Bước 3: Tạo câu lệnh SQL để lấy tất cả hàng hóa
                    string sql = "SELECT MaHang, TenHang, ChatLieu, SoLuong, DonGiaNhap, DonGiaBan, Anh FROM tblHang ORDER BY MaHang";
                    
                    // Bước 4: Sử dụng SqlDataAdapter để lấy dữ liệu
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    
                    // Bước 5: Điền dữ liệu vào DataTable
                    da.Fill(dt);
                    
                    // Bước 6: Hiển thị dữ liệu lên DataGridView
                    dgvHang.DataSource = dt;
                    
                    // Bước 7: Định dạng cột hiển thị
                    if (dgvHang.Columns.Count > 0)
                    {
                        dgvHang.Columns["MaHang"].HeaderText = "Mã Hàng";
                        dgvHang.Columns["TenHang"].HeaderText = "Tên Hàng";
                        dgvHang.Columns["ChatLieu"].HeaderText = "Chất Liệu";
                        dgvHang.Columns["SoLuong"].HeaderText = "Số Lượng";
                        dgvHang.Columns["DonGiaNhap"].HeaderText = "Đơn Giá Nhập";
                        dgvHang.Columns["DonGiaBan"].HeaderText = "Đơn Giá Bán";
                        dgvHang.Columns["Anh"].HeaderText = "Ảnh";
                        
                        // Định dạng số tiền
                        dgvHang.Columns["DonGiaNhap"].DefaultCellStyle.Format = "N0";
                        dgvHang.Columns["DonGiaBan"].DefaultCellStyle.Format = "N0";
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadComboBox()
        {
            cbChatLieu.Items.AddRange(new string[] { "Vàng", "Bông", "Mây", "Tre", "Tổng hợp" });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Kiểm tra dữ liệu đầu vào
                if (!ValidateInput())
                {
                    return; // Dừng nếu dữ liệu không hợp lệ
                }

                // Bước 2: Kiểm tra mã hàng trùng
                if (CheckDuplicateMaHang(txtMaHang.Text.Trim()))
                {
                    MessageBox.Show("Mã hàng '" + txtMaHang.Text + "' đã tồn tại trong hệ thống!\nVui lòng nhập mã hàng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaHang.Focus();
                    return;
                }

                // Bước 3: Kiểm tra đơn giá bán > đơn giá nhập
                float donGiaNhap = float.Parse(txtDonGiaNhap.Text);
                float donGiaBan = float.Parse(txtDonGiaBan.Text);
                if (donGiaBan <= donGiaNhap)
                {
                    MessageBox.Show("Đơn giá bán (" + donGiaBan.ToString("N0") + ") phải lớn hơn đơn giá nhập (" + donGiaNhap.ToString("N0") + ")!\nVui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaBan.Focus();
                    return;
                }

                // Bước 4: Thực hiện thêm vào database
                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Tạo câu lệnh SQL INSERT
                    string sql = @"INSERT INTO tblHang (MaHang, TenHang, ChatLieu, SoLuong, DonGiaNhap, DonGiaBan, Anh) 
                                   VALUES (@ma, @ten, @chatlieu, @soluong, @dongianhap, @dongiaban, @anh)";
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    
                    // Thêm parameters để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@ma", txtMaHang.Text.Trim());
                    cmd.Parameters.AddWithValue("@ten", txtTenHang.Text.Trim());
                    cmd.Parameters.AddWithValue("@chatlieu", cbChatLieu.Text);
                    cmd.Parameters.AddWithValue("@soluong", int.Parse(txtSoLuong.Text));
                    cmd.Parameters.AddWithValue("@dongianhap", donGiaNhap);
                    cmd.Parameters.AddWithValue("@dongiaban", donGiaBan);
                    cmd.Parameters.AddWithValue("@anh", string.IsNullOrEmpty(selectedImagePath) ? "" : Path.GetFileName(selectedImagePath));

                    // Mở kết nối và thực thi
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        // Bước 5: Cập nhật giao diện
                        LoadData();        // Tải lại danh sách
                        ClearInputs();     // Xóa trắng form
                        MessageBox.Show("Thêm hàng hóa '" + txtTenHang.Text + "' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm hàng hóa. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi database khi thêm hàng hóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Kiểm tra đã chọn hàng cần sửa
                if (dgvHang.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa cần sửa từ danh sách bên dưới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Bước 2: Kiểm tra dữ liệu đầu vào
                if (!ValidateInput())
                {
                    return; // Dừng nếu dữ liệu không hợp lệ
                }

                // Bước 3: Parse dữ liệu số (đã được ValidateInput kiểm tra)
                float donGiaNhap = float.Parse(txtDonGiaNhap.Text.Trim());
                float donGiaBan = float.Parse(txtDonGiaBan.Text.Trim());

                // Bước 4: Xác nhận sửa
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn sửa thông tin hàng hóa '" + txtTenHang.Text + "'?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                // Bước 5: Thực hiện cập nhật database
                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Tạo câu lệnh SQL UPDATE
                    string sql = @"UPDATE tblHang 
                                   SET TenHang=@ten, ChatLieu=@chatlieu, SoLuong=@soluong, 
                                       DonGiaNhap=@dongianhap, DonGiaBan=@dongiaban, Anh=@anh 
                                   WHERE MaHang=@ma";
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    
                    // Thêm parameters
                    cmd.Parameters.AddWithValue("@ma", txtMaHang.Text.Trim());
                    cmd.Parameters.AddWithValue("@ten", txtTenHang.Text.Trim());
                    cmd.Parameters.AddWithValue("@chatlieu", cbChatLieu.Text);
                    cmd.Parameters.AddWithValue("@soluong", int.Parse(txtSoLuong.Text));
                    cmd.Parameters.AddWithValue("@dongianhap", donGiaNhap);
                    cmd.Parameters.AddWithValue("@dongiaban", donGiaBan);
                    cmd.Parameters.AddWithValue("@anh", string.IsNullOrEmpty(selectedImagePath) ? "" : Path.GetFileName(selectedImagePath));

                    // Mở kết nối và thực thi
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        // Bước 6: Cập nhật giao diện
                        LoadData();        // Tải lại danh sách
                        MessageBox.Show("Sửa thông tin hàng hóa '" + txtTenHang.Text + "' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể sửa thông tin hàng hóa. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi database khi sửa hàng hóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Kiểm tra đã chọn hàng cần xóa
                if (dgvHang.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa cần xóa từ danh sách bên dưới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Bước 2: Lấy thông tin hàng hóa cần xóa
                string maHang = txtMaHang.Text.Trim();
                string tenHang = txtTenHang.Text.Trim();
                
                if (string.IsNullOrEmpty(maHang))
                {
                    MessageBox.Show("Không thể xác định hàng hóa cần xóa. Vui lòng chọn lại từ danh sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Bước 3: Xác nhận xóa với thông tin chi tiết
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn XÓA hàng hóa sau?\n\n" +
                    "• Mã hàng: " + maHang + "\n" +
                    "• Tên hàng: " + tenHang + "\n\n" +
                    "⚠️ Hành động này KHÔNG THỂ HOÀN TÁC!",
                    "Xác nhận xóa hàng hóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Bước 4: Thực hiện xóa từ database
                    string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        // Tạo câu lệnh SQL DELETE
                        string sql = "DELETE FROM tblHang WHERE MaHang=@ma";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ma", maHang);

                        // Mở kết nối và thực thi
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Bước 5: Cập nhật giao diện
                            LoadData();        // Tải lại danh sách
                            ClearInputs();     // Xóa trắng form
                            MessageBox.Show("Xóa hàng hóa '" + tenHang + "' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa hàng hóa. Hàng hóa có thể đã bị xóa hoặc không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi database khi xóa hàng hóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Tạo OpenFileDialog để chọn ảnh
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                // Bước 2: Thiết lập bộ lọc file ảnh
                openFileDialog.Filter = "Tất cả file ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif|" +
                                       "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                       "PNG files (*.png)|*.png|" +
                                       "Bitmap files (*.bmp)|*.bmp|" +
                                       "GIF files (*.gif)|*.gif";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Chọn ảnh sản phẩm";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                
                // Bước 3: Hiển thị dialog và xử lý kết quả
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    
                    // Bước 4: Kiểm tra file có tồn tại không
                    if (File.Exists(selectedImagePath))
                    {
                        // Bước 5: Hiển thị ảnh lên PictureBox
                        picAnh.Image = System.Drawing.Image.FromFile(selectedImagePath);
                        
                        // Bước 6: Hiển thị thông tin file
                        string fileName = Path.GetFileName(selectedImagePath);
                        string fileSize = new FileInfo(selectedImagePath).Length.ToString("N0") + " bytes";
                        MessageBox.Show("Đã chọn ảnh thành công!\n\n" +
                                      "• Tên file: " + fileName + "\n" +
                                      "• Kích thước: " + fileSize + "\n" +
                                      "• Đường dẫn: " + selectedImagePath,
                                      "Thông tin ảnh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("File ảnh không tồn tại hoặc bị lỗi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        selectedImagePath = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                selectedImagePath = "";
            }
        }

        private void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Bước 1: Kiểm tra có click vào hàng hợp lệ không
                if (e.RowIndex >= 0 && e.RowIndex < dgvHang.Rows.Count)
                {
                    DataGridViewRow row = dgvHang.Rows[e.RowIndex];
                    
                    // Bước 2: Hiển thị thông tin hàng hóa lên form
                    txtMaHang.Text = row.Cells["MaHang"].Value?.ToString() ?? "";
                    txtTenHang.Text = row.Cells["TenHang"].Value?.ToString() ?? "";
                    cbChatLieu.Text = row.Cells["ChatLieu"].Value?.ToString() ?? "";
                    txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString() ?? "";
                    txtDonGiaNhap.Text = row.Cells["DonGiaNhap"].Value?.ToString() ?? "";
                    txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value?.ToString() ?? "";

                    // Bước 3: Xử lý hiển thị ảnh
                    string imageName = row.Cells["Anh"].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        // Tìm ảnh trong thư mục ứng dụng
                        string imagePath = Path.Combine(Application.StartupPath, imageName);
                        if (File.Exists(imagePath))
                        {
                            picAnh.Image = System.Drawing.Image.FromFile(imagePath);
                            selectedImagePath = imagePath;
                        }
                        else
                        {
                            // Nếu không tìm thấy trong thư mục ứng dụng, thử thư mục gốc
                            string rootImagePath = Path.Combine(Application.StartupPath, "..", "..", imageName);
                            if (File.Exists(rootImagePath))
                            {
                                picAnh.Image = System.Drawing.Image.FromFile(rootImagePath);
                                selectedImagePath = rootImagePath;
                            }
                            else
                            {
                                // Hiển thị ảnh mặc định nếu không tìm thấy
                                picAnh.Image = null;
                                selectedImagePath = "";
                                MessageBox.Show("Không tìm thấy file ảnh: " + imageName, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        // Không có ảnh
                        picAnh.Image = null;
                        selectedImagePath = "";
                    }
                    
                    // Bước 4: Hiển thị thông báo đã chọn
                    string tenHang = txtTenHang.Text;
                    if (!string.IsNullOrEmpty(tenHang))
                    {
                        this.Text = "Danh mục hàng hóa - Đã chọn: " + tenHang;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị thông tin hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            try
            {
                // Bước 1: Kiểm tra các trường bắt buộc không được để trống
                if (string.IsNullOrWhiteSpace(txtMaHang.Text))
                {
                    MessageBox.Show("Vui lòng nhập MÃ HÀNG!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaHang.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtTenHang.Text))
                {
                    MessageBox.Show("Vui lòng nhập TÊN HÀNG!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenHang.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cbChatLieu.Text))
                {
                    MessageBox.Show("Vui lòng chọn CHẤT LIỆU!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbChatLieu.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
                {
                    MessageBox.Show("Vui lòng nhập SỐ LƯỢNG!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtDonGiaNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập ĐƠN GIÁ NHẬP!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaNhap.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtDonGiaBan.Text))
                {
                    MessageBox.Show("Vui lòng nhập ĐƠN GIÁ BÁN!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaBan.Focus();
                    return false;
                }

                // Bước 2: Kiểm tra định dạng và giá trị hợp lệ
                
                // Kiểm tra mã hàng (không được chứa ký tự đặc biệt)
                string maHang = txtMaHang.Text.Trim();
                if (maHang.Length < 2 || maHang.Length > 20)
                {
                    MessageBox.Show("Mã hàng phải có từ 2-20 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaHang.Focus();
                    return false;
                }

                // Kiểm tra tên hàng
                string tenHang = txtTenHang.Text.Trim();
                if (tenHang.Length < 2 || tenHang.Length > 100)
                {
                    MessageBox.Show("Tên hàng phải có từ 2-100 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenHang.Focus();
                    return false;
                }

                // Kiểm tra số lượng
                if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương (lớn hơn 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return false;
                }

                if (soLuong > 999999)
                {
                    MessageBox.Show("Số lượng không được vượt quá 999,999!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return false;
                }

                // Kiểm tra đơn giá nhập
                if (!float.TryParse(txtDonGiaNhap.Text.Trim(), out float donGiaNhap) || donGiaNhap <= 0)
                {
                    MessageBox.Show("Đơn giá nhập phải là số dương (lớn hơn 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaNhap.Focus();
                    return false;
                }

                if (donGiaNhap > 999999999)
                {
                    MessageBox.Show("Đơn giá nhập không được vượt quá 999,999,999!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaNhap.Focus();
                    return false;
                }

                // Kiểm tra đơn giá bán
                if (!float.TryParse(txtDonGiaBan.Text.Trim(), out float donGiaBan) || donGiaBan <= 0)
                {
                    MessageBox.Show("Đơn giá bán phải là số dương (lớn hơn 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaBan.Focus();
                    return false;
                }

                if (donGiaBan > 999999999)
                {
                    MessageBox.Show("Đơn giá bán không được vượt quá 999,999,999!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaBan.Focus();
                    return false;
                }

                // Bước 3: Kiểm tra logic nghiệp vụ
                if (donGiaBan <= donGiaNhap)
                {
                    MessageBox.Show("Đơn giá bán (" + donGiaBan.ToString("N0") + ") phải lớn hơn đơn giá nhập (" + donGiaNhap.ToString("N0") + ")!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaBan.Focus();
                    return false;
                }

                return true; // Tất cả validation đều pass
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool CheckDuplicateMaHang(string maHang)
        {
            try
            {
                // Bước 1: Kiểm tra input
                if (string.IsNullOrWhiteSpace(maHang))
                {
                    return false; // Không cần kiểm tra nếu mã hàng rỗng
                }

                // Bước 2: Kết nối database và kiểm tra trùng lặp
                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Bước 3: Tạo câu lệnh SQL để kiểm tra trùng lặp
                    string sql = "SELECT COUNT(*) FROM tblHang WHERE UPPER(TRIM(MaHang)) = UPPER(TRIM(@ma))";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maHang.Trim());

                    // Bước 4: Thực thi truy vấn
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    
                    // Bước 5: Trả về kết quả (true nếu đã tồn tại)
                    return count > 0;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi database khi kiểm tra mã hàng trùng: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Trả về true để an toàn (không cho phép thêm)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra mã hàng trùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Trả về true để an toàn (không cho phép thêm)
            }
        }

        private void ClearInputs()
        {
            try
            {
                // Bước 1: Xóa tất cả textbox
                txtMaHang.Clear();
                txtTenHang.Clear();
                txtSoLuong.Clear();
                txtDonGiaNhap.Clear();
                txtDonGiaBan.Clear();
                txtGhiChu.Clear();
                
                // Bước 2: Reset combobox
                cbChatLieu.SelectedIndex = -1;
                
                // Bước 3: Xóa ảnh
                picAnh.Image = null;
                selectedImagePath = "";
                
                // Bước 4: Reset title form
                this.Text = "Danh mục hàng hóa";
                
                // Bước 5: Focus vào ô đầu tiên
                txtMaHang.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và các phím điều khiển (Backspace, Delete, Tab, Enter)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Số lượng chỉ được nhập số nguyên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtDonGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số, dấu chấm thập phân và các phím điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Đơn giá nhập chỉ được nhập số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.KeyChar == '.')
            {
                // Kiểm tra xem đã có dấu chấm nào chưa
                TextBox textBox = sender as TextBox;
                if (textBox != null && textBox.Text.Contains('.'))
                {
                    e.Handled = true;
                    MessageBox.Show("Chỉ được nhập một dấu chấm thập phân!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtDonGiaBan_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số, dấu chấm thập phân và các phím điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Đơn giá bán chỉ được nhập số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.KeyChar == '.')
            {
                // Kiểm tra xem đã có dấu chấm nào chưa
                TextBox textBox = sender as TextBox;
                if (textBox != null && textBox.Text.Contains('.'))
                {
                    e.Handled = true;
                    MessageBox.Show("Chỉ được nhập một dấu chấm thập phân!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác nhận thoát ứng dụng
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát khỏi hệ thống quản lý hàng hóa?\n\n" +
                    "• Tất cả dữ liệu chưa lưu sẽ bị mất\n" +
                    "• Ứng dụng sẽ đóng hoàn toàn",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Hiển thị thông báo tạm biệt
                    MessageBox.Show("Cảm ơn bạn đã sử dụng hệ thống quản lý hàng hóa!\n\nHẹn gặp lại!", 
                                  "Tạm biệt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Thoát ứng dụng
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thoát ứng dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // Vẫn thoát ngay cả khi có lỗi
            }
        }
    }
}
