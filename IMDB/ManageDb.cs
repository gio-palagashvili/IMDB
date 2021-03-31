using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;
using System.Runtime;
using System.Security.AccessControl;
using System.Threading.Channels;
using Ubiety.Dns.Core;

namespace IMDB
{
    public class ManageDb : Sql
    {
        protected static void GetRandomMovieAndRate()
        {
            var random = new Random();
            using var conn = new MySqlConnection(ConnStr);
            conn.Open();
            const string sql = "SELECT COUNT(*) FROM movies_tbl";
            using var cmd = new MySqlCommand(sql, conn);
            var count = cmd.ExecuteScalar();
            var rand = random.Next(1, int.Parse(count.ToString()!));
            var l = 0;
            while (CheckIfRated(rand.ToString()))
            {
                rand = random.Next(1, int.Parse(count.ToString()!));
                if (l + 1 == int.Parse(count.ToString()!))
                {
                    Console.WriteLine("you have rate every movie");
                    break;
                }
                l++;
            }
            Movie randomMovie = GetShowFromIndex(rand.ToString());
            Console.WriteLine("Show Selected '{0}'", randomMovie.MovieName);
            Console.WriteLine($"what would you rate the selected show?(max. 10) ");
            var rating = Console.ReadLine();
            while (int.Parse(rating!) > 10)
            {
                Console.WriteLine("what would you rate the selected show?(max. 10)");
                rating = Console.ReadLine();
            }

            RateShow.Rate(rand.ToString(), rating);
            Console.WriteLine($"You rerate '{randomMovie.MovieName}' {rating}/10");
            
            Console.ReadKey();
        }

        protected static void InsertInto()
        {
            Console.WriteLine("Input movie title");
            var title = Console.ReadLine();
            Console.WriteLine("Input movie desc");
            var desc = Console.ReadLine();
            var conn = new MySqlConnection(ConnStr);
            string sql = "INSERT INTO `movies_tbl`(`movie_name`, `movie_desc`) VALUES(@a,@b)";
            conn.Open();
            try
            {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@a", title);
                cmd.Parameters.AddWithValue("@b", desc);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static Movie GetShowFromIndex(string index)
        {
            const string sql = "SELECT * FROM movies_tbl WHERE id = @a";
            using var conn = new MySqlConnection(ConnStr);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@a", index);
            using var rdr = cmd.ExecuteReader();
            rdr.Read();
            var movieName = rdr.GetString(1);
            var movieOverAllRating = rdr.GetString(3);
            rdr.Close();
            conn.Close();
            return new Movie() {MovieName = movieName, MovieOverAllRating = movieOverAllRating};
        }

        protected static void EstablishShowRating(string id)
        {
            const string sql = "SELECT * FROM rates_tbl WHERE movie_id = @a";
            using var conn = new MySqlConnection(ConnStr);
            conn.Open();
            
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@a", id);
            using var rdr = cmd.ExecuteReader();
            int ratesSum = 0;
            int ratesAmount = 0;
            
            while (rdr.Read())
            {
                ratesAmount++;
                ratesSum += int.Parse(rdr.GetString(2));
            }
            rdr.Close();
            
            var rating = (ratesSum / ratesAmount).ToString();
            const string sqlUpdate = "UPDATE movies_tbl SET movie_rating = @movieRating WHERE id = @id";
            using var cmdUpdate = new MySqlCommand(sqlUpdate, conn);
            cmdUpdate.Parameters.AddWithValue("@movieRating", rating);
            cmdUpdate.Parameters.AddWithValue("@id", id);
            cmdUpdate.ExecuteNonQuery();
        } 
        private static void SqlInsertInto(string movieName, string movieDesc, string movieRating)
        {
            const string connStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
            var sql = "INSERT INTO `movies_tbl`(`movie_name`,`movie_desc`,`movie_rating`) VALUES('" + movieName +
                      "','" + movieDesc + "','" + movieRating + "'" + ")";
            var conn = new MySqlConnection(connStr);
            conn.Open();
            try
            {
                var cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected static void ReadFileAndInsert(string path)
        {
            var line = "";
            using var sr = new StreamReader(path);
            while ((line = sr.ReadLine()) != null)
            {
                SqlInsertInto(line.Split("/")[0], line.Split("/")[2], line.Split("/")[1]);
            }
        }

        protected static void GetTop(int limit)
        {
            var result = new List<string>();
            const string connStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
            const string sql = "SELECT * FROM movies_tbl ORDER BY movie_rating DESC";
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            using var rdr = cmd.ExecuteReader();
            var count = 1;
            while (rdr.Read())
            {
                result.Add(rdr.GetString(1));
            }

            rdr.Close();
            foreach (var x in result)
            {
                Console.WriteLine($"{count}){x}");
                count++;
            }

            conn.Close();
        }

        private static bool CheckIfRated(string movieId)
        {
            using var conn = new MySqlConnection(ConnStr);
            conn.Open();
            var sql = "SELECT * FROM rates_tbl WHERE user_id = @a AND movie_id = @b";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@a", User.Userid);
            cmd.Parameters.AddWithValue("@b", movieId);
            using var rdr = cmd.ExecuteReader();
            try
            {
                rdr.Read();
                var z = rdr[1].ToString();
                conn.Close();
                rdr.Close();
                if (z == User.Userid.ToString())
                {
                    return true;
                }

                return false;
            }
            catch (MySqlException)
            {
            }

            return false;
        }

        protected static void GetSearchResAndRate(string term)
        {
            var titles = new List<string>();
            var ratings = new List<string>();
            var ids = new List<string>();

            var sql = "SELECT * FROM movies_tbl WHERE movie_name LIKE" + "'%" + term + "%'" + "";
            using var conn = new MySqlConnection(ConnStr);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                titles.Add(rdr.GetString(1));
                ratings.Add(rdr.GetString(3));
                ids.Add(rdr.GetString(0));
            }

            for (var i = 0; i < titles.Count; i++)
            {
                Console.WriteLine(CheckIfRated(ids[i])
                    ? $"{i + 1}) {titles[i]} | Rating : {ratings[i]}/10  | your Rating ?/10"
                    : $"{i + 1}) {titles[i]} | Rating : {ratings[i]}/10");
            }

            Console.WriteLine("-------------------------------");
            Console.WriteLine("Would you like to rate/add to your list one of the shows?(y/n)");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                try
                {
                    Console.WriteLine("input the index");
                    var z = int.Parse(Console.ReadLine() ?? string.Empty);
                    if (CheckIfRated((ids[z - 1])))
                    {
                        Console.WriteLine(
                            "what would you rate '{0}'? (max Rating = 10) Note : you've already review this show this actions will simply update your score",
                            titles[z - 1]);
                        var rating = Console.ReadLine();
                        RateShow.RateUpdate(ids[z - 1], rating);
                        Console.WriteLine($"You rated '{titles[z - 1]}' {rating}/10");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("what would you rate '{0}'? (max Rating = 10)", titles[z - 1]);
                        var rating = Console.ReadLine();
                        RateShow.Rate(ids[z - 1], rating);
                        Console.WriteLine($"You rated '{titles[z - 1]}' {rating}/10");
                        Console.ReadKey();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("bro..");
                }
            }

            conn.Close();
        }

        protected static void MyList()
        {
            Console.WriteLine("-------------------- Your List --------------------------");
            var ids = new List<Movie>();
            const string sql = "SELECT * FROM rates_tbl WHERE user_id = @a";
            using var conn = new MySqlConnection(ConnStr);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@a", User.Userid);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ids.Add(new Movie() {MovieId = rdr.GetString(0), MovieUserRating = rdr.GetString(2)});
            }

            rdr.Close();
            foreach (var x in ids)
            {
                var z = GetShowFromIndex(x.MovieId);
                Console.WriteLine(
                    $"{z.MovieName} | Your Rating : {x.MovieUserRating} | overall rating : {z.MovieOverAllRating}");
            }

            conn.Close();
            Console.WriteLine("---------------------------------------------------------");
            Console.ReadKey();
        }
    }
}