using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    internal class Program
    {

        public const int WINDOW_HEIGHT = 30;
        public const int WINDOW_WIDTH = 120;
        public static string currentDir = Directory.GetCurrentDirectory();
        public static int someInt = 0;
        
        static void Main(string[] args)
        {
           
            Console.Title = "File Manager";

            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Console.SetBufferSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            Utils.DrawWindow(0, 0, WINDOW_WIDTH, 18);
            Utils.DrawWindow(0, 18, WINDOW_WIDTH, 8);

            Utils.UpdateConsole();

            Console.ReadKey(true);

            
        }        
    }
}