using System;

namespace Twitter
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = "";
            var token = "";
            Twitter twitter = new Twitter();
            var response = twitter.GetTwittersAsync(username, 10, token).Result;
            Console.WriteLine(response);
        }
    }
}
