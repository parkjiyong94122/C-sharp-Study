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
using System.Windows.Forms.DataVisualization.Charting;  

namespace erptest1
{

    public partial class Home : Form
    {
        private string connectionString = "Data Source=192.168.0.16;Database=SmartFactory;User ID=root;Password=1234";

        public class NoticeInfo
        {
            public string 작성ID;
            public DateTime 날짜;
            public string 공지사항;
        }
        public class NoticeInfoDAC
        {
            MySqlConnection _mySqlCon;

            public NoticeInfoDAC(MySqlConnection mySqlCon)
            {
                _mySqlCon = mySqlCon;
            }
            void FillParameters(MySqlCommand cmd, NoticeInfo item)
            {
                MySqlParameter paramID = new MySqlParameter("작성ID", MySqlDbType.VarChar, 20);
                paramID.Value = item.작성ID;

                MySqlParameter paramDate = new MySqlParameter("날짜", MySqlDbType.DateTime);
                paramDate.Value = item.날짜;

                MySqlParameter paramNotice = new MySqlParameter("공지사항", MySqlDbType.VarChar, 200);
                paramNotice.Value = item.공지사항;

                cmd.Parameters.Add(paramID);
                cmd.Parameters.Add(paramDate);
                cmd.Parameters.Add(paramNotice);
            }
            public void Insert(NoticeInfo item)
            {
                string text = "INSERT INTO Notice VALUES(@작성ID, @날짜, @공지사항)";

                MySqlCommand cmd = new MySqlCommand(text, _mySqlCon);
                FillParameters(cmd, item);
                cmd.ExecuteNonQuery();
            }
            public DataSet SelectTimeASC()
            {
                DataSet ds = new DataSet();

                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Notice ORDER BY 날짜 DESC", _mySqlCon);
                adapter.Fill(ds, "Notice");

                return ds;
            }

        }

        public class ProductInfoDAC
        {
            MySqlConnection _MyConnection;

            public ProductInfoDAC(MySqlConnection MyConnection)
            {
                _MyConnection = MyConnection;
            }

            public DataSet SearchDaily(int sel, string month)
            {

                string nowYear = DateTime.Now.ToString("yyyy");
                string NextYear = DateTime.Now.AddMonths(1).ToString("yyyy");

                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT DATE_FORMAT(생산시간,'%Y-%m-%d') m, COUNT(*) FROM Product WHERE 제품명 = 'YELLOW' GROUP BY m HAVING m >= @DT1 AND m < @DT2 ;", _MyConnection);

                if (sel <= 10)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@DT1", nowYear + "-" + month + "-00");
                    adapter.SelectCommand.Parameters.AddWithValue("@DT2", nowYear + "-" + month + "-31");
                }
                else
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@DT1", nowYear + "-" + month.ToString());
                    adapter.SelectCommand.Parameters.AddWithValue("@DT2", NextYear);
                }

                DataSet ds = new DataSet();
                adapter.Fill(ds, "day");

                return ds;
            }

            public DataSet SearchMonthly(int year)
            {

                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT DATE_FORMAT(생산시간,'%Y-%m') m, COUNT(*) FROM Product WHERE 제품명 = 'YELLOW' GROUP BY m HAVING m >= @DT1 AND m <= @DT2 ;", _MyConnection);
                adapter.SelectCommand.Parameters.AddWithValue("@DT1", year);
                adapter.SelectCommand.Parameters.AddWithValue("@DT2", (year + 1).ToString());

                DataSet ds = new DataSet();
                adapter.Fill(ds, "month");

                return ds;
            }

            public DataSet SearchSerialNum(string SearchedNum)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Product WHERE 시리얼번호 = @DT", _MyConnection);
                adapter.SelectCommand.Parameters.AddWithValue("@DT", SearchedNum);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "serial");

                return ds;
            }

            public DataSet SearchProductDate(string Date)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Product WHERE 생산시간 >= @DT1 AND 생산시간 <= @DT2", _MyConnection);
                adapter.SelectCommand.Parameters.AddWithValue("@DT1", Date + " 00:00:00");
                adapter.SelectCommand.Parameters.AddWithValue("@DT2", Date + " 23:59:59");
                DataSet ds = new DataSet();
                adapter.Fill(ds, "productDate");

                return ds;
            }
        }

        public class PlanInfo
        {
            public DateTime 입력날짜;
            public string 라인;
            public int 목표수;
            public string 상황;
            public string 제품명;
            public int ID;
        }
        public class PlanInfoDAC
        {
            MySqlConnection _MyConnection;

            public PlanInfoDAC(MySqlConnection MyConnection)
            {
                _MyConnection = MyConnection;
            }

            void FillParameters(MySqlCommand cmd, PlanInfo item)
            {

                MySqlParameter paramDate = new MySqlParameter("입력날짜", MySqlDbType.DateTime);
                paramDate.Value = item.입력날짜;

                MySqlParameter paramLine = new MySqlParameter("라인", MySqlDbType.VarChar, 15);
                paramLine.Value = item.라인;

                MySqlParameter paramGoal = new MySqlParameter("목표수", MySqlDbType.Int32);
                paramGoal.Value = item.목표수;

                MySqlParameter paramSituation = new MySqlParameter("상황", MySqlDbType.VarChar, 20);
                paramSituation.Value = item.상황;

                MySqlParameter paramProduct = new MySqlParameter("제품명", MySqlDbType.VarChar, 20);
                paramProduct.Value = item.제품명;

                MySqlParameter paramID = new MySqlParameter("ID", MySqlDbType.Int16);
                paramID.Value = item.ID;

                cmd.Parameters.Add(paramDate);
                cmd.Parameters.Add(paramLine);
                cmd.Parameters.Add(paramGoal);
                cmd.Parameters.Add(paramSituation);
                cmd.Parameters.Add(paramProduct);
                cmd.Parameters.Add(paramID);
            }
            public void Insert(PlanInfo item)
            {
                string text = "INSERT INTO Plan VALUES(@입력날짜, @라인, @목표수, @상황, @제품명, @ID, null)";

                MySqlCommand cmd = new MySqlCommand(text, _MyConnection);
                FillParameters(cmd, item);
                cmd.ExecuteNonQuery();
            }
            public void DeleteDB(PlanInfo item)
            {
                string text = "DELETE FROM Plan WHERE ID=@ID";

                MySqlCommand cmd = new MySqlCommand(text, _MyConnection);
                FillParameters(cmd, item);
                cmd.ExecuteNonQuery();
            }

            public DataSet SelectSituation(string str)
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT 입력날짜,제품명,목표수,라인,상황,ID  FROM Plan WHERE 상황 = @DT", _MyConnection);
                adapter.SelectCommand.Parameters.AddWithValue("@DT", str);
                adapter.Fill(ds);

                return ds;
            }

            public DataSet SelectMaxId()
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT MAX(ID) from Plan;", _MyConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }

            public DataSet SelectLine(string LineName)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT 제품명,상황,목표수,생산수 from Plan WHERE 라인 = @DT ;", _MyConnection);
                adapter.SelectCommand.Parameters.AddWithValue("@DT", LineName);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }

            public void UpdateID(PlanInfo item)
            {
                string text = "UPDATE Plan SET 상황 = '가동중' WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(text, _MyConnection);
                FillParameters(cmd, item);
                cmd.ExecuteNonQuery();
            }
        }

        public Home(string UserID)
        {
            InitializeComponent();
            User.Text = UserID;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button_Notice_Click(sender, e);
        }

        private void button_Notice_Click(object sender, EventArgs e)
        {
            Page0_Notice.BringToFront();

            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            NoticeInfoDAC dac = new NoticeInfoDAC(myConnection);

            dac.SelectTimeASC();

            NoticeGrid.DataSource = dac.SelectTimeASC().Tables["Notice"];
            NoticeGrid.Columns[0].MinimumWidth = 80;
            NoticeGrid.Columns[1].MinimumWidth = 180;
            NoticeGrid.Columns[2].MinimumWidth = NoticeGrid.Width - 345;
        }   //공지사항

        private void button2_Click(object sender, EventArgs e)
        {
            Page_SearchTab.SelectTab(1);
            Page_SearchTab.SelectTab(0);
            page1_Search.BringToFront();
        }  //조회

        private void button_plan_Click(object sender, EventArgs e)
        {
            Page_planTab.SelectTab(1);
            Page_planTab.SelectTab(0);
            page2_line.BringToFront();
        }   //계획

        private void metroGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String searchedNum = ProductGrid[2, e.RowIndex].Value.ToString();    //선택된 행의 serialNumr string 값 
            MessageBox.Show("1111");
        }   //cell 더블클릭 이벤트 추가예정

        private void button2_Click_1(object sender, EventArgs e)   //예약(O) 
        {

            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            int ID = 0;

            try
            {
                PlanInfoDAC dac = new PlanInfoDAC(myConnection);

                DataTable dt = new DataTable();
                dt = dac.SelectMaxId().Tables[0];

                if (dt.Rows[0][0].ToString() == "")
                    ID = 1;
                else
                    ID = Convert.ToInt16(dt.Rows[0][0].ToString()) + 1;

                PlanInfo item = new PlanInfo();
                item.입력날짜 = DateTime.Now;
                item.라인 = SelectProductionLine.Text;
                item.목표수 = Convert.ToInt16(Line_num.Text);
                item.상황 = "예약";
                item.제품명 = Line_Product.Text;
                item.ID = ID;
                dac.Insert(item);
                LinePlanGrid.DataSource = dac.SelectSituation("예약").Tables[0];
                LinePlanGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MessageBox.Show("입력이 완료되었습니다.");

                myConnection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("제품명 , 제품 수량 , 생산 라인을 입력해주세요.");
            }
            finally
            {
                if (myConnection != null && myConnection.State == System.Data.ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
            Line_num.Clear();
        }

        private void Delete_Plan_Click_1(object sender, EventArgs e)    //예약 취소 
        {
            MySqlConnection myConnection = null;
            try
            {
                myConnection = new MySqlConnection(connectionString);
                myConnection.Open();

                PlanInfoDAC dac = new PlanInfoDAC(myConnection);

                string ID = LinePlanGrid[5, LinePlanGrid.SelectedRows[0].Index].Value.ToString();

                PlanInfo item = new PlanInfo();
                item.제품명 = Line_Product.Text;

                item.ID = Convert.ToInt32(ID);

                dac.DeleteDB(item);

                LinePlanGrid.DataSource = dac.SelectSituation("예약").Tables[0];
                LinePlanGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                myConnection.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("셀을 선택해주세요.");
            }
            finally
            {
                if (myConnection != null && myConnection.State == System.Data.ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        private void SelectYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            ProductInfoDAC dac = new ProductInfoDAC(myConnection);

            MonthlyProductionChart.Series[0].XValueMember = "m";
            MonthlyProductionChart.Series[0].YValueMembers = "COUNT(*)";
            MonthlyProductionChart.DataSource = dac.SearchMonthly(Convert.ToInt16(SelectYear.Text)).Tables["month"];
            MonthlyProductionChart.DataBind();
            //  차트 타입    MonthlyProductionChart.Series[0].ChartType = SeriesChartType.
            myConnection.Close();
        }   // O

        private void SelectMonth_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            int sel = SelectMonth.SelectedIndex;
            string month = SelectMonth.Text;

            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            ProductInfoDAC dac = new ProductInfoDAC(myConnection);

            dailyProductionChart.Series[0].XValueMember = "m";
            dailyProductionChart.Series[0].YValueMembers = "COUNT(*)";
            dailyProductionChart.DataSource = dac.SearchDaily(sel, month).Tables["day"];
            dailyProductionChart.DataBind();
            myConnection.Close();
        }   // O

        private void ProductDateTime_ValueChanged_1(object sender, EventArgs e)
        {
            string date = ProductDateTime.Value.ToString("yyyy-MM-dd");

            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            ProductInfoDAC dac = new ProductInfoDAC(myConnection);

            myConnection.Close();
            ProductGrid.DataSource = dac.SearchProductDate(date).Tables["productDate"];
            ProductGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }   // O

        private void SerialSearch_Click_1(object sender, EventArgs e)
        {
            String SearchedNum = SerialNum.Text;

            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            ProductInfoDAC dac = new ProductInfoDAC(myConnection);

            myConnection.Close();
            ProductGrid.DataSource = dac.SearchSerialNum(SearchedNum).Tables["serial"];
        }   // O

        private void NoticeSend_Click(object sender, EventArgs e)
        {
            MySqlConnection myConnection = null;
            try
            {
                NoticeInfo item = new NoticeInfo();
                item.공지사항 = NoticeNote.Text;
                item.날짜 = DateTime.Now;
                item.작성ID = User.Text;

                myConnection = new MySqlConnection(connectionString);

                myConnection.Open();

                NoticeInfoDAC dac = new NoticeInfoDAC(myConnection);
                dac.Insert(item);

                NoticeGrid.DataSource = dac.SelectTimeASC().Tables["Notice"];
                NoticeGrid.Columns[0].MinimumWidth = 80;
                NoticeGrid.Columns[1].MinimumWidth = 180;
                NoticeGrid.Columns[2].MinimumWidth = NoticeGrid.Width - 345;

                myConnection.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("공지사항을 입력해주세요.");
            }
            finally
            {
                if (myConnection != null && myConnection.State == System.Data.ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
            NoticeNote.Clear();
        }   // O

        private void NoticeNote_KeyDown(object sender, KeyEventArgs e)  // O
        {
            if (e.KeyCode == Keys.Enter)
                this.NoticeSend_Click(sender, e);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM ProductName", myConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            myConnection.Close();
            ProductNameGrid.DataSource = ds.Tables[0];
            ProductNameGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void RESET_Click(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            PlanInfoDAC dac = new PlanInfoDAC(myConnection);

            DataTable line1 = dac.SelectLine("1번라인").Tables[0];
            DataTable line2 = dac.SelectLine("2번라인").Tables[0];
            DataTable line3 = dac.SelectLine("3번라인").Tables[0];
            DataTable line4 = dac.SelectLine("4번라인").Tables[0];

            Line1_Grid.DataSource = line1;
            Line1_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Line2_Grid.DataSource = line2;
            Line2_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Line3_Grid.DataSource = line3;
            Line3_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Line4_Grid.DataSource = line4;
            Line4_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //예약 -> 주황색, 가동중 -> 초록색, null - > 빨강
            //red_Circle.png orange_Circle  green_Circle
            ShowLineSituation(line1, Line1_LightPicture);
            ShowLineSituation(line2, Line2_LightPicture);
            ShowLineSituation(line3, Line3_LightPicture);
            ShowLineSituation(line4, Line4_LightPicture);

            myConnection.Close();

        }

        private void Page_planTab_SelectedIndexChanged_1(object sender, EventArgs e)    // O
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            if (Page_planTab.SelectedIndex == 0)
            {
                PlanInfoDAC dac = new PlanInfoDAC(myConnection);
                LinePlanGrid.DataSource = dac.SelectSituation("예약").Tables[0];
                LinePlanGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            if (Page_planTab.SelectedIndex == 1)
            {
                RESET_Click(sender, e);
            }
        }

        private void Page_SearchTab_SelectedIndexChanged(object sender, EventArgs e)    // O
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();

            if (Page_SearchTab.SelectedIndex == 0)
            {
                ProductDateTime_ValueChanged_1(sender, e);
            }
            else if (Page_SearchTab.SelectedIndex == 1)
            {
                SelectYear.Text = DateTime.Now.ToString("yyyy");
                SelectYear_SelectedIndexChanged(sender, e);
            }
            else if (Page_SearchTab.SelectedIndex == 2)
            {
                SelectMonth.Text = DateTime.Now.ToString("MM");
                SelectMonth_SelectedIndexChanged_2(sender, e);
            }
            else if (Page_SearchTab.SelectedIndex == 3)
            {
                buttonRefresh_Click(sender, e);
            }
        }

        void ShowLineSituation(DataTable dataTable, PictureBox picture)
        {
            try
            { 
                if (dataTable.Rows[0][1].ToString() == "예약" || dataTable.Rows[0][1].ToString() == "중지")
                    picture.Image = new Bitmap(@"E:\ERP CODE BASE\C-sharp-Study\erptest1\orange_Circle.png");
                else if (dataTable.Rows[0][1].ToString() == "가동중")
                    picture.Image = new Bitmap(@"E:\ERP CODE BASE\C-sharp-Study\erptest1\green_Circle.png");
                else if (dataTable.Rows[0][1].ToString() == "")
                    picture.Image = new Bitmap(@"E:\ERP CODE BASE\C-sharp-Study\erptest1\red_Circle.png");
            }
            catch (Exception)
            {
                picture.Image = new Bitmap(@"E:\ERP CODE BASE\C-sharp-Study\erptest1\red_Circle.png");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }   //종료

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }   //최소화

        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            
            PlanInfoDAC dac = new PlanInfoDAC(myConnection);
            
            DataTable dt = dac.SelectSituation("중지").Tables[0]; // 1개만 서치 
            if (dt.Rows.Count!=0)
            {
                timer1.Enabled = false;
                if (MessageBox.Show("재가동 하시겠습니까 ?" , dt.Rows[0][3].ToString() + dt.Rows[0][4].ToString(), MessageBoxButtons.OK) == DialogResult.OK)
                {
                    PlanInfo item = new PlanInfo();
                    item.ID = Convert.ToInt32( dt.Rows[0][5].ToString());
                    
                    dac.UpdateID(item);
                    myConnection.Close();
                    timer1.Enabled = true;
                }
            }
        }
    }
}
