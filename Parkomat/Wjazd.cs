using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Parkomat
{
    class Wjazd
    {
        static void Main(string[] args)
        {
            SqlConnection myConn =
                    new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=ParkomatDB");
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

            string komenda = "";
            while (komenda != "exit")
            {
                Console.Clear();
                int iloscWolnychMiejsc = IleWolnychMiejsc();
                if (iloscWolnychMiejsc != 0)
                {
                    int noweID = NadajID(); 
                    DodajNoweID(noweID);
                    Console.WriteLine(
                        $"Witamy! Liczba wolnych miejsc parkingowych wynosi: {iloscWolnychMiejsc} \nCena postoju: 1 PLN/h \nTwoje ID to: {noweID} \nPobierz bilet.");
                    //Thread.Sleep(5000);
                    komenda = Console.ReadLine();
                    Console.WriteLine("Dziękujemy! Proszę wjechać.");
                    Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine("Przepraszamy. Brak wolnych miejsc!");
                    komenda = "exit";
                }

            }
            #region
            void DodajNoweID(int noweID)
            {
                PolaczZBaza();
                cmd.CommandText = $"INSERT INTO Tickets VALUES ({noweID}, GETDATE(), null, null);";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = myConn;


                cmd.ExecuteNonQuery();
                OdlaczOdBazy();
            }
            int NadajID()
            {
                int nowyID;
                int ostatniID = 0;
                PolaczZBaza();
                cmd.CommandText = "SELECT TOP 1 ID FROM Tickets ORDER BY ID DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = myConn;
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ostatniID = reader.GetInt32(0);
                    }
                }

                nowyID = ostatniID + 1;

                OdlaczOdBazy();
                return nowyID;
            }
            int IleWolnychMiejsc()
            {
                cmd.CommandText = "SELECT COUNT (id) as Number FROM Tickets where CzasWyjazdu IS NULL";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = myConn;
                int zajeteMiejsca = 0;
                int wolneMiejsca = 10;
                PolaczZBaza();
                //SqlCommand queryDB = new SqlCommand(str, myConn);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        zajeteMiejsca = reader.GetInt32(0);
                    }
                }
                OdlaczOdBazy();
                wolneMiejsca = wolneMiejsca - zajeteMiejsca;
                    return wolneMiejsca;
                }
            void PolaczZBaza()
            {
                myConn.Open();
            }
            void OdlaczOdBazy()
            {
                myConn.Close();
            }
            #endregion
        }
    }
}
