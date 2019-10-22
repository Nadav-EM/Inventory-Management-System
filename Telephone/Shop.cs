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
    public partial class Shop : Form
    {
        int[] monim = new int[1000];
        double[] monimTotalPrice = new double[1000];
        int[] check = new int[13];


        private void Shop_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;




            FileStream ranki = new FileStream("Login.txt", FileMode.Open);
            StreamReader j = new StreamReader(ranki);
            j.ReadLine();

            if (int.Parse(j.ReadLine()) == 1) 
                button13.Visible = true;

            j.Close();
            ranki.Close();
            

            DateTime you = DateTime.Now;

            if (int.Parse(you.Day.ToString()) == 31 && int.Parse(you.Hour.ToString()) > 16)
            {
                FileStream recheck4 = new FileStream("DailySales.txt", FileMode.Create);
                StreamWriter d1 = new StreamWriter(recheck4);
                d1.WriteLine(0);
                d1.Close();
                recheck4.Close();
            }


            int counterrrrArray = 0;
            con.Open();
            SqlCommand coma1234 = con.CreateCommand();
            coma1234.CommandText = "Select * from Products;";
            SqlDataReader sha1234 = coma1234.ExecuteReader();

            while (sha1234.Read())                                                           // Counting products in the store right now
            {
                counterrrrArray++;
            }
            sha1234.Close();
            con.Close();

            con.Open();
            SqlCommand coma123 = con.CreateCommand();
            coma123.CommandText = "Select * from Sales;";

            SqlDataReader sha123 = coma123.ExecuteReader();


            while (sha123.Read())                                                           // updating MONIM array from Sales database in Load
            {

                monim[int.Parse(sha123[0].ToString())] = int.Parse(sha123[1].ToString());
                monimTotalPrice[int.Parse(sha123[0].ToString())] = double.Parse(sha123[4].ToString());

            }
            sha123.Close();
            con.Close();



            con.Open();
            SqlCommand comaFF = con.CreateCommand();
            comaFF.CommandText = "Select * from Stat_sales;";

            SqlDataReader shaFF = comaFF.ExecuteReader();


            while (shaFF.Read())                                                           // updating MONIM array from Statistics database in Load
            {
                check[int.Parse(shaFF[0].ToString())] = int.Parse(shaFF[1].ToString());
            }
            shaFF.Close();
            con.Close();


            label15.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            button12.BackColor = System.Drawing.Color.Transparent;
            button3.BackColor = System.Drawing.Color.Transparent;
            button14.BackColor = System.Drawing.Color.Transparent;
            pictureBox3.BackColor = System.Drawing.Color.Transparent;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;

            Display();
            dataGridView1.Rows[0].Selected = true;

            //  monim = new int[counterrrrArray]; // Malloc to Monim Array
            // monimTotalPrice = new double[counterrrrArray];

            label18.Text = counterrrrArray.ToString();
        }

        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");
        SqlCommand comcom;

        double total = 0;
        string final1;
        string numberrrr = "";
        string imgLoc = "";
        string flag = "";


        public Shop()
        {
            InitializeComponent();

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




        private void button1_Click(object sender, EventArgs e) // APPEND - CART
        {
            FileStream recheck5 = new FileStream("DailySales.txt", FileMode.Append);
            StreamWriter l12 = new StreamWriter(recheck5);
            l12.WriteLine(int.Parse(textBox1.Text.ToString()));
            l12.Close();
            recheck5.Close();



            SqlCommand mySqlCommand = con.CreateCommand();
            mySqlCommand.CommandText = "Select * from Mobiles;";
            con.Open();
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            string str2 = "";

            StreamReader sr = new StreamReader("Buyer.txt");
            string str = final1 = sr.ReadLine();
            sr.Close();

            while (mySqlDataReader.Read())
            {
                if (mySqlDataReader[2].ToString() == str.Trim())
                {
                    str2 += mySqlDataReader[0] + "\n" + mySqlDataReader[1] + "\n" + mySqlDataReader[2] + "\n"
                    + mySqlDataReader[3] + "\n" + mySqlDataReader[4] + "\n" + mySqlDataReader[5] + "\n" + mySqlDataReader[6];
                }
            }

            mySqlDataReader.Close();
            con.Close();

            FileStream z = new FileStream(str.Trim() + "_History" + ".txt", FileMode.Append);
            StreamWriter x = new StreamWriter(z);
            x.WriteLine("\n--------");
            x.Close();
            z.Close();

            FileStream prd = new FileStream("Product.txt", FileMode.Create);
            StreamWriter a = new StreamWriter(prd);
            a.WriteLine(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            a.Close();
            prd.Close();

            StreamReader abc = new StreamReader("product.txt");
            string arr = abc.ReadToEnd();
            abc.Close();


            if (int.Parse(textBox3.Text) <= 10 && int.Parse(textBox3.Text) >= 1)
            {
                flag = "1";

                FileStream u = new FileStream("Message.txt", FileMode.Append);
                StreamWriter d = new StreamWriter(u);

                DateTime value = DateTime.Now;

                d.WriteLine(value.ToString() + "\n");
                d.WriteLine("Product: " + textBox1.Text.Trim() + "\n" + textBox2.Text.Trim() + " - Stock is Runing out !\n\n");

                int left = int.Parse(textBox3.Text) - 1;
                d.WriteLine(left.ToString() + " Products left in store \n");

                d.Close();
                u.Close();

                Login dfg = new Login();
                dfg.myProperty5 = flag;

            }


            if (int.Parse(textBox3.Text) < 1)
            {
                flag = "1";

                FileStream u = new FileStream("Message.txt", FileMode.Append);
                StreamWriter d = new StreamWriter(u);

                DateTime value = DateTime.Now;
                d.WriteLine(value.ToString() + "\n");
                d.WriteLine("Product:  " + textBox1.Text.Trim() + "\n" + textBox2.Text.Trim() + "  -   is out of Stock!\n\n");

                d.Close();
                u.Close();

                MessageBox.Show("Sorry, Product is out of Stock !");

                Login dfg = new Login();
                dfg.myProperty5 = flag;
            }
            else
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE Products  
                    SET inStock = '" + (int.Parse(textBox3.Text) - 1) + "' WHERE(prod_id = '" + textBox1.Text + "')", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Quantity updated!");

                    DateTime now = DateTime.Now;

                    string monthiii = now.Month.ToString();

                    check[int.Parse(monthiii.ToString())]++;


                    con.Open();
                    SqlCommand cmdGG = new SqlCommand(@"UPDATE Stat_sales  
                    SET Quant_sales = '" + check[int.Parse(monthiii.ToString())] + "' WHERE (Month_sales = '" + int.Parse(monthiii.ToString()) + "')", con);

                    cmdGG.ExecuteNonQuery();
                    con.Close();



                    con.Open();
                    SqlCommand coma1 = con.CreateCommand();
                    coma1.CommandText = "Select * from Sales;";

                    SqlDataReader sha1 = coma1.ExecuteReader();

                    string st1 = textBox1.Text;

                    bool flagi = true;

                    while (sha1.Read())
                    {
                        if (sha1[0].ToString() == st1.Trim())
                        {
                            flagi = false;
                        }
                    }

                    con.Close();




                    if (flagi == true)  // new prod in Sales
                    {

                        con.Open();
                        SqlCommand cmd1 = new SqlCommand(@"INSERT INTO Sales (prod_id,sold,inStock,price,totalprice,catagory,prodName) 
                        VALUES ('" + textBox1.Text.ToString() + "','" + 1 + "','"
                                   + (int.Parse(textBox3.Text) - 1) + "','"
                                   + double.Parse(textBox4.Text.ToString()) + "','" + double.Parse(textBox4.Text.ToString())
                                   + "','" + comboBox1.Text + "','" + textBox2.Text + "') ", con);

                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        con.Open();



                        monim[(Convert.ToInt32(textBox1.Text))]++;
                        monimTotalPrice[(Convert.ToInt32(textBox1.Text))] += double.Parse(textBox4.Text.ToString());



                        SqlCommand cmd2 = new SqlCommand(@"UPDATE Sales  
                        SET prod_id = '" + textBox1.Text + "',sold = '" + monim[(Convert.ToInt32(textBox1.Text))] + "', inStock = '"
                        + (int.Parse(textBox3.Text) - 1) + "',price = '" + double.Parse(textBox4.Text.ToString()) + "',totalprice = '" + monimTotalPrice[(Convert.ToInt32(textBox1.Text))] + "',Catagory = '" + comboBox1.Text + "',prodName = '"
                        + textBox2.Text + "' WHERE (prod_id = '" + textBox1.Text + "')", con);
                        cmd2.ExecuteNonQuery();
                        con.Close();


                        // MessageBox.Show("updated");
                    }


                    //label18.Text = monim[(Convert.ToInt32(textBox1.Text))].ToString();


                }

                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                con.Close();




                SqlCommand coma = con.CreateCommand();
                coma.CommandText = "Select * from Products;";
                con.Open();
                SqlDataReader sha = coma.ExecuteReader();

                string st = "";

                while (sha.Read())
                {
                    if (sha[0].ToString() == arr.Trim())
                    {
                        st += "Catalog:  " + sha[0] + "\n" + "Product Name:  " + sha[1] + "\n" + "Price:  "
                        + sha[3] + " $" + "\n" + "Description:  " + sha[4];

                        total += double.Parse(sha[3].ToString());
                    }
                }
                total = Math.Round(total, 2);
                label3.Text = total.ToString();

                FileStream u = new FileStream(str.Trim() + ".txt", FileMode.Append);
                StreamWriter d = new StreamWriter(u);
                d.WriteLine("---------------------------\n" + st);
                d.Close();
                u.Close();

                FileStream t = new FileStream(str.Trim() + "_History" + ".txt", FileMode.Append);
                StreamWriter g = new StreamWriter(t);
                g.WriteLine(st + "\n");
                g.Close();
                t.Close();

                con.Close();
            }
            Display();
        }

    


        private void button2_Click(object sender, EventArgs e) // BTN * BUY *
        {




            DateTime sss = DateTime.Now;
            FileStream recheckkk = new FileStream("DailySales.txt", FileMode.Open);
            StreamReader kkk = new StreamReader(recheckkk);
            string whattt = kkk.ReadLine();
            kkk.Close();
            recheckkk.Close();

            if (int.Parse(sss.Day.ToString()) > int.Parse(whattt.ToString()))  // new file
            {

                DateTime ram = DateTime.Now;
                FileStream recheck2 = new FileStream("DailySales.txt", FileMode.Create);
                StreamWriter l = new StreamWriter(recheck2);

                l.WriteLine(int.Parse(ram.Day.ToString()));                                 // put the Day in Header of file
                l.WriteLine(int.Parse(textBox1.Text.ToString()));
                l.Close();
                recheck2.Close();
            }
            else
            {
                FileStream recheck3 = new FileStream("DailySales.txt", FileMode.Append);
                StreamWriter l1 = new StreamWriter(recheck3);
                l1.WriteLine(int.Parse(textBox1.Text.ToString()));
                l1.Close();
                recheck3.Close();
            }

           //----------------------------------------------------------------------------------

            SqlCommand mySqlCommand = con.CreateCommand();
            mySqlCommand.CommandText = "Select * from Mobiles;";
            con.Open();
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            string str2 = "Date:  " + this.dateTimePicker1.Text + "\n";

            StreamReader sr = new StreamReader("Buyer.txt");       // readying Mobile from Buyer text file and search DB the row with full info                                                               // getting from 'buyer'.txt the phone number of the buyer and put it in str
            string str = final1 = sr.ReadLine();
            sr.Close();

            FileStream credit = new FileStream("credit.txt", FileMode.Open);  // Credit ******* XXXX
            StreamReader dr = new StreamReader(credit);

            string input = textBox7.Text.ToString().Trim();
            string line = "";
            bool floog = false;

            while ((line = dr.ReadLine()) != null && floog == false)
            {
                if (input == line.Trim())
                {
                    floog = true;
                }
            }

            if (floog == true)
            {
                while (mySqlDataReader.Read())                                      // search the phon num from the txt file in ALL DB , if find - copy the line
                {
                    if (mySqlDataReader[2].ToString() == str.Trim())
                    {
                        str2 += "First Name:  " + mySqlDataReader[0] + "\n" + "Last Name:  " + mySqlDataReader[1]
                        + "\n" + "Mobile:  " + mySqlDataReader[2] + "\n" + "Email:  " + mySqlDataReader[3] + "\n" + "Status:  "
                        + mySqlDataReader[4] + "\n" + "Address:  " + mySqlDataReader[5] + "\n" + "City:  " + mySqlDataReader[6]
                        + "\n" + "Payment:  " + comboBox3.Text + "\nCredit Number:  " + "XXXX-XXXX-XXXX-" + textBox7.Text.Substring(12);
                    }
                }
            }

            else
            {
                while (mySqlDataReader.Read())                                      // search the phon num from the txt file in ALL DB , if find - copy the line
                {
                    if (mySqlDataReader[2].ToString() == str.Trim())
                    {
                        str2 += "First Name:  " + mySqlDataReader[0] + "\n" + "Last Name:  " + mySqlDataReader[1]
                        + "\n" + "Mobile:  " + mySqlDataReader[2] + "\n" + "Email:  " + mySqlDataReader[3] + "\n" + "Status:  "
                        + mySqlDataReader[4] + "\n" + "Address:  " + mySqlDataReader[5] + "\n" + "City:  " + mySqlDataReader[6]
                        + "\n" + "Payment:  " + comboBox3.Text;
                    }
                }
            }

            mySqlDataReader.Close();
            con.Close();
        
                
            FileStream f = new FileStream(str.Trim() + ".txt", FileMode.Create);      // FULL data of buyer ---> DATA.txt (0544931135.txt)
            StreamWriter s = new StreamWriter(f);
            s.WriteLine(str2);
            s.Close();
            f.Close();
           
            FileStream z = new FileStream(str.Trim()+ "_History" + ".txt", FileMode.Append);                                    // APPEND TO HISTORY FILE
            StreamWriter x = new StreamWriter(z);
            x.WriteLine(str2 + "\n--------");
            x.Close();
            z.Close();
            
            FileStream prd = new FileStream("Product.txt", FileMode.Create);                                                     // Prod_ID --> Product.txt (IL101)
            StreamWriter a = new StreamWriter(prd);
            a.WriteLine(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            a.Close();
            prd.Close();


            if (int.Parse(textBox3.Text) <= 10 && int.Parse(textBox3.Text) >= 1) 
            {
                flag = "1";

                FileStream u = new FileStream("Message.txt", FileMode.Append);
                StreamWriter d = new StreamWriter(u);

                DateTime value = DateTime.Now;
                d.WriteLine(value.ToString() + "\n");
                d.WriteLine("Product: " + textBox1.Text.Trim() + "\n" + textBox2.Text.Trim() + " - Stock is Runing out !\n\n");

                int left = int.Parse(textBox3.Text) - 1;
                d.WriteLine(left.ToString() + " Products left in store \n");

                d.Close();
                u.Close();


                Login abc = new Login();
                abc.myProperty5 = flag;
            }


            if (int.Parse(textBox3.Text) < 1)
            {
                flag = "1";

                FileStream u = new FileStream("Message.txt", FileMode.Append);
                StreamWriter d = new StreamWriter(u);

                DateTime value = DateTime.Now;
                d.WriteLine(value.ToString() + "\n");
                d.WriteLine("Product: " + textBox1.Text.Trim() + "\n" + textBox2.Text.Trim() + " - is out of Stock!\n\n");
               
                d.Close();
                u.Close();

                MessageBox.Show("Sorry, Product is out of Stock !");
               
                Login abc = new Login();
                abc.myProperty5 = flag;
                //label17.Text = abc.myProperty5.ToString();
            }

            else
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE Products  
                    SET inStock = '" + (int.Parse(textBox3.Text) - 1) + "' WHERE(prod_id = '" + textBox1.Text + "')", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Quantity updated!");

                    DateTime now = DateTime.Now;

                    string monthiii = now.Month.ToString();

                    check[int.Parse(monthiii.ToString())]++;

                    con.Open();
                    SqlCommand cmdGG = new SqlCommand(@"UPDATE Stat_sales  
                    SET Quant_sales = '" + check[int.Parse(monthiii.ToString())] + "' WHERE (Month_sales = '" + int.Parse(monthiii.ToString()) + "')", con);

                    cmdGG.ExecuteNonQuery();
                    con.Close();
                   
               
             

                    /********************************   SALES PART   *********************************************/


                    con.Open();
                    SqlCommand coma1 = con.CreateCommand();
                    coma1.CommandText = "Select * from Sales;";
                
                    SqlDataReader sha1 = coma1.ExecuteReader();

                    string st1 = textBox1.Text;

                    bool flagi = true;

                    while (sha1.Read())
                    {
                        if (sha1[0].ToString() == st1.Trim())
                        {
                            flagi = false;
                        }
                    }

                    con.Close();


                    

                    if (flagi == true)  // new prod in Sales
                    {
                       
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand(@"INSERT INTO Sales (prod_id,sold,inStock,price,totalprice,catagory,prodName) 
                        VALUES ('" + textBox1.Text.ToString() + "','" + 1 + "','" 
                                   + (int.Parse(textBox3.Text) - 1) + "','"
                                   + double.Parse(textBox4.Text.ToString()) + "','" + double.Parse(textBox4.Text.ToString())
                                   + "','" + comboBox1.Text + "','" + textBox2.Text + "') ", con);

                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        con.Open();

                        monim[(Convert.ToInt32(textBox1.Text))]++;  // sold
                        monimTotalPrice[(Convert.ToInt32(textBox1.Text))] += double.Parse(textBox4.Text.ToString()); // += price


                        SqlCommand cmd2 = new SqlCommand(@"UPDATE Sales  
                        SET prod_id = '" + textBox1.Text + "',sold = '" + monim[(Convert.ToInt32(textBox1.Text))] + "', inStock = '" 
                        + (int.Parse(textBox3.Text) -1) + "',price = '" + double.Parse(textBox4.Text.ToString()) + "',totalprice = '" + monimTotalPrice[(Convert.ToInt32(textBox1.Text))] + "',Catagory = '" + comboBox1.Text + "',prodName = '" 
                        + textBox2.Text + "' WHERE (prod_id = '" + textBox1.Text + "')", con);
                        cmd2.ExecuteNonQuery();
                        con.Close();


                        // MessageBox.Show("updated");
                    }


                   // label18.Text = monim[(Convert.ToInt32(textBox1.Text))].ToString();

                    /******************************************************/



                    Display();
                }


                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                con.Close();

                StreamReader abc = new StreamReader("product.txt");
                string arr = abc.ReadToEnd();
                abc.Close();

                SqlCommand coma = con.CreateCommand();
                coma.CommandText = "Select * from Products;";
                con.Open();
                SqlDataReader sha = coma.ExecuteReader();

                string st = "";

                while (sha.Read())
                {
                    if (sha[0].ToString() == arr.Trim())
                    {
                        st += "Catalog:  " + sha[0] + "\n" + "Product Name:  " + sha[1] + "\n" + "Price:  "
                        + sha[3] + " $" + "\n" + "Description:  " + sha[4];

                        total += double.Parse(sha[3].ToString());
                    }
                }
                total = Math.Round(total, 2);

                label3.Text = total.ToString();

                FileStream u = new FileStream(str.Trim() + ".txt", FileMode.Append); // FULL data of buyer ---> DATA.txt (0544931135.txt)
                StreamWriter d = new StreamWriter(u);
                d.WriteLine("---------------------------\n" + st);
                d.Close();
                u.Close();

                //
                FileStream t = new FileStream(str.Trim() + "_History" + ".txt", FileMode.Append); // APPEND TO HISTORY FILE
                StreamWriter g = new StreamWriter(t);
                g.WriteLine(st + "\n");
                g.Close();
                t.Close();
                //

                con.Close();
                label14.Visible = true;
                label13.Visible = true;
                button1.Visible = true;
                button2.Visible = false;
                pictureBox7.Visible = true;
            }

            sr.Close();
            Display();
        }
        

        void Display() // Show Datagrid Window
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Products order by prod_id", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();

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
            comboBox1.SelectedIndex = 0;
            comboBox3.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e) // NEW BTN
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            textBox1.Focus();
            total = 0;
            pictureBox6.Image = null;
        }

        private void button3_Click(object sender, EventArgs e) // INSERT BTN (OLD ONE)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"INSERT INTO Products (prod_id,prodName,inStock,price,descript,catagory) 
                VALUES ('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" 
                           + textBox4.Text.ToString() + "','" + textBox5.Text.ToString() + "','" + comboBox1.Text + "') ", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Successfully Saved !");

                Display();
                con.Close();
            }

            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e) // DELETE BTN
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"DELETE FROM Products  
                WHERE (prod_id = '" + textBox1.Text + "')", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Delete Successfully !");

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                comboBox1.SelectedIndex = -1;
                textBox1.Focus();
                Display();
                con.Close(); 
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e) // show all details in the text Boxes when i press on some line
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

 
            string sql = "SELECT prod_id,prodName, inStock, price, descript, catagory,image FROM Products WHERE prod_id=" + textBox1.Text + "";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            comcom = new SqlCommand(sql, con);
            SqlDataReader reader = comcom.ExecuteReader(); // CRUSH WHEN PROD_ID is "AB12"
            reader.Read();

            if (reader.HasRows)
            {
                textBox1.Text = reader[0].ToString().Trim();
                textBox2.Text = reader[1].ToString().Trim();
                textBox3.Text = reader[2].ToString().Trim();
                textBox4.Text = reader[3].ToString().Trim();
                textBox5.Text = reader[4].ToString().Trim();
                comboBox1.Text = reader[5].ToString().Trim();
                

               
                
                byte[] img = (byte[])(reader[6]);

                if (img == null)
                {
                    pictureBox6.Image = null;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox6.Image = Image.FromStream(ms);
                }
                
            }
            con.Close();
        }


        private void button8_Click(object sender, EventArgs e) // UPDATE BTN
        {

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"UPDATE Products  
                SET prod_id = '" + textBox1.Text + "',prodName = '" + textBox2.Text + "', inStock = '" + textBox3.Text 
                                 + "',price = '" + textBox4.Text + "',descript = '" + textBox5.Text + "',catagory = '" 
                                 + comboBox1.Text + "' WHERE (prod_id = '" + textBox1.Text + "')", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Update Successfully !");

                Display();
                con.Close();
            }

            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e) // SEARCH
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Products where (prod_id like '%" + textBox6.Text + "%') or (prodName like '%" + textBox6.Text + "%') or (catagory like '%" + textBox6.Text + "%') ", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            runonDataGrid(dt);
        }

        
        private void button9_Click(object sender, EventArgs e) // CHECKOUT
        {
            if (label3.Text == "") 
            {
                MessageBox.Show("You havn't bought anything");
            }
            else
            {
                FileStream f = new FileStream("Buyer.txt", FileMode.Open);
                StreamReader s = new StreamReader(f);
                string status1 = s.ReadLine();
                string status2 = s.ReadLine();
                s.Close();
                f.Close();
                con.Close();

                if (status2.ToString() == "FRIENDS") // Calculate discount for VIP Club members
                {
                    total *= (1 - 0.17);
                    total = Math.Round(total, 2);

                    FileStream u = new FileStream(final1.Trim() + ".txt", FileMode.Append); // add final price
                    StreamWriter d = new StreamWriter(u);

                    string send = total.ToString();

                    d.WriteLine("_________________\nTOTAL  [ 17% 'Friends Club' ]  " + string.Format("{0:0.00}", total.ToString()) + " $");
                    d.Close();
                    u.Close();

                    FileStream t = new FileStream(final1.Trim() + "_History" + ".txt", FileMode.Append); // add final price
                    StreamWriter g = new StreamWriter(t);
                    g.WriteLine("\n" + total.ToString() + "\n\n-----------------------" + "\n-----------------------" + "\n-----------------------\n\n");
                    g.Close();
                    t.Close();
                    con.Close();
                }
                else
                {

                    total = Math.Round(total, 2);

                    FileStream u = new FileStream(final1.Trim() + ".txt", FileMode.Append); // add final price
                    StreamWriter d = new StreamWriter(u);
                    d.WriteLine("_________________\nTOTAL:  " + string.Format("{0:F2}", total.ToString()) + " $");
                    d.Close();
                    u.Close();

                   
                    FileStream t = new FileStream(final1.Trim() + "_History" + ".txt", FileMode.Append); // add final price
                    StreamWriter g = new StreamWriter(t);
                    g.WriteLine("\n" + total.ToString() + "\n\n-----------------------" + "\n-----------------------" + "\n-----------------------\n\n");
                    g.Close();
                    t.Close();
                    con.Close();
                }
               
                con.Close();
           
                numberrrr = "";
                Report abc = new Report();
                abc.myProperty3 = numberrrr;
                abc.Show();
                this.Hide();
            }           
        }


        private void button10_Click(object sender, EventArgs e) // sort 
        {
            switch(comboBox2.Text)
            {
                case "Prod_Name":
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from Products order by prodName", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt);
                    break;

                case "Price":
                    SqlDataAdapter sda2 = new SqlDataAdapter("Select * from Products order by price", con);
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt2);
                    break;


                case "Catagory":
                    SqlDataAdapter sda3 = new SqlDataAdapter("Select * from Products order by catagory", con);
                    DataTable dt3 = new DataTable();
                    sda3.Fill(dt3);
                    dataGridView1.Rows.Clear();
                    runonDataGrid(dt3);
                    break;
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Phone b = new Phone();
            this.Hide();
            b.Show();
        }

        private void button12_Click(object sender, EventArgs e)
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

        private void button13_Click(object sender, EventArgs e) // BROWS BTN
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "JPG Files(*.jpg)|*.jpg|GIF Files(*.gif)|*.gif|ALL Files(*.*)|*.*";
                dlg.Title = "Select Product Picture";

                if (dlg.ShowDialog() == DialogResult.OK) 
                {
                    imgLoc = dlg.FileName.ToString();
                    pictureBox6.ImageLocation = imgLoc;
                }
            }  

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void button15_Click(object sender, EventArgs e) // SAVE BTN  // NEW INSTERT
        {
            try
            {
                byte[] img = null;
                FileStream fs = new FileStream(imgLoc, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                img = br.ReadBytes((int)fs.Length);

                string sql = "INSERT INTO Products (prod_id, prodName, inStock, price, descript, catagory,image)VALUES('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','"
                           + textBox4.Text.ToString() + "','" + textBox5.Text.ToString() + "','" + comboBox1.Text + "',@img)";

                if (con.State != ConnectionState.Open) 
                {
                    con.Open();
                }
                comcom = new SqlCommand(sql, con);
                comcom.Parameters.Add(new SqlParameter("@img", img));
                int x = comcom.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(x.ToString() + " Product Saved Successfuly");
                Display();
                
            }

            catch (Exception err)
            {
                con.Close();
                MessageBox.Show(err.Message);
            }
            
        }

        private void button14_Click(object sender, EventArgs e) // SHOW BTN *********************************************************
        {
            try
            {
                string sql = "SELECT prod_id,prodName, inStock, price, descript, catagory,image FROM Products WHERE prod_id = " + textBox1.Text + "";

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                comcom = new SqlCommand(sql, con);
                SqlDataReader reader = comcom.ExecuteReader();
                reader.Read();

                if(reader.HasRows)
                {
                    textBox1.Text = reader[0].ToString();
                    textBox2.Text = reader[1].ToString();
                    textBox3.Text = reader[2].ToString();
                    textBox4.Text = reader[3].ToString();
                    textBox5.Text = reader[4].ToString();
                    comboBox1.Text = reader[5].ToString();

                    byte[] img = (byte[])(reader[6]);

                    if (img == null)
                        pictureBox6.Image = null;
                    
                    else
                    {
                        MemoryStream ms = new MemoryStream(img);
                        pictureBox6.Image = Image.FromStream(ms);
                    }
                }

                else
                    MessageBox.Show("No Pic");

                con.Close();
            }

            catch 
            {
                con.Close();
                MessageBox.Show("File does not exist");
            }

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Credit") 
            {
                textBox7.Visible = true;
                label19.Visible = true;
            }
            else
            {
                textBox7.Visible = false;
                label19.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ControlPanel a = new ControlPanel();
            a.Show();
            this.Hide();
        }
    }
}


