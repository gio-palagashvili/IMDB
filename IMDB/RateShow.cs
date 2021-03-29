using System;
using MySql.Data.MySqlClient;

namespace IMDB
{
    public static class RateShow
    {
        private const string ConnStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
        public static void Calc(){}
        public static void Rate(string id,string rating)
        {
            if (int.Parse(rating) > 10 )
            {
                Console.WriteLine("...");
                Environment.Exit(0);
            }
            else
            {
                using var conn = new MySqlConnection(ConnStr);
                conn.Open();
                var sql = "INSERT INTO `rates_tbl`(`movie_id`, `user_id`, `movie_score`) VALUES(@movieId,@userId,@movieRating)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@movieId", id);
                cmd.Parameters.AddWithValue("@userId", User.Userid);
                cmd.Parameters.AddWithValue("@movieRating", rating);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void RateUpdate(string id, string rating)
        {
            if (int.Parse(rating) > 10 )
            {
                Console.WriteLine("...");
                Environment.Exit(0);
            }
            else
            {
                using var conn = new MySqlConnection(ConnStr);
                conn.Open();
                var sql = "UPDATE rates_tbl SET movie_score = @rating WHERE user_id = @userid AND movie_id = @movieId";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@userId", User.Userid);
                cmd.Parameters.AddWithValue("@movieId", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private static void RatingHelper(string id){}
    }
}