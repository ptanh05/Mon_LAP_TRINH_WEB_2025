using System;
using System.Data;
using System.Data.SqlClient;

namespace Classes
{
    class DataBaseProcess
    {
        //Khai báo biến toàn cục, bạn phải thay đổi chuối kết nối phù hợp
        string strConnect = "Data Source=PHUNGTHEANH\\SQLEXPRESS;" +
                "Initial Catalog=BanHang;Integrated Security=true;Connect Timeout=30";
        SqlConnection sqlConnect = null;

        //Phương thức mở kết nối
        void OpenConnect()
        {
            sqlConnect = new SqlConnection(strConnect);
            if (sqlConnect.State != ConnectionState.Open)
                sqlConnect.Open();
        }

        //Phương thức đóng kết nối
        void CloseConnect()
        {
            if (sqlConnect.State != ConnectionState.Closed)
            {
                sqlConnect.Close();
                sqlConnect.Dispose();
            }
        }

        //Phương thức thực thi câu lệnh Select trả về một DataTable
        public DataTable DataReader(string sqlSelect)
        {
            DataTable tblData = new DataTable();
            OpenConnect();
            SqlDataAdapter sqlData = new SqlDataAdapter(sqlSelect, sqlConnect);
            sqlData.Fill(tblData);
            CloseConnect();
            return tblData;
        }

        //Phương thức thực hiện câu lệnh dạng insert,update,delete
        public void DataChange(string sql)
        {
            OpenConnect();
            SqlCommand sqlcomma = new SqlCommand();
            sqlcomma.Connection = sqlConnect;
            sqlcomma.CommandText = sql;
            sqlcomma.ExecuteNonQuery();
            CloseConnect();
        }
    }
}
