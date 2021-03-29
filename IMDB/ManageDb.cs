using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.IO;
using System.Runtime;
using Ubiety.Dns.Core;

namespace IMDB
{
    public class AddToDb : Sql
    {
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

        protected static void ReadFile(string path)
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

        protected static void GetSearchResAndVote(string term)
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
                    ? $"{i + 1}){titles[i]} | Rating : {ratings[i]}/10  | your Rating ?/10"
                    : $"{i + 1}){titles[i]} | Rating : {ratings[i]}/10");
            }

            Console.WriteLine("Would you like to rate one of the shows?(yes/no)");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                try
                {
                    Console.WriteLine("input the index");
                    var z = int.Parse(Console.ReadLine() ?? string.Empty);
                    if (CheckIfRated((ids[z - 1])))
                    {
                        Console.WriteLine("what would you rate '{0}'? (max Rating = 10) Note : you've already review this show this actions will simply update your score", titles[z - 1]);
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
    }
}