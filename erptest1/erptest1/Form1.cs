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
        private string connectionString = "Data Source=192.168.0.8;Database=SmartFactory;User ID=root;Password=1234";
        public Home()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //DataSet ds = GetData();
            
            //MySqlConnection myConnection = new MySqlConnection(connectionString);
            //myConnection.Open();
            //
            //MySqlCommand cmd = new MySqlCommand();
            //cmd.Connection = myConnection;
            //
            //string name = "허연색";
            //string quality = "양호";
            //int storage = 1;
            ////for (int j = 1; j < 12; j++)
            ////{
            ////
            ////
            ////for (int i = 1; i < 10; i++)
            ////{
            //string serialnum = "1111234";
            //    String text = string.Format("INSERT INTO exex(name) VALUES ('{0}')",name);
            //    cmd.CommandText = text;
            //    cmd.ExecuteNonQuery();
            //}
            //}
            //page1_Search.BringToFront();
            //
            //metroGrid1.DataSource = ds.Tables[0];
            //metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //string connectionString = "Data Source=192.168.233.129;Database=test1;User ID=root;Password=1234";
            //MySqlConnection myConnection = new MySqlConnection(connectionString);
            //myConnection.Open();
            //
            //MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT DATE_FORMAT(생산시간,'%Y-%m') m, COUNT(*) FROM Product GROUP BY m HAVING m >= '2018' AND m <= '2019' ;", myConnection);
            ////adapter.SelectCommand.Parameters.AddWithValue("@DT1", comboBox1.Text);
            ////adapter.SelectCommand.Parameters.AddWithValue("@DT2", (Convert.ToInt16(comboBox1.Text) + 1).ToString());
            //DataSet ds = new DataSet();
            //adapter.Fill(ds);
            //
            //chart1.Series[0].XValueMember = "m";
            //chart1.Series[0].YValueMembers = "COUNT(*)";
            //chart1.DataSource = ds.Tables[0];
            //chart1.DataBind();
            //myConnection.Close();
        }

        private DataSet GetData()
        {
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
            page1_Search.Visible = true;
            page2_Input.Visible = false;
            page_graph.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            page1_Search.Visible = false;
            page2_Input.Visible = true;
            page_graph.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            page_graph.Visible = true;
            page1_Search.Visible = false;
            page2_Input.Visible = false;
           
        }
        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM product WHERE createTime >= @DT1 AND createTime <= @DT2", myConnection);
            adapter.SelectCommand.Parameters.AddWithValue("@DT1", metroDateTime1.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            adapter.SelectCommand.Parameters.AddWithValue("@DT2", metroDateTime1.Value.ToString("yyyy-MM-dd") + " 23:59:59");
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            myConnection.Close();
            metroGrid1.DataSource = ds.Tables[0];
            metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void HeadPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT DATE_FORMAT(생산시간,'%Y-%m') m, COUNT(*) FROM Product GROUP BY m HAVING DATE(m) = @DT1 AND m <= @DT2 ;", myConnection);
            adapter.SelectCommand.Parameters.AddWithValue("@DT1", comboBox1.Text);
            adapter.SelectCommand.Parameters.AddWithValue("@DT2", (Convert.ToInt16(comboBox1.Text) + 1).ToString());
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            chart1.Series[0].XValueMember = "m";
            chart1.Series[0].YValueMembers = "COUNT(*)";
            chart1.DataSource = ds.Tables[0];
            chart1.DataBind();
            myConnection.Close();
        }
    }
}
