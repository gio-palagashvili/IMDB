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
                case 5:
                    Console.WriteLine("input the path of the file");
                    var path = Console.ReadLine();
                    k = new Interpreter(5,path,0);
                    break;
                case 6:
                    Console.WriteLine("file has been created");
                    k = new Interpreter(6,"",10);
                    break;
            }
        }
        private static void Text()
        {
                var k = new Register();
                Console.WriteLine("Choose an option :");
                Console.WriteLine("1)Check-out the top 100");
                Console.WriteLine("2)Add a movie to your list");
                Console.WriteLine("3)Rate a random movie");
                Console.WriteLine("4)Add A movie to The Database");
                Console.WriteLine("5)Add Movies to the db with a file");
                Console.WriteLine("6)write a random movie list file");
                Console.WriteLine("7)list all movies with the ability to review them");
                Console.WriteLine("8)search for a movie"); 
        }
        static void Main()
        {
            Text();var x = int.Parse(Console.ReadLine()!);
            Handler(x);
        }
        }
    }