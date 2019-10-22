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
    public partial class Phone : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");
        string number = "";
        string logi = "";
        string list_mail = "";

        public Phone()
        {
            InitializeComponent();            
        }

        private void Phone_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = 0;

            string month = DateTime.Today.ToString("MMMM");
            comboBox3.Text = month;                                         // Current month in comboBox3 default
            label13.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            label15.BackColor = System.Drawing.Color.Transparent;
            button7.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            label14.BackColor = System.Drawing.Color.Transparent;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;

            FileStream gg = new FileStream("Login.txt", FileMode.Open);
            StreamReader bb = new StreamReader(gg);
            string str = bb.ReadLine().Trim();

            label14.Text = "Hello " + str;                                     // HELLO NADAV MSG

            str = bb.ReadLine().ToString(); // READING RANK (boss-1,worker-2)

            if (int.Parse(str) == 1) // check Rank to Show.Panel8
                panel8.Visible = true;

            bb.Close();
            gg.Close();
            Display();
        }

        private void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            //textBox3.Clear();
            textBox4.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox1.Focus();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
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
                dataGridView1.Rows[i].Cells[6].Value = item[6].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e) // Clear Button
        {
            clear();
        }

        private void button2_Click(object sender, EventArgs e) // Insert Button
        {
            double num;
            string str = textBox3.Text.Trim(), shtrudel = textBox4.Text;
            bool isNum = double.TryParse(str, out num);

            if (shtrudel.Contains("@"))
            {
                if (isNum && str.Length == 10)
                {
                    if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                    {
                        try
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand(@"INSERT INTO Mobiles (First,Last,Mobile,Email,Catagory,Adddress,City) 
                            VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + comboBox1.Text + "','" + textBox6.Text + "','" + textBox7.Text + "' ) ", con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Successfully Saved !");
                            Display();
                            clear();
                        }

                        catch (Exception)
                        {
                            MessageBox.Show("Error !");
                        }
                    }
                    else                  
                        MessageBox.Show("Please Fill Mandatory Fields !");
                }
                else
                    MessageBox.Show("Invalid Phone number !");               
            }
            else
                MessageBox.Show("Email should contains '@'");
        }

        void Display() // Show Datagrid Window
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Mobiles order by Last", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            runonDataGrid(dt);
        }

        void dataGridRunner(SqlDataAdapter sda)
        {
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            runonDataGrid(dt);
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e) // Grid Mouse
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox7.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
        }
               
        private void button3_Click(object sender, EventArgs e) // Delete Button
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"DELETE FROM Mobiles  
                WHERE (Mobile = '" + textBox3.Text + "')", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Delete Successfully !");
                clear();
                Display();
                con.Close();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e) // Update Button
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"UPDATE Mobiles  
                SET First = '" + textBox1.Text + "',Last = '" + textBox2.Text + "', Mobile = '" + textBox3.Text + "',Email = '" + textBox4.Text + "',Catagory = '" + comboBox1.Text + "',Adddress = '" + textBox6.Text + "',City = '" + textBox7.Text + "' WHERE (Mobile = '" + textBox3.Text + "')", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Update Successfully !");
                clear();
                Display();
                con.Close();
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }       
        }

        private void textBox5_TextChanged(object sender, EventArgs e) // Search box
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter
                ("Select * from Mobiles where (Mobile like '%" + textBox5.Text + "%') or (First  like '%" + textBox5.Text + "%') or (Last  like '%" + textBox5.Text + "%') ", con);
                dataGridRunner(sda);
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e) // Sort Button
        {
            switch (comboBox2.Text)
            {
                case "City":
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from Mobiles order by City", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt);
                    break;

                case "Address":
                    SqlDataAdapter sda2 = new SqlDataAdapter("Select * from Mobiles order by Adddress", con);
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt2);
                    break;

                case "Last Name":
                    SqlDataAdapter sda3 = new SqlDataAdapter("Select * from Mobiles order by Last", con);
                    DataTable dt3 = new DataTable();
                    sda3.Fill(dt3);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt3);
                    break;

                case "First Name":
                    SqlDataAdapter sda4 = new SqlDataAdapter("Select * from Mobiles order by First", con);
                    DataTable dt4 = new DataTable();
                    sda4.Fill(dt4);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt4);
                    break;

                case "Catagory":
                    SqlDataAdapter sda5 = new SqlDataAdapter("Select * from Mobiles order by Catagory", con);
                    DataTable dt5 = new DataTable();
                    sda5.Fill(dt5);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt5);
                    break;
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // NEXT BTN
        private void button6_Click(object sender, EventArgs e) // copy Mobile number to TXT, Open Form2, close Form1
        {
            FileStream f = new FileStream("Buyer.txt", FileMode.Create);
            StreamWriter s = new StreamWriter(f);
            s.WriteLine(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            s.WriteLine(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
            s.Close();
            f.Close();

            FileStream ii = new FileStream("BuyerREP.txt", FileMode.Create); // only file for Report/Email FORM
            StreamWriter jj = new StreamWriter(ii);
            jj.WriteLine(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            jj.Close();
            ii.Close();

            con.Close();

            Shop b = new Shop();
            this.Hide();
            b.Show();        
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
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

            this.Close();
            Login a = new Login();
            a.Show();
        }

        private void button8_Click(object sender, EventArgs e) // History BTN
        {
            number = textBox3.Text.Trim();
            Report abc = new Report();
            abc.myProperty = number;
            abc.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e) // WORKER LOGING DATA 
        {
            /*
            logi = comboBox3.Text.Trim();
            Report abc = new Report();
            abc.myProperty2 = logi;
            abc.Show();
            this.Hide();
            */
        }

        private void button10_Click(object sender, EventArgs e) // Mailing List BTN
        {
            MailList M = new MailList();
            list_mail = M.run();










            Report a = new Report();
            a.mailingList = list_mail;
            a.Show();
            this.Hide();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }   
}
