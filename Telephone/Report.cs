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
using System.Drawing.Printing;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;


namespace Telephone
{
    public partial class Report : Form
    {
        public string myProperty { get; set; }
        public string myProperty2 { get; set; }
        public string myProperty3 { get; set; }
        public string myProperty65 { get; set; }
        public string mailingList { get; set; }



        public Report()
        {
            InitializeComponent();
            label1.Text = "";
            label2.Text = "";
           
        }

        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");



        private void button1_Click(object sender, EventArgs e) // Recipt
        {
            printPreviewDialog1.Document = printDocument1;

            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }










        }


        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // PRINT BTN
        {
            printDialog1.Document = printDocument1;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) // PRINT FUNC
        {
            string str = "";

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                str += listBox1.Items[i].ToString();               
            }

            Bitmap bpm = Properties.Resources.logo;
            Image newImage = bpm;
            e.Graphics.DrawImage(newImage, 300, 35, 225, 95);

            e.Graphics.DrawString("\n\n\n\n"+str, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, 90, 140);
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

            /*------------------------------------------------------*/

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
            this.Close();
            a.Show();
        }


        private void button3_Click_1(object sender, EventArgs e)                                //nbfinalpoject@gmail.com      //nbfinal1234
        {
            try
            {
                SqlCommand mySqlCommand = con.CreateCommand();
                mySqlCommand.CommandText = "Select * from Mobiles;";
                con.Open();
                SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                StreamReader sr = new StreamReader("BuyerREP.txt");                             // readying Mobile from Buyer text file and search DB the row with full info                                                               // getting from 'buyer'.txt the phone number of the buyer and put it in str
                string str = sr.ReadLine();
                sr.Close();

                string mail = "";

                while (mySqlDataReader.Read())                                                   // search the phon num from the txt file in ALL DB , if find - copy the line
                {
                    if (mySqlDataReader[2].ToString() == str.Trim())
                    {
                        mail += mySqlDataReader[3];
                    }
                }

                mySqlDataReader.Close();
                con.Close();

                string str2 = "";

                for (int i = 0; i < listBox1.Items.Count; i++)
                    str2 += listBox1.Items[i].ToString();



                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("nbfinalpoject@gmail.com", "nbfinal1234");
                MailMessage msg = new MailMessage();
                msg.To.Add(mail.ToString());                            // To: 
                msg.From = new MailAddress("nbfinalpoject@gmail.com");
                msg.Subject = "Reciept: Thank you for shopping with us";
                msg.Body = str2;
                client.Send(msg);
                MessageBox.Show("Mail have been sent successfully !");


            }

            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Phone c = new Phone();
            this.Close();
            c.Show();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Transparent;
            button2.BackColor = System.Drawing.Color.Transparent;
            button3.BackColor = System.Drawing.Color.Transparent;
            //button4.BackColor = System.Drawing.Color.Transparent;
            //button5.BackColor = System.Drawing.Color.Transparent;
            button6.BackColor = System.Drawing.Color.Transparent;
            //button7.BackColor = System.Drawing.Color.Transparent;
            //button8.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;


            if (this.myProperty3 != null)               // CHECK OUT
            {
                try
                {
                    FileStream f = new FileStream("Buyer.txt", FileMode.Open);
                    StreamReader s = new StreamReader(f);
                    string str = s.ReadLine();

                    FileStream t = new FileStream(str.Trim() + ".txt", FileMode.Open);
                    StreamReader sr = new StreamReader(t);

                    string line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line);
                        listBox1.Items.Add("\n\n");
                    }
                    button3.Visible = true;
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.Message);
                    Phone a = new Phone();
                    this.Close();
                    a.Show();
                }
            }

            else if (this.myProperty != null)           // HISTORY
            {
                try
                {
                    StreamReader sr = new StreamReader(this.myProperty + "_History" + ".txt");
                    string run = "";
                    while ((run = sr.ReadLine()) != null)
                    {
                        listBox1.Items.Add(run);
                        listBox1.Items.Add("\n");
                    }
                }

                catch
                {
                    MessageBox.Show("No Purchase History data");
                    Phone a = new Phone();
                    this.Close();
                    a.Show();
                }
            }

            else if (this.myProperty2 != null)          // LOG DATA
            {
                try
                {
                    StreamReader sr1 = new StreamReader(@"Log_Workers\" + myProperty2.ToString().Trim() + ".txt"); // reading from CURRENT month file

                    string run1 = "";
                    while ((run1 = sr1.ReadLine()) != null)
                    {
                        listBox1.Items.Add(run1);
                    }
                }
                catch 
                {
                    MessageBox.Show("No data");
                    Workers a = new Workers();
                    this.Close();
                    a.Show();             
                }
            }

            else if (this.myProperty65 != null)          // MESSAGE
            {
                button3.Visible = false;
                button2.Visible = false;

                try
                {
                    StreamReader sr1 = new StreamReader("Message.txt"); // reading from CURRENT month file

                    string run1 = "";
                    while ((run1 = sr1.ReadLine()) != null)
                    {
                        listBox1.Items.Add(run1);
                    }

                    sr1.Close();
                }

                catch
                {
                    MessageBox.Show("No data");
                    Phone a = new Phone();
                    this.Close();
                    a.Show();
                }

                File.Delete("Message.txt"); // delete message after read
            }

            else if (this.mailingList != null)  // Mailing show
            {
                try
                {
                    StreamReader Mlist = new StreamReader("Mailing_List.txt");
                    string run2 = "";

                    while ((run2 = Mlist.ReadLine()) != null) 
                    {
                        listBox1.Items.Add(run2);
                        listBox1.Items.Add("\n");
                    }

                    Mlist.Close();
                }

                catch
                {
                    MessageBox.Show("No Data");
                    Phone b = new Phone();
                    this.Close();
                    b.Show();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Login a = new Login();
            a.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FileStream f = new FileStream("Mailing_List.txt", FileMode.Open);
            StreamReader run = new StreamReader(f);
            string line;
            timer1.Interval = 10000;
            while ((line = run.ReadLine()) != null) 
            {
                timer1.Start();
                try
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("nbfinalpoject@gmail.com", "nbfinal1234");
                    MailMessage msg = new MailMessage();
                    msg.To.Add(line);                            // To: 
                    msg.From = new MailAddress("nbfinalpoject@gmail.com");
                    msg.Subject = "Check 1 2 3";
                    msg.Body = "BLA BLA";
                    client.Send(msg);
                    timer1.Stop();
                }
                catch
                {
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ControlPanel a = new ControlPanel();
            a.Show();
            this.Hide();
        }
    }  
}

