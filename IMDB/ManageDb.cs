using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient; 
using System.IO;
using Ubiety.Dns.Core;

namespace IMDB
{
    public class AddToDb : Sql
    {
        private static void SqlInsertInto(string movieName,string movieDesc,string movieRating)
        {
            const string connStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
            var sql = "INSERT INTO `movies_tbl`(`movie_name`,`movie_desc`,`movie_rating`) VALUES('" + movieName +"','" + movieDesc + "','" + movieRating + "'" + ")";
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
        protected static void GetSearchRes(string term)
        {
            var titles = new List<string>();
            var ratings = new List<string>();
            const string connStr = "server=localhost;user=root;database=imdb_db;port=3306;password=''";
            var sql = "SELECT * FROM movies_tbl WHERE movie_name LIKE" + "'%" + term + "%'" + "";
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            using var rdr = cmd.ExecuteReader();     
            while (rdr.Read())
            {
                titles.Add(rdr.GetString(1));                
                ratings.Add(rdr.GetString(3));
            }
            for (var i = 0; i < titles.Count; i++)
            {
                Console.WriteLine($"{i + 1}){titles[i]} | Rating : {ratings[i]}/10");    
            }
        }
    }
}