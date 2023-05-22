using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Server
{
    public static class Log
    {
        public static readonly object LockObj = new object();

        private static void Print(string value)
        {
            lock (LockObj) {
                DateTime now = DateTime.Now;
                string time = $"{now.Hour}:{now.Minute}:{now.Second}.{now.Millisecond} {now.Day}.{now.Month}.{now.Year}";
                Console.WriteLine($"{time} {value}");
            }
        }

        public static void Debug(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Print("[Debug] " + value);
            Console.ResetColor();
        }

        public static void Info(string value)
        {
            Print("[Info] " + value);
        }

        public static void Error(string value)
        {
            lock (LockObj) {
                Console.ForegroundColor = ConsoleColor.Red;
                Print("[Error] " + value);
                Console.ResetColor();
            }
        }
        public static void Exception(Exception ex)
        {
            lock (LockObj) {
                Console.ForegroundColor = ConsoleColor.Red;
                Print("[Exception] " + ex);
                Console.ResetColor();
            }
        }

        public static void Socket(string value)
        {
            Print($"[Socket] {value}");
        }

        public static void PressAnyKey(string message = "", bool exit = false)
        {
            lock (LockObj) {
                string mes;
                if (exit)
                    mes = message == "" ? "Press any key to exit..." : $"{message}, press any key to exit...";
                else
                    mes = message == "" ? "Press any key to continue..." : $"{message}, press any key to continue...";
                Console.WriteLine(mes);
                Console.ReadKey(true);
                if (exit)
                    Environment.Exit(-1);
            }
        }
    }
}
