using System;
using MySql.Data.MySqlClient;

namespace IMDB
{
    public class Sql
    {
        protected Sql()
        {
            const string connStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
            
            static void SqlInsertInto(string movieName,string movieDesc,int movieRating)
            {
                var conn = new MySqlConnection(connStr);
                conn.Open();
                try
                {
                    const string sql = "INSERT INTO movies_db(movie_name,movie_desc,movie_rating) VALUES(movieName,movieDesc,movieRating)";
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            
                conn.Close();
            }

            static void SqlRead(string comm)
            { 
                var conn = new MySqlConnection(connStr);
             conn.Open();
             conn.Close();
            }
        }
    }
}