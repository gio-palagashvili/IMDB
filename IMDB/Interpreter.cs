using System;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Driver.Core.Operations;

// ReSharper disable All
namespace IMDB
{
    public class Interpreter : AddToDb
    {
        public static string RandomString(int size, bool lowerCase = true)  
        {  
            var builder = new StringBuilder(size);
            Random random = new Random();
            char offset = lowerCase ? 'a' : 'A';  
            const int lettersOffset = 26;
  
            for (var i = 0; i < size; i++)  
            {  
                var @char = (char)random.Next(offset, offset + lettersOffset);  
                builder.Append(@char);  
            }  
  
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();  
        }

        public static string CastAsString(string name, int rating)
        {
            return $"{name}/{rating}";
        }
        public static string GenerateMovieWithRating()
        {
            Random random = new Random();
            var size = random.Next(3, 15);
            var movieName = RandomString(size);
            int movieRating = random.Next(0, 10);
            
            return CastAsString(movieName,movieRating);
        }
        public int Action { get; set; }
        public static void WriteMain()
        {
        }

        public Interpreter()
        {
            if(Action == 6) WriteMain();
            
        }
    }
}