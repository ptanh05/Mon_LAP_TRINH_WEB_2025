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
            frmDangNhap frmDN = new frmDangNhap();
            frmDN.ShowDialog();

            if (string.IsNullOrEmpty(userName))
            {
                Application.Exit();
                return;
            }

            lblDangNhap.Text = "Xin chào: " + userName;
            LoadData();
            LoadComboBox();
        }

        void LoadData()
        {
            string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblHang", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvHang.DataSource = dt;
            }
        }

        void LoadComboBox()
        {
            cbChatLieu.Items.AddRange(new string[] { "Vàng", "Bông", "Mây", "Tre", "Tổng hợp" });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (CheckDuplicateMaHang(txtMaHang.Text))
                {
                    MessageBox.Show("Mã hàng đã tồn tại!");
                    return;
                }

                if (float.Parse(txtDonGiaBan.Text) <= float.Parse(txtDonGiaNhap.Text))
                {
                    MessageBox.Show("Đơn giá bán phải lớn hơn đơn giá nhập!");
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "INSERT INTO tblHang VALUES (@ma, @ten, @chatlieu, @soluong, @dongianhap, @dongiaban, @anh)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", txtMaHang.Text);
                    cmd.Parameters.AddWithValue("@ten", txtTenHang.Text);
                    cmd.Parameters.AddWithValue("@chatlieu", cbChatLieu.Text);
                    cmd.Parameters.AddWithValue("@soluong", int.Parse(txtSoLuong.Text));
                    cmd.Parameters.AddWithValue("@dongianhap", float.Parse(txtDonGiaNhap.Text));
                    cmd.Parameters.AddWithValue("@dongiaban", float.Parse(txtDonGiaBan.Text));
                    cmd.Parameters.AddWithValue("@anh", Path.GetFileName(selectedImagePath));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadData();
                ClearInputs();
                MessageBox.Show("Thêm thành công!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hàng cần sửa!");
                return;
            }

            if (ValidateInput())
            {
                if (float.Parse(txtDonGiaBan.Text) <= float.Parse(txtDonGiaNhap.Text))
                {
                    MessageBox.Show("Đơn giá bán phải lớn hơn đơn giá nhập!");
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "UPDATE tblHang SET TenHang=@ten, ChatLieu=@chatlieu, SoLuong=@soluong, DonGiaNhap=@dongianhap, DonGiaBan=@dongiaban, Anh=@anh WHERE MaHang=@ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", txtMaHang.Text);
                    cmd.Parameters.AddWithValue("@ten", txtTenHang.Text);
                    cmd.Parameters.AddWithValue("@chatlieu", cbChatLieu.Text);
                    cmd.Parameters.AddWithValue("@soluong", int.Parse(txtSoLuong.Text));
                    cmd.Parameters.AddWithValue("@dongianhap", float.Parse(txtDonGiaNhap.Text));
                    cmd.Parameters.AddWithValue("@dongiaban", float.Parse(txtDonGiaBan.Text));
                    cmd.Parameters.AddWithValue("@anh", Path.GetFileName(selectedImagePath));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadData();
                MessageBox.Show("Sửa thành công!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hàng cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa hàng này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "DELETE FROM tblHang WHERE MaHang=@ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", txtMaHang.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadData();
                ClearInputs();
                MessageBox.Show("Xóa thành công!");
            }
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                picAnh.Image = System.Drawing.Image.FromFile(selectedImagePath);
            }
        }

        private void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHang.Rows[e.RowIndex];
                txtMaHang.Text = row.Cells["MaHang"].Value.ToString();
                txtTenHang.Text = row.Cells["TenHang"].Value.ToString();
                cbChatLieu.Text = row.Cells["ChatLieu"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                txtDonGiaNhap.Text = row.Cells["DonGiaNhap"].Value.ToString();
                txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value.ToString();

                string imageName = row.Cells["Anh"].Value.ToString();
                if (!string.IsNullOrEmpty(imageName))
                {
                    string imagePath = Path.Combine(Application.StartupPath, imageName);
                    if (File.Exists(imagePath))
                    {
                        picAnh.Image = System.Drawing.Image.FromFile(imagePath);
                        selectedImagePath = imagePath;
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtMaHang.Text) || string.IsNullOrEmpty(txtTenHang.Text) ||
                string.IsNullOrEmpty(cbChatLieu.Text) || string.IsNullOrEmpty(txtSoLuong.Text) ||
                string.IsNullOrEmpty(txtDonGiaNhap.Text) || string.IsNullOrEmpty(txtDonGiaBan.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return false;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!");
                return false;
            }

            if (!float.TryParse(txtDonGiaNhap.Text, out float donGiaNhap) || donGiaNhap <= 0)
            {
                MessageBox.Show("Đơn giá nhập phải là số dương!");
                return false;
            }

            if (!float.TryParse(txtDonGiaBan.Text, out float donGiaBan) || donGiaBan <= 0)
            {
                MessageBox.Show("Đơn giá bán phải là số dương!");
                return false;
            }

            return true;
        }

        private bool CheckDuplicateMaHang(string maHang)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT COUNT(*) FROM tblHang WHERE MaHang=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", maHang);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private void ClearInputs()
        {
            txtMaHang.Clear();
            txtTenHang.Clear();
            cbChatLieu.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGiaNhap.Clear();
            txtDonGiaBan.Clear();
            picAnh.Image = null;
            selectedImagePath = "";
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDonGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtDonGiaBan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
