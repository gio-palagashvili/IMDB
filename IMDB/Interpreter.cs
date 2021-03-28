using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
namespace IMDB
{
    public class Interpreter : AddToDb
    {
        private static string RandomString(int size, bool lowerCase = true)  
        {  
            var builder = new StringBuilder(size);
            var random = new Random();
            var offset = lowerCase ? 'a' : 'A';  
            const int lettersOffset = 26;
  
            for (var i = 0; i < size; i++)  
            {  
                var @char = (char)random.Next(offset, offset + lettersOffset);  
                builder.Append(@char);  
            }  
  
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();  
        }
        private static string CastAsString(string name, int rating)
        {
            return $"{name}/{rating}/loremLorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam porttitor sit amet dolor id vulputate. Nam ac arcu aliquam, pretium dolor vel, lacinia sem. In suscipit dui quis risus auctor lobortis";
        }
        private static string GenerateMovieWithRating()
        {
            var random = new Random();
            var size = random.Next(3, 15);
            var movieName = RandomString(size);
            var movieRating = random.Next(1, 10);
            
            return CastAsString(movieName,movieRating);
        }
        private static List<string> WriteMain(int numberOfMovies)
        {
            var values = new List<string>();
            for (var i = 0; i < numberOfMovies; i++)
            {
                values.Add(GenerateMovieWithRating());              
            }
            const string path = @"C:\Users\Gio\Documents\c#\IMDB\IMDB\text.txt";
            using StreamWriter sw = new StreamWriter(path);
            foreach (var s in values) {
                sw.WriteLine(s);
            }
            return values;
        }
        public Interpreter(int action, string path,int rep) 
        {
            if (action == 5) ReadFile(path);
            var random = new Random();
            if(action == 6) WriteMain(rep);
        }
    }

}