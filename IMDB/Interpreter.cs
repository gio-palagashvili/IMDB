using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Driver.Core.Operations;

// ReSharper disable All
namespace IMDB
{
    public class Interpreter : AddToDb
    {
        public int Action { get; set; }
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
            return $"{name}/{rating}/loremLorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam porttitor sit amet dolor id vulputate. Nam ac arcu aliquam, pretium dolor vel, lacinia sem. In suscipit dui quis risus auctor lobortis";
        }
        public static string GenerateMovieWithRating()
        {
            Random random = new Random();
            var size = random.Next(3, 15);
            var movieName = RandomString(size);
            int movieRating = random.Next(1, 10);
            
            return CastAsString(movieName,movieRating);
        }
        public static List<string> WriteMain(int numberOfMovies)
        {
            List<string> Values = new List<string>();
            for (int i = 0; i < numberOfMovies; i++)
            {
                Values.Add(GenerateMovieWithRating());                
            }

            var path = @"C:\Users\Gio\Documents\c#\IMDB\IMDB\text.txt";
            using (StreamWriter sw = new StreamWriter(path)) 

                foreach (string s in Values) {
                    sw.WriteLine(s);
                }

            return Values;
        }
        public Interpreter()
        {
            Random random = new Random();
            if(Action == 6) WriteMain(random.Next(3,10));
        }
    }

}