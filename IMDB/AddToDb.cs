using System;
using MySql.Data.MySqlClient; 
using System.IO;
namespace IMDB
{
    public class AddToDb : Sql
    {
        static void SqlInsertInto(string movieName,string movieDesc,string movieRating)
        {
            const string connStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
            var sql = "INSERT INTO `movies_tbl`(`movie_name`,`movie_desc`,`movie_rating`) VALUES('" + movieName +"','" + movieDesc + "','" + movieRating + "'" + ")";
            var conn = new MySqlConnection(connStr);
            conn.Open();
            try
            {
                var cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            conn.Close();
        }

        protected static void ReadFile(string path)
        {
            var line = "";
            using var sr = new StreamReader(path);
            while ((line = sr.ReadLine()) != null)
            {
                SqlInsertInto(line.Split("/")[0], line.Split("/")[2], line.Split("/")[1]);
            }
        }
    }
}