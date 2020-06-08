using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyCuaHangSach
{
    public partial class FrmCongViec : Form
    {
        DataTable CongViec;
        public FrmCongViec()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetValues();
            int count = 0;
            count = dataGridView1.Rows.Count;
            string chuoi = "";
            int chuoi2 = 0;
            chuoi = Convert.ToString(dataGridView1.Rows[count - 2].Cells[0].Value);
            chuoi2 = Convert.ToInt32((chuoi.Remove(0, 2)));
            if (chuoi2 + 1 < 100)
            {
                cmbMCV.Text = "CV0" + (chuoi2 + 1).ToString();
            }
        }

        private void FrmCongViec_Load(object sender, EventArgs e)
        {
            DAO.OpenConnection();
            LoadDataToGridview();
            fillDataToCombo();
            DAO.CloseConnetion();
        }
        public void LoadDataToGridview()
        {
            DAO.OpenConnection();
            string sql = "SELECT * FROM CongViec";
            CongViec = DAO.GetDataToTable(sql);
            dataGridView1.DataSource = CongViec;
            DAO.CloseConnetion();
        }
        public void fillDataToCombo()
        {
            string sql = "Select * from CongViec";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, DAO.conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            cmbMCV.DataSource = table;
            cmbMCV.ValueMember = "MaCongViec";
            cmbMCV.DisplayMember = "MaCongViec";
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cmbMCV.Text = dataGridView1.CurrentRow.Cells["MaCongViec"].Value.ToString();
            txtTCV.Text = dataGridView1.CurrentRow.Cells["TenCongViec"].Value.ToString();
            
        }
        private void ResetValues()
        {
            cmbMCV.Text = "";
            txtTCV.Text = "";
            

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cmbMCV.Text == "")
            {
                MessageBox.Show("Bạn cần nhập mã công việc!");
                cmbMCV.Focus();
                return;
            }
            if (txtTCV.Text == "")
            {
                MessageBox.Show("Bạn cần nhập tên công việc!");
                txtTCV.Focus();
                return;
            }
            string sql = "SELECT MaCongViec FROM CongViec WHERE MaCongViec = '" + cmbMCV.Text + "'";
            DAO.OpenConnection();
            if (DAO.checkKeyExit(sql))
            {
                MessageBox.Show("Mã tác giả này đã tồn tại, bạn phải nhập mã khác", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMCV.Focus();
                cmbMCV.Text = "";
                return;
            }
            sql = "INSERT INTO TacGia (MaTacGia, TenTacGia) VALUES ('" + cmbMCV.Text + "', N'" + txtTCV.Text + "')";
            DAO.RunSql(sql);
            DAO.CloseConnetion();
            LoadDataToGridview();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DAO.OpenConnection();
            string sql;
            if (CongViec.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbMCV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "UPDATE CongViec SET TenCongViec = N'" + txtTCV.Text.Trim() + "' WHERE MaCongViec = '" + cmbMCV.Text + "'";
            DAO.RunSql(sql);
            DAO.CloseConnetion();
            LoadDataToGridview();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {


            DAO.OpenConnection();
            string sql;
            if (CongViec.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbMCV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE FROM CongViec WHERE MaCongViec = '" + cmbMCV.Text + "'";
                DAO.RunSql(sql);
                DAO.CloseConnetion();
                LoadDataToGridview();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtTCV.Text == "")
            {
                MessageBox.Show("Bạn cần nhập điều kiện tìm kiếm", "Yêu cầu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "select * from CongViec where 1=1";
            if (txtTCV.Text != "")
            {
                sql = sql + "and TenCongViec like N'%" + txtTCV.Text + "%'";
            }
            CongViec = DAO.GetDataToTable(sql);
            if (CongViec.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi nào thỏa mãn", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Có " + CongViec.Rows.Count + " bản ghi thỏa mãn", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dataGridView1.DataSource = CongViec;

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
