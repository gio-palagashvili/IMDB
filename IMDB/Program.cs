using System;

namespace IMDB
{
    internal static class Program
    {
        private static void Text()
        {
            Console.WriteLine("Choose an option :");
            Console.WriteLine("1)Check-out the top 100");
            Console.WriteLine("2)Add a movie to your list");
            Console.WriteLine("3)Rate a random movie");
            Console.WriteLine("4)Add A movie to The Database");
            Console.WriteLine("5)Add Movies to the db with a file");
            Console.WriteLine("6)write a random movie list file");
        }
        static void Main()
        {
            Text();var x = int.Parse(Console.ReadLine()!);
            var z = new Interpreter(){Action = 6};
            Interpreter.WriteMain(6).ForEach(Console.WriteLine);
        }
    }
}