﻿using System;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=192.168.0.16;Database=SmartFactory;User ID=root;Password=1234";
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Role,UserID FROM Login WHERE UserID = '" + textBox1.Text + "' and Password = '" + textBox2.Text+"' ", myConnection);
            DataTable ds = new DataTable();
            adapter.Fill(ds);

            if (ds.Rows.Count == 1)
            {
                this.Hide();
                Home home = new Home(ds.Rows[0][1].ToString());
                home.Show();
            }
            else
            {
                MessageBox.Show("아이디 또는 비밀번호를 확인하세요.");
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.button1_Click(sender, e);
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
