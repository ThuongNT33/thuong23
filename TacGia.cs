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
    public partial class FrmTacGia : Form
    {
        DataTable TacGia;
        public FrmTacGia()
        {
            InitializeComponent();
        }

        public void LoadDataToGridview()
        {
            DAO.OpenConnection();
            string sql = "SELECT * FROM TacGia";
            TacGia = DAO.GetDataToTable(sql);
            dataGridView1.DataSource = TacGia;
            DAO.CloseConnetion();
        }
        public void fillDataToCombo()
        {
            string sql = "Select * from TacGia";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, DAO.conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            cmbMTG.DataSource = table;
            cmbMTG.ValueMember = "MaTacGia";
            cmbMTG.DisplayMember = "MaTacGia";
        }

        private void FrmTacGia_Load(object sender, EventArgs e)
        {
            DAO.OpenConnection();
            LoadDataToGridview();
            fillDataToCombo();
            DAO.CloseConnetion();

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cmbMTG.Text = dataGridView1.CurrentRow.Cells["MaTacGia"].Value.ToString();
            txtTTG.Text = dataGridView1.CurrentRow.Cells["TenTacGia"].Value.ToString();
            txtNS.Text = dataGridView1.CurrentRow.Cells["NgaySinh"].Value.ToString();
            cmbGT.Text = dataGridView1.CurrentRow.Cells["GioiTinh"].Value.ToString();
            txtDC.Text = dataGridView1.CurrentRow.Cells["DiaChi"].Value.ToString();
        }
        private void ResetValues()
        {
            cmbMTG.Text = "";
            txtTTG.Text = "";
            txtNS.Text = "";
            cmbGT.Text = "";
            txtDC.Text = "";

        }
        //Thêm
        private void btnThem_Click(object sender, EventArgs e)
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
                cmbMTG.Text = "TG0" + (chuoi2 + 1).ToString();
            }

        }
        //Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cmbMTG.Text == "")
            {
                MessageBox.Show("Bạn cần nhập mã tác giả!");
                cmbMTG.Focus();
                return;
            }
            if (txtTTG.Text == "")
            {
                MessageBox.Show("Bạn cần nhập tên tác giả!");
                txtTTG.Focus();
                return;
            }
            if (txtNS.Text == "")
            {
                MessageBox.Show("Bạn cần nhập ngày sinh!");
                txtNS.Focus();
                return;
            }
            if (cmbGT.Text == "")
            {
                MessageBox.Show("Bạn cần nhập giới tính!");
                cmbGT.Focus();
                return;
            }
            if (txtDC.Text == "")
            {
                MessageBox.Show("Bạn cần nhập đại chỉ!");
                txtDC.Focus();
                return;
            }
            string sql = "SELECT MaTacGia FROM TacGia WHERE MaTacGia = '" + cmbMTG.Text + "'";
            DAO.OpenConnection();
            if (DAO.checkKeyExit(sql))
            {
                MessageBox.Show("Mã tác giả này đã tồn tại, bạn phải nhập mã khác", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMTG.Focus();
                cmbMTG.Text = "";
                return;
            }
            sql = "INSERT INTO TacGia (MaTacGia, TenTacGia) VALUES ('" + cmbMTG.Text + "', N'" + txtTTG.Text + "')";
            DAO.RunSql(sql);
            DAO.CloseConnetion();
            LoadDataToGridview();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DAO.OpenConnection();
            string sql;
            if (TacGia.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbMTG.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "UPDATE TacGia SET TenTacGia = N'" + txtTTG.Text.Trim() + "' WHERE MaNgonNgu = '" + cmbMTG.Text + "'";
            DAO.RunSql(sql);
            DAO.CloseConnetion();
            LoadDataToGridview();

        }
        //Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            DAO.OpenConnection();
            string sql;
            if (TacGia.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbMTG.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE FROM TacGia WHERE MaTacGia = '" + cmbMTG.Text + "'";
                DAO.RunSql(sql);
                DAO.CloseConnetion();
                LoadDataToGridview();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtTTG.Text == "")
            {
                MessageBox.Show("Bạn cần nhập điều kiện tìm kiếm", "Yêu cầu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "select * from TacGia where 1=1";
            if (txtTTG.Text != "")
            {
                sql = sql + "and TenTacGia like N'%" + txtTTG.Text + "%'";
            }
            TacGia = DAO.GetDataToTable(sql);
            if (TacGia.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi nào thỏa mãn", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Có " + TacGia.Rows.Count + " bản ghi thỏa mãn", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dataGridView1.DataSource = TacGia;

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }   
    }

