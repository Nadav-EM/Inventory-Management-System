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
    public partial class Login : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");
        DateTime d = DateTime.Now;
        public string myProperty5 { get; set; }
        string message = "";
        string idForSal = "";
        string rankWorker = "";
        public Login()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e) // LOGING BTN
        {
            con.Open();


            Sha pass = new Sha(textBox2.Text);
            string crypto = pass.GetPassword().ToString();



            string query = "Select * from Login Where username = '" + textBox5.Text.Trim() + "' and password = '" + crypto + "' and pass_mail='" + textBox6.Text.Trim() + "'";

            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dtl = new DataTable();
            sda.Fill(dtl);

            if (dtl.Rows.Count == 1)
            {
                SqlCommand mySqlCommand = con.CreateCommand();              // put the name of the boss/worker in LOGIN file
                mySqlCommand.CommandText = "Select * from Login;";
                SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                string str = textBox5.Text, str2 = "";

                while (mySqlDataReader.Read())                              // search the name from the txt file in ALL DB , if find - copy the line
                {
                    if (mySqlDataReader[1].ToString() == str.Trim())
                    {
                        str2 += mySqlDataReader[5] + "\n" + mySqlDataReader[3]; // Name + Rank --> Login.text
                        rankWorker = mySqlDataReader[3].ToString();
                    }
                }

                FileStream prd = new FileStream("Login.txt", FileMode.Create);
                StreamWriter a = new StreamWriter(prd);
                a.WriteLine(str2.ToString());
                a.Close();
                prd.Close();
                con.Close();

                        try
                        {
                            string month = DateTime.Today.ToString("MMMM");
                            FileStream mnth = new FileStream(@"Log_Workers\" + month.ToString() + ".txt", FileMode.Append);
                            StreamWriter ff = new StreamWriter(mnth);

                            SqlCommand mySqlCommand1 = con.CreateCommand();
                            mySqlCommand1.CommandText = "Select * from Login;";
                            con.Open();
                            SqlDataReader mySqlDataReader1 = mySqlCommand1.ExecuteReader();

                            DateTime value = DateTime.Now;
                            string data = "Login:\n" + value.ToString() + "\n\n";

                            string str1 = crypto;

                            while (mySqlDataReader1.Read())
                            {
                                if (mySqlDataReader1[2].ToString() == str1.Trim())
                                {
                                    data += (mySqlDataReader1[5] + "\n" + mySqlDataReader1[4] + "\n");
                                    idForSal = mySqlDataReader1[0].ToString();
                                }
                            }

                            //data += "- - - - - - - - ";
                            ff.WriteLine(data.ToString());
                            ff.Close();
                            mnth.Close();
                            con.Close();
                        }

                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }

                FileStream f = new FileStream("Login.txt", FileMode.Open);
                StreamReader s = new StreamReader(f);
                string status1 = s.ReadLine();
                status1 = s.ReadLine();
                f.Close();
                s.Close();


                FileStream f_sal = new FileStream("Salary.txt", FileMode.Create); // SALARY FILE, RANK + SECONDS NOW
                StreamWriter s_sal = new StreamWriter(f_sal);

                string hour = DateTime.Now.ToString("HH");
                DateTime m = DateTime.Now;


                double total_Sal = 0;
                double sal = 0;

                total_Sal += (double.Parse(hour.ToString()) * 3600);
                total_Sal += (double.Parse(m.Minute.ToString()) * 60);

                s_sal.WriteLine(rankWorker);
                s_sal.WriteLine(idForSal); // worker_id
                s_sal.WriteLine(total_Sal.ToString()); // total time in seconds

                s_sal.Close();
                f_sal.Close();
               
                if (int.Parse(status1.ToString().Trim()) == 1)  // check BOSS RANK (1)
                {
                   // button3.Visible = true;
                    //button4.Visible = true;
                   // button6.Visible = true;
                   // button7.Visible = true;
                    button8.Visible = true;
                    textBox5.Visible = false;
                    textBox2.Visible = false;
                    textBox6.Visible = false;
                    try
                    {
                        if (new FileInfo("Message.txt").Length > 0)          // if message file is empty
                        {
                            button5.Visible = true;
                        }

                        string NAME = "";

                        FileStream name = new FileStream("Login.txt", FileMode.Open);
                        StreamReader w = new StreamReader(name);
                        NAME += w.ReadLine().ToString().Trim();

                        MessageBox.Show("Hello Mr. " + NAME.ToString() +"\n\n" + "You got a Message");
                    }
                    catch
                    {   
                                          
                    }
                }

                else
                {
                    this.Hide();
                    Phone ann = new Phone();
                    ann.Show();
                }             
            }

            else
            {
                MessageBox.Show("Incorrect Username / Password / Email");
            }

            textBox5.Clear();
            textBox2.Clear();
            textBox6.Clear();
            //this.ActiveControl = textBox5;
            //textBox5.Focus();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

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
        }

        private void Login_Load(object sender, EventArgs e)
        {

            DateTime time = DateTime.Now;

            if (int.Parse(time.Day.ToString()) == 1 && int.Parse(time.Hour.ToString()) <= 10) 
            {
                string query = "UPDATE SalaryWorkers SET salary_month = '0'";
                SqlCommand cmd = new SqlCommand(query, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception eff)
                {
                    MessageBox.Show(eff.Message);
                }
                finally
                {
                    con.Close();

                }
            }


            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox3.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            button5.BackColor = System.Drawing.Color.Transparent;

            
            DateTime d = DateTime.Now;

            if (int.Parse(d.Day.ToString()) == 1 && int.Parse(d.Month.ToString()) == 1 && int.Parse(d.Hour.ToString()) == 6)  // delete Log folder every 1/1/0000
            {
                Directory.Delete(@"Log_Workers", true);
            }

           // this.ActiveControl = textBox5;
           // textBox5.Focus();
        }

        private void textBox5_TextChanged(object sender, EventArgs e) // colors t0 labels when i press User/Passwrd/Email
        {
            /*
            if (textBox5.Text != "")
                label1.ForeColor = Color.Turquoise;
            else
                label1.ForeColor = Color.WhiteSmoke;
                */
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Phone ann = new Phone();
            ann.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Shop ann = new Shop();
            ann.Show();
        }

        private void button5_Click(object sender, EventArgs e) // Message BTN
        {
            FileStream msg = new FileStream("Message.txt", FileMode.Open);
            StreamReader f = new StreamReader(msg);

            message = f.ReadToEnd();
            f.Close();
            msg.Close();

            Report a = new Report();
            a.myProperty65 = message;
            a.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Workers a = new Workers();
            this.Hide();
            a.Show();
        }


        private void textBox5_Enter(object sender, EventArgs e)
        {

            if (textBox5.Text == "Username") 
            {
                textBox5.Text = "";
               textBox5.ForeColor = Color.WhiteSmoke;

            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.Text = "Username";
                textBox5.ForeColor = Color.DimGray;
            }
        }


        private void textBox2_Enter_1(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.WhiteSmoke;
                textBox2.UseSystemPasswordChar = true; 
            }
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Password";
                textBox2.ForeColor = Color.DimGray;
                textBox2.UseSystemPasswordChar = false;
            }
        }

        private void textBox6_Enter_1(object sender, EventArgs e)
        {
            if (textBox6.Text == "Email")
            {
                textBox6.Text = "";
                textBox6.ForeColor = Color.WhiteSmoke;
            }
        }

        private void textBox6_Leave_1(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.Text = "Email";
                textBox6.ForeColor = Color.DimGray;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Sales a = new Sales();
            a.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ControlPanel a = new ControlPanel();
            a.Show();
            this.Hide();
        }
    }
}
