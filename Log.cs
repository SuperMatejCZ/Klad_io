using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public static class Log
    {
        private static void Print(string value)
        {
            DateTime now = DateTime.Now;
            string time = $"{now.Hour}:{now.Minute}:{now.Second}.{now.Millisecond} {now.Day}.{now.Month}.{now.Year}";
            Console.WriteLine($"{time} {value}");
        }

        public static void Info(string value)
        {
            Print("[Info] " + value);
        }

        public static void Error(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Print("[Error] " + value);
            Console.ResetColor();
        }
        public static void Exception(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Print("[Exception] " + ex);
            Console.ResetColor();
        }
    }
}
