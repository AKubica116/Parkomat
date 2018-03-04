using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;


namespace ParkomatWyjazd
{
    class Wyjazd
    {
        static void Main(string[] args)
        {
            SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=ParkomatDB");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            int cenaZaGodzine = 1;
            string komenda = "";

            while (komenda != "exit")
            {
                Console.Clear();
                Console.WriteLine($"Proszę zbliżyć bilet lub podać ID.");
                int ID = Convert.ToInt32(Console.ReadLine());
                komenda = ID.ToString();
                Console.WriteLine($"Podałeś ID: {ID}");
                int czasPostoju = ObliczCzasPostoju(ID);
                Console.WriteLine($"Twój czas postoju wyniósł: {czasPostoju} godzin");
                int kwotaDoZaplaty = czasPostoju * cenaZaGodzine;
                Console.WriteLine(
                    $"Kwota do zapłaty: {kwotaDoZaplaty} PLN \nDostępne nominały: 1, 2, 5 PLN \nWprowadź pieniądze:");
                int kwotaWprowadzona = Convert.ToInt32(Console.ReadLine());
                int reszta = kwotaWprowadzona - kwotaDoZaplaty;
                if (reszta == 0)
                {
                    ZapiszWyjazd(ID,kwotaDoZaplaty);
                    Console.WriteLine($"Zapłacone. Dziękujemy!");
                }
                else
                {
                    Console.WriteLine($"Wprowadziłeś {kwotaWprowadzona} PLN. Odbierz resztę: {reszta} PLN");
                    ZapiszWyjazd(ID, kwotaDoZaplaty);
                    Console.ReadKey();
                    Console.WriteLine("Dziękujemy!");
                }
                Thread.Sleep(5000);
            }
            int ObliczCzasPostoju(int IDklienta)
            {
                DateTime czasWjazdu = new DateTime();
                DateTime obecnyCzas = DateTime.Now;
                cmd.CommandText = $"SELECT [CzasWjazdu] FROM Tickets WHERE ID = {IDklienta}";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = myConn;
                PolaczZBaza();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        czasWjazdu = reader.GetDateTime(0);
                    }
                } 
                OdlaczOdBazy();
                int spedzonyCzas = obecnyCzas.Hour - czasWjazdu.Hour + 1;
                return spedzonyCzas;
            }
            void ZapiszWyjazd(int ID, int kwota)
            {
                PolaczZBaza();
                cmd.CommandText = $"UPDATE [dbo].[Tickets] SET CzasWyjazdu = GetDate(), Kwota = {kwota} WHERE ID = {ID};";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = myConn;
                cmd.ExecuteNonQuery();
                OdlaczOdBazy();
            }
            void PolaczZBaza()
            {
                myConn.Open();
            }
            void OdlaczOdBazy()
            {
                myConn.Close();
            }
        }
    }
}
