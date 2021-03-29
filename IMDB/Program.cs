using System;
using System.Net;
using System.Xml;

namespace IMDB
{
    internal static class Program
    {
        private static string GetIp()
        {
            return new WebClient().DownloadString("https://ipv4.icanhazip.com/").TrimEnd();
        }
        private static void Text()
        {
                // Console.WriteLine("Hello User {0}",userId);
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
        private static void Handler(int v)
        {
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
        static void Main()
        {
            // Text();var x = int.Parse(Console.ReadLine()!);
            // Handler(x);
            var k = new Register();
            
        }
        }
    }