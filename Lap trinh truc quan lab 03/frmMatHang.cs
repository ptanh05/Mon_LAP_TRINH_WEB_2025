using System;
using System.Data;
using System.Windows.Forms;

namespace Lap_trinh_truc_quan_lab_03
{
    public partial class frmMatHang : Form
    {
        //Khai báo và khởi tạo biến toàn cục trong class frmMatHang sử dụng class DataBaseProcess
        Classes.DataBaseProcess dtbase = new Classes.DataBaseProcess();

        public frmMatHang()
        {
            InitializeComponent();
        }

        //Phương thức ẩn hiện các control trong groupBox Chi tiết
        private void HienChiTiet(bool hien)
        {
            txtMaSP.Enabled = hien;
            txtTenSP.Enabled = hien;
            dtpNgayHH.Enabled = hien;
            dtpNgaySX.Enabled = hien;
            txtDonVi.Enabled = hien;
            txtDonGia.Enabled = hien;
            txtGhiChu.Enabled = hien;
            //Ẩn hiện 2 nút Lưu và Hủy
            btnLuu.Enabled = hien;
            btnHuy.Enabled = hien;
        }

        //Phương thức xóa trắng chi tiết
        private void XoaTrangChiTiet()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            dtpNgaySX.Value = DateTime.Today;
            dtpNgayHH.Value = DateTime.Today;
            txtDonVi.Text = "";
            txtDonGia.Text = "";
            txtGhiChu.Text = "";
            //Xóa các lỗi validation
            errChiTiet.Clear();
        }

        //Sự kiện load_Form
        private void frmMatHang_Load(object sender, EventArgs e)
        {
            //Load dữ liệu lên DataGridView
            dgvKetQua.DataSource = dtbase.DataReader("Select * from tblMatHang");
            //Ẩn nút Sửa,xóa      
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Ẩn groupBox chi tiết
            HienChiTiet(false);
            //Ẩn nút Lưu và Hủy ban đầu
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        //Chức năng nút Tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //Cập nhật trên nhãn tiêu đề
            lblTieuDe.Text = "TÌM KIẾM SẢN PHẨM";
            //Cấm nút Sửa và Xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem 
            string sql = "SELECT * FROM tblMatHang where MaSP is not null ";
            //Tim theo MaSP khac rong
            if (txtTKMaSP.Text.Trim() != "")
            {
                sql += " and MaSP like N'%" + txtTKMaSP.Text.Trim() + "%'";
            }
            //kiem tra TenSP 
            if (txtTKTenSP.Text.Trim() != "")
            {
                sql += " AND TenSP like N'%" + txtTKTenSP.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dgvKetQua.DataSource = dtbase.DataReader(sql);
        }

        //Sự kiện CellClick cho datagridView
        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Hien thi nut sua
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = false;
            //Bắt lỗi khi người sử dụng kích linh tinh lên datagrid
            try
            {
                txtMaSP.Text = dgvKetQua.CurrentRow.Cells[0].Value.ToString().Trim();
                txtTenSP.Text = dgvKetQua.CurrentRow.Cells[1].Value.ToString();
                dtpNgaySX.Value = (DateTime)dgvKetQua.CurrentRow.Cells[2].Value;
                dtpNgayHH.Value = (DateTime)dgvKetQua.CurrentRow.Cells[3].Value;
                txtDonVi.Text = dgvKetQua.CurrentRow.Cells[4].Value.ToString();
                txtDonGia.Text = dgvKetQua.CurrentRow.Cells[5].Value.ToString();
                txtGhiChu.Text = dgvKetQua.CurrentRow.Cells[6].Value.ToString();
            }
            catch (Exception ex)
            {
            }
        }

        //Chức năng thêm mới mặt hàng
        private void btnThem_Click(object sender, EventArgs e)
        {
            lblTieuDe.Text = "THÊM SẢN PHẨM";
            //Xoa trang GroupBox chi tiết sản phẩm
            XoaTrangChiTiet();
            //Cam nut sua xoa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Hiện GroupBox Chi tiết
            HienChiTiet(true);
        }

        //Chức năng nút Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            //Cập nhật tiêu đề
            lblTieuDe.Text = "CẬP NHẬT SẢN PHẨM";
            //Ẩn hai nút Thêm và Xóa
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }

        //Chức năng nút Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Bật Message Box cảnh báo người sử dụng
            if (MessageBox.Show("Bạn  có  chắc  chắn  xóa  mã  sản  phẩm  " + txtMaSP.Text + " không ? Nếu  có  ấn  nút  Lưu, không  thì  ấn  nút  Hủy", "Xóa  sản  phẩm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lblTieuDe.Text = "XÓA SẢN PHẨM";
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                //Hiện gropbox chi tiết
                HienChiTiet(true);
            }
        }

        //Chức năng nút Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = "";
            //Chúng ta sử dụng control ErrorProvider để hiển thị lỗi
            //Kiểm tra tên sản phầm có bị để trống không
            if (txtTenSP.Text.Trim() == "")
            {
                errChiTiet.SetError(txtTenSP, "Bạn không để trống tên sản phẩm!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //Kiểm  tra  ngày  sản  xuất,  lỗi  nếu  người  sử  dụng  nhập  vào  ngày  sản  xuất  lớn  hơn ngày hiện tại
            if (dtpNgaySX.Value > DateTime.Now)
            {
                errChiTiet.SetError(dtpNgaySX, "Ngày sản xuất không hợp lệ!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //Kiểm tra ngày hết hạn xem có lớn hơn ngày sản xuất không
            if (dtpNgayHH.Value < dtpNgaySX.Value)
            {
                errChiTiet.SetError(dtpNgayHH, "Ngay  hết  hạn  nhỏ  hơn  ngày  sản  xuất!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //Kiểm  tra   đơn vị  xem có  để trống  không  
            if (txtDonVi.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonVi, "Bạn  không  để  trống  đơn  vi!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //Kiểm  tra  đơn  giá  
            if (txtDonGia.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonGia, "Bạn  không  để  trống  đơn  giá!");
                return;
            }
            else
            {
                //Kiểm tra đơn giá có phải là số không
                double donGia;
                if (!double.TryParse(txtDonGia.Text.Trim(), out donGia) || donGia < 0)
                {
                    errChiTiet.SetError(txtDonGia, "Đơn giá phải là số dương hợp lệ!");
                    return;
                }
                errChiTiet.Clear();
            }
            //Nếu  nút Thêm  enable  thì  thực  hiện thêm  mới  
            //Dùng  ký  tự  N'  trước mỗi giá  trị kiểu  text để  insert giá  trị có  dấu tiếng  việt vào  CSDL được  đúng 
            if (btnThem.Enabled == true)
            {  //Kiểm  tra  xem  ô  nhập  MaSP  có  bị  trống  không  if
                if (txtMaSP.Text.Trim() == "")
                {
                    errChiTiet.SetError(txtMaSP, "Bạn  không  để  trống  mã  sản phẩm  trường  này!");
                    return;
                }
                else if (txtMaSP.Text.Trim().Length > 5)
                {
                    errChiTiet.SetError(txtMaSP, "Mã sản phẩm không được quá 5 ký tự!");
                    return;
                }
                else
                {  //Kiểm  tra  xem  mã  sản  phẩm  đã  tồn  tại  chưa  đẻ  tránh việc  insert  mới  bị  lỗi  
                    sql = "Select  *  From  tblMatHang  Where  MaSP  =N'" + txtMaSP.Text.PadRight(5).Substring(0, 5) + "'";
                    DataTable dtSP = dtbase.DataReader(sql);
                    if (dtSP.Rows.Count > 0)
                    {
                        errChiTiet.SetError(txtMaSP, "Mã sản phẩm trùng trong cơ sở dữ liệu");
                        return;
                    }
                    errChiTiet.Clear();
                }
                //Insert vao CSDL
                sql = "INSERT  INTO  tblMatHang(MaSP, TenSP, NgaySX, NgayHH, DonVi, DonGia, GhiChu) VALUES(";
                sql += "N'" + txtMaSP.Text.PadRight(5).Substring(0, 5) + "',N'" + txtTenSP.Text + "','" + dtpNgaySX.Value.Date + "','" + 
                    dtpNgayHH.Value.Date + "',N'" + txtDonVi.Text + "'," + txtDonGia.Text + ",N'" + txtGhiChu.Text + "')";
            }
            //Nếu nút Sửa enable thì thực hiện cập nhật dữ liệu
            if (btnSua.Enabled == true)
            {
                sql = "Update tblMatHang SET ";
                sql += "TenSP = N'" + txtTenSP.Text + "',";
                sql += "NgaySX = '" + dtpNgaySX.Value.Date + "',";
                sql += "NgayHH = '" + dtpNgayHH.Value.Date + "',";
                sql += "DonVi = N'" + txtDonVi.Text + "',";
                sql += "DonGia = " + txtDonGia.Text + ",";
                sql += "GhiChu = N'" + txtGhiChu.Text + "' ";
                sql += "Where MaSP = N'" + txtMaSP.Text.PadRight(5).Substring(0, 5) + "'";
            }
            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (btnXoa.Enabled == true)
            {
                sql = "Delete From tblMatHang Where MaSP =N'" + txtMaSP.Text.PadRight(5).Substring(0, 5) + "'";
            }
            dtbase.DataChange(sql);
            //Cap nhat lai DataGrid
            sql = "Select * from tblMatHang";
            dgvKetQua.DataSource = dtbase.DataReader(sql);
            //Ẩn hiện các nút phù hợp chức năng
            HienChiTiet(false);
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        //Chức năng nút Hủy
        private void btnHuy_Click(object sender, EventArgs e)
        {
            //Thiết lập lại các nút như ban đầu
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            //xoa trang chi tiết
            XoaTrangChiTiet();
            //Cam nhap vào groupBox chi tiết
            HienChiTiet(false);
        }

        //Chức năng nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "TB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
