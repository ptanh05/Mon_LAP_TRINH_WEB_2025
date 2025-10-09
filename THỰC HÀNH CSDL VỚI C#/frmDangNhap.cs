using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace THỰC_HÀNH_CSDL_VỚI_C_
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT COUNT(*) FROM tblUser WHERE userName=@u AND passWord=@p";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", txtTenDN.Text);
                cmd.Parameters.AddWithValue("@p", txtMatKhau.Text);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    frmHang.userName = txtTenDN.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
