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
using System.Net.Mail;
using System.Net;
using System.Net.Mime;


namespace Telephone
{
    public partial class ControlPanel : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");
        string list_mail = "";
        int btnCounter = 0;

        public ControlPanel()
        {
            InitializeComponent();
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Transparent;
            button2.BackColor = System.Drawing.Color.Transparent;
            button3.BackColor = System.Drawing.Color.Transparent;
            button4.BackColor = System.Drawing.Color.Transparent;
            button5.BackColor = System.Drawing.Color.Transparent;
            button6.BackColor = System.Drawing.Color.Transparent;
            button7.BackColor = System.Drawing.Color.Transparent;
            button8.BackColor = System.Drawing.Color.Transparent;
            button9.BackColor = System.Drawing.Color.Transparent;
            //label14.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            FileStream gg = new FileStream("Login.txt", FileMode.Open);
            StreamReader bb = new StreamReader(gg);
            string str = bb.ReadLine().Trim();
            bb.Close();
            gg.Close();

            DateTime t = DateTime.Now;

            if (int.Parse(t.Hour.ToString()) >= 7 && int.Parse(t.Hour.ToString()) < 12) 
                label14.Text = "Good Morning " + str;                     
            else if (int.Parse(t.Hour.ToString()) >= 12 && int.Parse(t.Hour.ToString()) < 16)
                label14.Text = "Good Noon " + str;
            else if (int.Parse(t.Hour.ToString()) >= 16 && int.Parse(t.Hour.ToString()) < 20)
                label14.Text = "Good Evening " + str;
            else 
                label14.Text = "Good Night " + str;
                
        }

        private void button1_Click(object sender, EventArgs e) // Mail List
        {
            MailList M = new MailList();
            list_mail = M.run();
            Report a = new Report();
            a.mailingList = list_mail;
            a.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e) // Send ALL
        {
            btnCounter++;

            if(btnCounter == 1)
            {
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            if (btnCounter == 2)
            {
                FileStream f = new FileStream("Mailing_List.txt", FileMode.Open);
                StreamReader run = new StreamReader(f);
                string line;
                timer1.Interval = 10000;
                MessageBox.Show("Loading...");
                while ((line = run.ReadLine()) != null)
                {
                    timer1.Start();
                    string subject = textBox2.Text;
                    string body = textBox1.Text;

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
                        msg.Subject = subject;
                        msg.Body = body;
                        client.Send(msg);
                        timer1.Stop();
                    }
                    catch
                    {
                    }
                }

                MessageBox.Show("All mails sent Successfully");
                textBox1.Visible = false;
                textBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
            }

        }

        private void button3_Click(object sender, EventArgs e) // WOrkers Salary
        {
            Workers a = new Workers();
            this.Hide();
            a.Show();
        }

        private void button4_Click(object sender, EventArgs e) // Products
        {
            this.Hide();
            Shop a = new Shop();
            a.Show();
        }

        private void button5_Click(object sender, EventArgs e) // Sales
        {
            Sales a = new Sales();
            a.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e) // Products
        {
            this.Hide();
            Phone a = new Phone();
            a.Show();
        }

        private void button7_Click(object sender, EventArgs e) // Report
        {
            this.Hide();
            Report a = new Report();
            a.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login a = new Login();
            a.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MailList M = new MailList();
            list_mail = M.run();
            Report a = new Report();
            a.mailingList = list_mail;
            a.Show();
            this.Hide();
        }
    }
}
