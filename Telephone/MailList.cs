using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace Telephone
{
    class MailList
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Phones2;Integrated Security=True");

        public string run()
        {
            SqlCommand mySqlCommand = con.CreateCommand();
            mySqlCommand.CommandText = "Select * from Mobiles;";
            con.Open();
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            string str2 = "";

            while (mySqlDataReader.Read()) 
                str2 += mySqlDataReader[3] + "\n";          

            mySqlDataReader.Close();
            con.Close();

            FileStream mailing = new FileStream("Mailing_List.txt", FileMode.Create);
            StreamWriter dr = new StreamWriter(mailing);
            dr.WriteLine(str2);
            dr.Close();
            mailing.Close();
            return str2;
        }
    }
}
