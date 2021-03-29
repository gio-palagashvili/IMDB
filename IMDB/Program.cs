using System;

namespace IMDB
{
    // ReSharper disable all RedundantAssignment
    // ReSharper disable once ClassNeverInstantiated.Global

    internal class Program : Register
    {
        private static void Handler(int v)
        {
            // ReSharper disable once NotAccessedVariable
            Interpreter k;
            switch (v)
            {
                case 1:
                    k = new Interpreter(1, "", 0);
                    break;
                case 5:
                    Console.WriteLine("input the path of the file");
                    var path = Console.ReadLine();
                    k = new Interpreter(5, path, 0);
                    break;
                case 6:
                    Console.WriteLine("file has been created");
                    k = new Interpreter(6, "", 10);
                    break;
                case 7:
                    k = new Interpreter(7, "", 0);
                    break;
                case 8:
                    Console.WriteLine("Input movie title");
                    var temp = Console.ReadLine();
                    k = new Interpreter(8, temp, 0);
                    Console.WriteLine("--------");
                    break;
            }
        }

        private static void Text()
        {
            var k = new Register();
            Console.WriteLine("--------------");
            Console.WriteLine("Choose an option :");
            Console.WriteLine("1)list the top 100");
            Console.WriteLine("2)Add a movie to your list"); //todo
            Console.WriteLine("3)Rate a random show"); //todo
            Console.WriteLine("4)Add A movie to The Database"); // todo
            Console.WriteLine("5)Add Movies to the db with a file");
            Console.WriteLine("6)write a random movie list file");
            Console.WriteLine("7)My List");
            Console.WriteLine("8)search for a movie"); // todo update ratingHelper
        }

        static void Main()
        {
            while (true)
            {
                Text();
                try
                {
                    var x = int.Parse(Console.ReadLine()!);
                    Handler(x);
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
        }
    }
}
    