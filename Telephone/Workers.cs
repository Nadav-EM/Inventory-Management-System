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
using System.Security.Cryptography;

namespace Telephone
{
    public partial class Workers : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");
        string logi = "";

        string choose;

        public Workers()
        {
            InitializeComponent();
        }

        private void Workers_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            string month = DateTime.Today.ToString("MMMM");
            comboBox1.Text = month;
            label15.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;

            Display();
        }

        void Display() 
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Login", con);
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
                dataGridView1.Rows[i].Cells[1].Value = item[1].ToString();
                dataGridView1.Rows[i].Cells[2].Value = item[2].ToString();
                dataGridView1.Rows[i].Cells[3].Value = item[3].ToString();
                dataGridView1.Rows[i].Cells[4].Value = item[4].ToString();
                dataGridView1.Rows[i].Cells[5].Value = item[5].ToString();
            }
        }
        void dataGridRunner(SqlDataAdapter sda)
        {
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            runonDataGrid(dt);
        }

        private void clear()
        {
            choose = "";

            while (checkedListBox1.CheckedIndices.Count > 0)
            {
                checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[0], false);
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void button1_Click(object sender, EventArgs e)  // NEW
        {
            clear();
        }

        private void button2_Click(object sender, EventArgs e) // INSERT
        {
            int c = 0;
            bool flag = true;
            string shtudel = textBox5.Text;


            Sha cryp = new Sha(textBox3.Text);
            string cipher = cryp.GetPassword().ToString(); // cyper get SHA Password
  
           

            if (checkedListBox1.CheckedItems.Count != 0) // for CheckedlistBox
            {
                for (int x = 0; x < checkedListBox1.CheckedItems.Count; x++)
                {
                    choose += checkedListBox1.CheckedItems[x].ToString();
                    c++;
                }
            }

            if (textBox1.Text == "" || textBox3.Text == "" || textBox6.Text == "") 
            {
                flag = false;
                MessageBox.Show("Empty fields");
            }

            else if (!shtudel.Contains("@"))
            {
                MessageBox.Show("Email should contains '@'");
                flag = false;
            }

           
            else if (c != 0 && flag)   
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Login (login_id,username,password,rank,pass_mail,fname) 
                            VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + cipher + "','"
                                           + choose.ToString() + "','" + textBox5.Text + "','" + textBox6.Text + "') ", con);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Saved !");
                    clear();
                }

                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }

            else
            {
                MessageBox.Show("No Rank data checked");
            }



            Display();
            con.Close();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            clear();
            string f = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            checkedListBox1.SetItemCheckState((int.Parse(f.ToString()) - 1), CheckState.Checked);
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

        }

        private void button3_Click(object sender, EventArgs e) // UPDATE
        {
            try
            {
                if (checkedListBox1.CheckedItems.Count != 0) // copy into 'Choose' the value of the checked item
                {
                    for (int x = 0; x < checkedListBox1.CheckedItems.Count; x++)
                    {
                        choose = checkedListBox1.CheckedItems[x].ToString();
                    }
                }

                con.Open();

                Sha pass = new Sha(textBox3.Text);
                string cipher = pass.GetPassword().ToString();

                SqlCommand cmd = new SqlCommand(@"UPDATE Login  
                SET login_id = '" + textBox1.Text + "',username = '" + textBox2.Text + "', password = '" 
                + cipher + "',rank = '" + choose.ToString() + "',pass_mail = '" + textBox5.Text + "',fname = '" + textBox6.Text + "' WHERE (login_id = '" + textBox1.Text + "')", con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Update Successfully !");

            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            clear();
            Display();
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e) // DELETE
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"DELETE FROM Login  
                WHERE (login_id = '" + textBox1.Text + "')", con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Delete Successfully !");

            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            clear();
            Display();
            con.Close();
        }


        private void checkedListBox1_ItemCheck_1(object sender, ItemCheckEventArgs e) // Click only **ONE** item everytime
        {
            if(e.NewValue == CheckState.Checked)
            {
                for (int x = 0; x < checkedListBox1.Items.Count; x++)
                {
                        if(x != e.Index)
                    {
                        checkedListBox1.SetItemChecked(x, false);
                    }
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e) // search
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter
                ("Select * from Login where (login_id like '%" + textBox4.Text + "%') or (username  like '%" + textBox4.Text + "%') or (fname  like '%" + textBox4.Text + "%') ", con);
                dataGridRunner(sda);
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            Phone a = new Phone();
            this.Hide();
            a.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Shop a = new Shop();
            this.Hide();
            a.Show();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from SalaryWorkers", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView2.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                int i = dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = item[0].ToString();
                dataGridView2.Rows[i].Cells[1].Value = item[1].ToString();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            logi = comboBox1.Text.Trim();
            Report abc = new Report();
            abc.myProperty2 = logi;
            abc.Show();
            this.Hide();
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ControlPanel a = new ControlPanel();
            a.Show();
            this.Hide();         
        }
    }
}
