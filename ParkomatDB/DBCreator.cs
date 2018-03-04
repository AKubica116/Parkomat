using System;
using System.Data;
using System.Data.SqlClient;

namespace ParkomatDB
{
    class DBCreator
    {
        static void Main(string[] args)
        {
            String str, str2;
            SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=master");
            SqlConnection myConn2 = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=ParkomatDB");
            str = "CREATE DATABASE ParkomatDB";
            str2 = @"CREATE TABLE Tickets (
            ID int,
            CzasWjazdu datetime,
            CzasWyjazdu datetime,
            Kwota int
            ); ";
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                myConn.Close();           
                myConn2.Open();
                SqlCommand myCommand2 = new SqlCommand(str2, myConn2);
                myCommand2.ExecuteNonQuery();
                Console.WriteLine("DataBase is Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString(), "ParkomatDB");
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
                if (myConn2.State == ConnectionState.Open)
                {
                    myConn2.Close();
                }
            }
        }
    }
}
