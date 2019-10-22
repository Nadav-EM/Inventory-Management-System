using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Telephone
{
    public partial class Sales : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");

        int sold = 0, max = -1, min = 10000000, DailySold = 0;
        double income = 0, DayilyIncome = 0;
        string BestSeller = "", WorseSeller = "";
        int[] Daily = new int[1000];

        public Sales()
        {
            InitializeComponent();
        }



        private void button7_Click(object sender, EventArgs e)
        {
            FileStream f_sal = new FileStream("Salary.txt", FileMode.Append); // SALARY FILE
            StreamWriter s_sal = new StreamWriter(f_sal);
            string hour = DateTime.Now.ToString("HH");
            DateTime m = DateTime.Now;
            int total_Sal = 0;
            int sal = 0;
            total_Sal += (int.Parse(hour.ToString()) * 3600);
            total_Sal += (int.Parse(m.Minute.ToString()) * 60);
            s_sal.WriteLine(total_Sal.ToString()); // total time in seconds
            s_sal.Close();
            f_sal.Close();


            FileStream GHJ = new FileStream("Salary.txt", FileMode.Open);
            StreamReader v = new StreamReader(GHJ);

            string Rank, iddd;
            double first;
            double last;

            Rank = v.ReadLine();
            iddd = v.ReadLine();
            first = int.Parse(v.ReadLine());
            last = int.Parse(v.ReadLine());
            v.Close();
            GHJ.Close();

            SalaryCALC s1 = new SalaryCALC(Rank, last, first);


            con.Open();
            SqlCommand mySqlCommand = con.CreateCommand();
            mySqlCommand.CommandText = "Select * from SalaryWorkers;";
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            float lastsal = 0;

            while (mySqlDataReader.Read())
            {
                if (mySqlDataReader[0].ToString() == iddd)
                {
                    lastsal = float.Parse(mySqlDataReader[1].ToString());
                }
            }

            con.Close();

            lastsal += float.Parse(s1.count().ToString());

            con.Open();
            SqlCommand cmd = new SqlCommand(@"UPDATE SalaryWorkers  
            SET salary_month = '" + (lastsal) + "' WHERE (worker_id = '" + iddd.ToString() + "')", con);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Salary Successfully !");
            con.Close();

            try // louout
            {
                string month = DateTime.Today.ToString("MMMM");
                FileStream mnth = new FileStream(@"Log_Workers\" + month.ToString() + ".txt", FileMode.Append);
                StreamWriter ff = new StreamWriter(mnth);



                DateTime value = DateTime.Now;


                string data = "Logout:\n" + value.ToString() + "\n";

                data += "- - - - - - - - ";
                ff.WriteLine(data.ToString());
                ff.Close();
                mnth.Close();
                con.Close();
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            Login a = new Login();
            a.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Phone a = new Phone();
            a.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Shop a = new Shop();
            a.Show();
            this.Hide();
        }

        private void DailySld_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Workers a = new Workers();
            a.Show();
            this.Hide();
        }

        private void Sales_Load(object sender, EventArgs e)
        {
            FileStream fi = new FileStream("DailySales.txt", FileMode.Open);
            StreamReader run = new StreamReader(fi);
            run.ReadLine();                                                                         // step on first line (Day)

            string line;

            while ((line = run.ReadLine()) != null)
            {
                Daily[int.Parse(line.ToString())]++;
                DailySold++;
            }
                
              
            

            run.Close();
            fi.Close();


            for (int i = 0; i < 1000; i++)
            {
                if (Daily[i] > 0) 
                {
                    con.Open();
                    SqlCommand coma1 = con.CreateCommand();
                    coma1.CommandText = "Select * from Products;";

                    SqlDataReader sha1 = coma1.ExecuteReader();


                    string name = "";
                    bool flig = true;

                    while (sha1.Read() && flig)
                    {
                        if (int.Parse(sha1[0].ToString()) == i)
                        {
                            name = sha1[1].ToString();
                            DayilyIncome += double.Parse(sha1[3].ToString());
                            flig = false;
                        }
                    }
                    con.Close();
                    DailySld.Series["Daily Quantity"].Points.AddXY(name, Daily[i]);
                    

                }
            }
            label8.Text = DayilyIncome.ToString();
            label11.Text = DailySold.ToString();
            //-------------------------------------------------------
            /*
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            label15.BackColor = System.Drawing.Color.Transparent;
            this.prodQuant.ChartAreas[0].AxisX.LineColor = Color.DarkTurquoise;
            this.prodQuant.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.DarkTurquoise;
            this.prodQuant.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.DarkTurquoise;
            this.prodQuant.ChartAreas[0].AxisY.LineColor = Color.DarkTurquoise;
            this.prodQuant.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.DarkTurquoise;
            this.prodQuant.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.DarkTurquoise;

            this.DailySld.ChartAreas[0].AxisX.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisY.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.DarkTurquoise;
            */

            Display();

            label2.Text = sold.ToString();
            label4.Text = income.ToString();

            SqlCommand mySqlCommand = con.CreateCommand();
            mySqlCommand.CommandText = "Select * from Stat_sales;";
            con.Open();
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read() && int.Parse(mySqlDataReader[0].ToString()) <= 12)                    // creating grapgh per month   
            {
                string strMonth = new DateTime(2019, int.Parse(mySqlDataReader[0].ToString()), 02).ToString("MMM");
                prodQuant.Series["Quantity"].Points.AddXY(strMonth, mySqlDataReader[1]);
            }

            mySqlDataReader.Close();
            con.Close();
          

            this.DailySld.ChartAreas[0].AxisX.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisY.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.DarkTurquoise;
            this.DailySld.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.DarkTurquoise;



            DailySld.Series[0]["PieLabelStyle"] = "Disabled";
   

        }

        void Display()
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Sales", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            runonDataGrid(dt);

        }

        private void runonDataGrid(DataTable dt)
        {
            foreach (DataRow item in dt.Rows)
            {
                int i = dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = item[0].ToString();
                dataGridView1.Rows[i].Cells[1].Value = item[6].ToString();
                dataGridView1.Rows[i].Cells[2].Value = item[1].ToString();
                dataGridView1.Rows[i].Cells[3].Value = item[2].ToString();
                dataGridView1.Rows[i].Cells[4].Value = item[3].ToString();
                dataGridView1.Rows[i].Cells[5].Value = item[4].ToString();
                dataGridView1.Rows[i].Cells[6].Value = item[5].ToString();

                sold += int.Parse(item[1].ToString());
                income += double.Parse(item[4].ToString());

                if (max < int.Parse(item[1].ToString()))
                {
                    max = int.Parse(item[1].ToString());
                    BestSeller = item[6].ToString();
                }

                if (min > int.Parse(item[1].ToString()))
                {
                    min = int.Parse(item[1].ToString());
                    WorseSeller = item[6].ToString();
                }

                label5.Text = BestSeller;
               // label7.Text = WorseSeller;
            }
        }
    }
}
