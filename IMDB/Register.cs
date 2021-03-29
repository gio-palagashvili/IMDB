using System;
using System.Net;
using MySql.Data.MySqlClient;

// ReSharper disable all ClassNeverInstantiated.Global
namespace IMDB
{
    internal class Register : Sql
    {
        static MySqlConnection conn = new MySqlConnection(ConnStr);
        public Register()
        {
          conn.Open();
          //login handled here
          static void CheckIfUserExists(string ip)
          {
              string sql = "select case when exists((SELECT * FROM user_tbl WHERE ip='" + ip + "'))  then 1 else 0 end";
              MySqlCommand cmd = new MySqlCommand(sql, conn);
              bool exists = (int) cmd.ExecuteScalar() == 1;
              if (exists)
              {
                  //Auto Login
                  User.Userid = GetId();
              }
              else
              {
                  Console.WriteLine("user does not exist automatic registration initiated");
                  string sql3 = "INSERT INTO user_tbl(`ip`) VALUES("+ "'" + ip + "'" + ")";
                  MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                  cmd3.ExecuteNonQuery();
                  CheckIfUserExists(GetIp());
              }
          }
          CheckIfUserExists(GetIp());
          conn.Close();
        }
        public static string GetIp()
        {
         return new WebClient().DownloadString("https://ipv4.icanhazip.com/").TrimEnd();
        }
        public static int GetId()
        {
            string sql2 = "SELECT * FROM user_tbl WHERE ip="+ "'" + GetIp() + "'" +"";
            MySqlCommand cmd1 = new MySqlCommand(sql2, conn);
            MySqlDataReader rdr = cmd1.ExecuteReader();
            rdr.Read();
            return (int) rdr[0];
        }
    }
}