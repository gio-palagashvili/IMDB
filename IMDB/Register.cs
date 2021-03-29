using System;
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;

namespace IMDB
{
    // ReSharper disable all ClassNeverInstantiated.Global
    internal class Register : Sql
    {
        static MySqlConnection conn = new MySqlConnection(ConnStr);
        public Register()
        {
          conn.Open();

          static int CheckIfUserExists(string ip)
          {
              string sql = "select case when exists((SELECT * FROM user_tbl WHERE ip='" + ip + "'))  then 1 else 0 end";
              MySqlCommand cmd = new MySqlCommand(sql, conn);
              bool exists = (int) cmd.ExecuteScalar() == 1;
              int id = -1;
              if (exists)
              {
                  string sql2 = "SELECT * FROM user_tbl WHERE ip='" + ip + "'";
                  MySqlCommand cmd1 = new MySqlCommand(sql2, conn);
                  MySqlDataReader rdr = cmd1.ExecuteReader();
                  rdr.Read();
                  id = (int) rdr[0];
              }
              else
              {
                  Console.WriteLine("user does not exist automatic registration initiated");
                  string sql3 = "INSERT INTO user_tbl(`ip`) VALUES("+ "'" + ip + "'" + ")";
                  MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                  cmd3.ExecuteNonQuery();
                  CheckIfUserExists(GetIp());
              }

              return id;
          }

          CheckIfUserExists(GetIp());
        }
        public static string GetIp()
        {
         return new WebClient().DownloadString("https://ipv4.icanhazip.com/").TrimEnd();
        }
    
    }
}