namespace Lap_trinh_truc_quan_lab_03
{
    partial class frmMatHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.grbTimKiem = new System.Windows.Forms.GroupBox();
            this.txtTKTenSP = new System.Windows.Forms.TextBox();
            this.txtTKMaSP = new System.Windows.Forms.TextBox();
            this.lblTKTenSP = new System.Windows.Forms.Label();
            this.lblTKMaSP = new System.Windows.Forms.Label();
            this.grbKetQua = new System.Windows.Forms.GroupBox();
            this.dgvKetQua = new System.Windows.Forms.DataGridView();
            this.grbChiTiet = new System.Windows.Forms.GroupBox();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.txtDonVi = new System.Windows.Forms.TextBox();
            this.dtpNgayHH = new System.Windows.Forms.DateTimePicker();
            this.dtpNgaySX = new System.Windows.Forms.DateTimePicker();
            this.txtTenSP = new System.Windows.Forms.TextBox();
            this.txtMaSP = new System.Windows.Forms.TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblDonVi = new System.Windows.Forms.Label();
            this.lblNgayHH = new System.Windows.Forms.Label();
            this.lblNgaySX = new System.Windows.Forms.Label();
            this.lblTenSP = new System.Windows.Forms.Label();
            this.lblMaSP = new System.Windows.Forms.Label();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.errChiTiet = new System.Windows.Forms.ErrorProvider(this.components);
            this.grbTimKiem.SuspendLayout();
            this.grbKetQua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKetQua)).BeginInit();
            this.grbChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errChiTiet)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.Blue;
            this.lblTieuDe.Location = new System.Drawing.Point(300, 9);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(200, 24);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "Quản lý sản phẩm";
            // 
            // grbTimKiem
            // 
            this.grbTimKiem.Controls.Add(this.txtTKTenSP);
            this.grbTimKiem.Controls.Add(this.txtTKMaSP);
            this.grbTimKiem.Controls.Add(this.lblTKTenSP);
            this.grbTimKiem.Controls.Add(this.lblTKMaSP);
            this.grbTimKiem.Location = new System.Drawing.Point(16, 50);
            this.grbTimKiem.Name = "grbTimKiem";
            this.grbTimKiem.Size = new System.Drawing.Size(350, 80);
            this.grbTimKiem.TabIndex = 1;
            this.grbTimKiem.TabStop = false;
            this.grbTimKiem.Text = "Tìm Kiếm";
            // 
            // txtTKTenSP
            // 
            this.txtTKTenSP.Location = new System.Drawing.Point(200, 50);
            this.txtTKTenSP.Name = "txtTKTenSP";
            this.txtTKTenSP.Size = new System.Drawing.Size(120, 20);
            this.txtTKTenSP.TabIndex = 4;
            // 
            // txtTKMaSP
            // 
            this.txtTKMaSP.Location = new System.Drawing.Point(80, 20);
            this.txtTKMaSP.Name = "txtTKMaSP";
            this.txtTKMaSP.Size = new System.Drawing.Size(120, 20);
            this.txtTKMaSP.TabIndex = 3;
            // 
            // lblTKTenSP
            // 
            this.lblTKTenSP.AutoSize = true;
            this.lblTKTenSP.Location = new System.Drawing.Point(20, 53);
            this.lblTKTenSP.Name = "lblTKTenSP";
            this.lblTKTenSP.Size = new System.Drawing.Size(40, 13);
            this.lblTKTenSP.TabIndex = 2;
            this.lblTKTenSP.Text = "Tên SP:";
            // 
            // lblTKMaSP
            // 
            this.lblTKMaSP.AutoSize = true;
            this.lblTKMaSP.Location = new System.Drawing.Point(20, 23);
            this.lblTKMaSP.Name = "lblTKMaSP";
            this.lblTKMaSP.Size = new System.Drawing.Size(36, 13);
            this.lblTKMaSP.TabIndex = 1;
            this.lblTKMaSP.Text = "Ma SP:";
            // 
            // grbKetQua
            // 
            this.grbKetQua.Controls.Add(this.dgvKetQua);
            this.grbKetQua.Location = new System.Drawing.Point(16, 150);
            this.grbKetQua.Name = "grbKetQua";
            this.grbKetQua.Size = new System.Drawing.Size(350, 200);
            this.grbKetQua.TabIndex = 2;
            this.grbKetQua.TabStop = false;
            this.grbKetQua.Text = "Kết Quả";
            // 
            // dgvKetQua
            // 
            this.dgvKetQua.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKetQua.Location = new System.Drawing.Point(10, 20);
            this.dgvKetQua.Name = "dgvKetQua";
            this.dgvKetQua.Size = new System.Drawing.Size(330, 170);
            this.dgvKetQua.TabIndex = 0;
            this.dgvKetQua.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKetQua_CellClick);
            // 
            // grbChiTiet
            // 
            this.grbChiTiet.Controls.Add(this.btnHuy);
            this.grbChiTiet.Controls.Add(this.btnLuu);
            this.grbChiTiet.Controls.Add(this.txtGhiChu);
            this.grbChiTiet.Controls.Add(this.txtDonGia);
            this.grbChiTiet.Controls.Add(this.txtDonVi);
            this.grbChiTiet.Controls.Add(this.dtpNgayHH);
            this.grbChiTiet.Controls.Add(this.dtpNgaySX);
            this.grbChiTiet.Controls.Add(this.txtTenSP);
            this.grbChiTiet.Controls.Add(this.txtMaSP);
            this.grbChiTiet.Controls.Add(this.lblGhiChu);
            this.grbChiTiet.Controls.Add(this.lblDonGia);
            this.grbChiTiet.Controls.Add(this.lblDonVi);
            this.grbChiTiet.Controls.Add(this.lblNgayHH);
            this.grbChiTiet.Controls.Add(this.lblNgaySX);
            this.grbChiTiet.Controls.Add(this.lblTenSP);
            this.grbChiTiet.Controls.Add(this.lblMaSP);
            this.grbChiTiet.Location = new System.Drawing.Point(400, 50);
            this.grbChiTiet.Name = "grbChiTiet";
            this.grbChiTiet.Size = new System.Drawing.Size(350, 300);
            this.grbChiTiet.TabIndex = 3;
            this.grbChiTiet.TabStop = false;
            this.grbChiTiet.Text = "Chi tiết";
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(200, 250);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 30);
            this.btnHuy.TabIndex = 15;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(100, 250);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 30);
            this.btnLuu.TabIndex = 14;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(100, 200);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(200, 30);
            this.txtGhiChu.TabIndex = 13;
            // 
            // txtDonGia
            // 
            this.txtDonGia.Location = new System.Drawing.Point(100, 170);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(200, 20);
            this.txtDonGia.TabIndex = 12;
            // 
            // txtDonVi
            // 
            this.txtDonVi.Location = new System.Drawing.Point(100, 140);
            this.txtDonVi.Name = "txtDonVi";
            this.txtDonVi.Size = new System.Drawing.Size(200, 20);
            this.txtDonVi.TabIndex = 11;
            // 
            // dtpNgayHH
            // 
            this.dtpNgayHH.Location = new System.Drawing.Point(100, 110);
            this.dtpNgayHH.Name = "dtpNgayHH";
            this.dtpNgayHH.Size = new System.Drawing.Size(200, 20);
            this.dtpNgayHH.TabIndex = 10;
            // 
            // dtpNgaySX
            // 
            this.dtpNgaySX.Location = new System.Drawing.Point(100, 80);
            this.dtpNgaySX.Name = "dtpNgaySX";
            this.dtpNgaySX.Size = new System.Drawing.Size(200, 20);
            this.dtpNgaySX.TabIndex = 9;
            // 
            // txtTenSP
            // 
            this.txtTenSP.Location = new System.Drawing.Point(100, 50);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.Size = new System.Drawing.Size(200, 20);
            this.txtTenSP.TabIndex = 8;
            // 
            // txtMaSP
            // 
            this.txtMaSP.Location = new System.Drawing.Point(100, 20);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.Size = new System.Drawing.Size(200, 20);
            this.txtMaSP.TabIndex = 7;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Location = new System.Drawing.Point(20, 203);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(44, 13);
            this.lblGhiChu.TabIndex = 6;
            this.lblGhiChu.Text = "Ghi chú";
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Location = new System.Drawing.Point(20, 173);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(44, 13);
            this.lblDonGia.TabIndex = 5;
            this.lblDonGia.Text = "Đơn giá";
            // 
            // lblDonVi
            // 
            this.lblDonVi.AutoSize = true;
            this.lblDonVi.Location = new System.Drawing.Point(20, 143);
            this.lblDonVi.Name = "lblDonVi";
            this.lblDonVi.Size = new System.Drawing.Size(38, 13);
            this.lblDonVi.TabIndex = 4;
            this.lblDonVi.Text = "Đơn vị";
            // 
            // lblNgayHH
            // 
            this.lblNgayHH.AutoSize = true;
            this.lblNgayHH.Location = new System.Drawing.Point(20, 113);
            this.lblNgayHH.Name = "lblNgayHH";
            this.lblNgayHH.Size = new System.Drawing.Size(50, 13);
            this.lblNgayHH.TabIndex = 3;
            this.lblNgayHH.Text = "Ngày HH";
            // 
            // lblNgaySX
            // 
            this.lblNgaySX.AutoSize = true;
            this.lblNgaySX.Location = new System.Drawing.Point(20, 83);
            this.lblNgaySX.Name = "lblNgaySX";
            this.lblNgaySX.Size = new System.Drawing.Size(50, 13);
            this.lblNgaySX.TabIndex = 2;
            this.lblNgaySX.Text = "Ngày SX";
            // 
            // lblTenSP
            // 
            this.lblTenSP.AutoSize = true;
            this.lblTenSP.Location = new System.Drawing.Point(20, 53);
            this.lblTenSP.Name = "lblTenSP";
            this.lblTenSP.Size = new System.Drawing.Size(40, 13);
            this.lblTenSP.TabIndex = 1;
            this.lblTenSP.Text = "Tên SP";
            // 
            // lblMaSP
            // 
            this.lblMaSP.AutoSize = true;
            this.lblMaSP.Location = new System.Drawing.Point(20, 23);
            this.lblMaSP.Name = "lblMaSP";
            this.lblMaSP.Size = new System.Drawing.Size(36, 13);
            this.lblMaSP.TabIndex = 0;
            this.lblMaSP.Text = "Mã sp";
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(650, 370);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(75, 30);
            this.btnThoat.TabIndex = 5;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(500, 370);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 30);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(400, 370);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(75, 30);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(300, 370);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 30);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(200, 370);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(75, 30);
            this.btnTimKiem.TabIndex = 0;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // errChiTiet
            // 
            this.errChiTiet.ContainerControl = this;
            // 
            // frmMatHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 420);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.grbChiTiet);
            this.Controls.Add(this.grbKetQua);
            this.Controls.Add(this.grbTimKiem);
            this.Controls.Add(this.lblTieuDe);
            this.Name = "frmMatHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý sản phẩm";
            this.Load += new System.EventHandler(this.frmMatHang_Load);
            this.grbTimKiem.ResumeLayout(false);
            this.grbTimKiem.PerformLayout();
            this.grbKetQua.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKetQua)).EndInit();
            this.grbChiTiet.ResumeLayout(false);
            this.grbChiTiet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errChiTiet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.GroupBox grbTimKiem;
        private System.Windows.Forms.TextBox txtTKTenSP;
        private System.Windows.Forms.TextBox txtTKMaSP;
        private System.Windows.Forms.Label lblTKTenSP;
        private System.Windows.Forms.Label lblTKMaSP;
        private System.Windows.Forms.GroupBox grbKetQua;
        private System.Windows.Forms.DataGridView dgvKetQua;
        private System.Windows.Forms.GroupBox grbChiTiet;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.TextBox txtDonVi;
        private System.Windows.Forms.DateTimePicker dtpNgayHH;
        private System.Windows.Forms.DateTimePicker dtpNgaySX;
        private System.Windows.Forms.TextBox txtTenSP;
        private System.Windows.Forms.TextBox txtMaSP;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.Label lblDonVi;
        private System.Windows.Forms.Label lblNgayHH;
        private System.Windows.Forms.Label lblNgaySX;
        private System.Windows.Forms.Label lblTenSP;
        private System.Windows.Forms.Label lblMaSP;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.ErrorProvider errChiTiet;
    }
}
