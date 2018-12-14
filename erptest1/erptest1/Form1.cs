using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;



namespace erptest1
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();

            DataSet ds = GetData();
            page1_Search.BringToFront();

            metroGrid1.DataSource = ds.Tables[0];
            metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private DataSet GetData()
        {
            string connectionString = "Data Source=192.168.233.129;Database=test1;User ID=root;Password=1234";
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM product", myConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            page2_Input.SendToBack();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            page2_Input.BringToFront();
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            string connectionString = "Data Source=192.168.233.129;Database=test1;User ID=root;Password=1234";
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM product WHERE createTime >= @DT1 AND createTime <= @DT2", myConnection);
            adapter.SelectCommand.Parameters.AddWithValue("@DT1", metroDateTime1.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            adapter.SelectCommand.Parameters.AddWithValue("@DT2", metroDateTime1.Value.ToString("yyyy-MM-dd") + " 23:59:59");
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            myConnection.Close();
            metroGrid1.DataSource = ds.Tables[0];
        }

        private void metroGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String searchedNum = metroGrid1[2, e.RowIndex].Value.ToString();    //선택된 행의 serialNumr string 값 

            //Form2 image = new Form2(searchedNum);
            //image.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String SerachedNum = textBox1.Text;

            string connectionString = "Data Source=192.168.233.129;Database=test1;User ID=root;Password=1234";
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM product WHERE SerialNum = @DT", myConnection);
            adapter.SelectCommand.Parameters.AddWithValue("@DT", SerachedNum);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            myConnection.Close();
            metroGrid1.DataSource = ds.Tables[0];
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.button5_Click(sender,e);
            }
        }
    }
}
