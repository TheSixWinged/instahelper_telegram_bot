using InstaHelper.Bot;
using InstaHelper.DataBase;
using System;

namespace InstaHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BotWorker worker = new BotWorker())
            {
                while(true)
                {
                    if (Console.ReadLine()=="exit")
                    {
                        break;
                    }
                }
            }
        }
    }
}
