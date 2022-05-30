using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    
    /// </summary>
    /// This class had been created to keep methods of current application. And make code compactly.
    /// </summary>
    internal class Utils
    {
        
        public static void Drawtree(DirectoryInfo dir, int page)
        {
            StringBuilder tree = new StringBuilder();
            GetTree(tree, dir, "", true);
            DrawWindow(0, 0, Program.WINDOW_WIDTH, 18);
            (int currentLeft, int currentTop) = GetCursorPosition();
            int pageLines = 16;
            string[] lines = tree.ToString().Split(new char[] { '\n' });
            int pageTotal = (lines.Length + pageLines) / pageLines;
            if (page > pageTotal)
                page = pageTotal;

            for (int i = (page - 1) * pageLines, counter = 0; i < page * pageLines; i++, counter++)
            {
                if (lines.Length - 1 > i)
                {
                    Console.SetCursorPosition(currentLeft + 1, currentTop + 1 + counter);
                    Console.WriteLine(lines[i]);
                }
            }


            //footer
            string footer = $"╣ {page} of {pageTotal} ╠";
            Console.SetCursorPosition(Program.WINDOW_WIDTH / 2 - footer.Length / 2, 17);
            Console.WriteLine(footer);

        }

        public static void GetTree(StringBuilder tree, DirectoryInfo dir, string indent, bool lastDirectory)
        {
            tree.Append(indent);
            if (lastDirectory)
            {
                tree.Append("└─");
                indent += "   ";
            }
            else
            {
                tree.Append("├─");
                indent += "│  ";
            }

            tree.Append($"{dir.Name}\n");

            DirectoryInfo[] subDirects = dir.GetDirectories();

            for (int i = 0; i < subDirects.Length; i++)
                GetTree(tree, subDirects[i], indent, i == subDirects.Length - 1);
        }

        public static void DrawConsole(string dir, int x, int y, int width, int height)
        {
            DrawWindow(x, y, width, height);
            Console.SetCursorPosition(x + 1, y + height / 2);
            Console.Write($"{dir}>");
        }

        public static void UpdateConsole()
        {
            DrawConsole(Program.currentDir, 0, 26, Program.WINDOW_WIDTH, 3);
            ProcessEnterCommand(Program.WINDOW_WIDTH);
        }
        
        public static (int left, int top) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }

        public static void ProcessEnterCommand(int width)
        {
            (int left, int top) = GetCursorPosition();
            StringBuilder command = new StringBuilder();
            char key;
            do
            {
                key = Console.ReadKey().KeyChar;

                if(key != 8 && key != 13)
                    command.Append(key);

                (int currentLeft, int currentTop) = GetCursorPosition();

                if(currentLeft == width - 2)
                {
                    Console.SetCursorPosition(currentLeft - 1, top);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentLeft - 1, top);
                }
                if (key == (char)8/*ConsoleKey.Backspace*/)
                {
                    if(command.Length > 0)
                        command.Remove (command.Length - 1, 1);
                    if(currentLeft >= left)
                    {
                        Console.SetCursorPosition(currentLeft, top);
                        Console.Write(" ");
                        Console.SetCursorPosition(currentLeft, top);
                    }
                    else
                    {
                        Console.SetCursorPosition(left, top);
                    }
                }
            }
            while (key != (char)13);
            ParseCommandString(command.ToString());
        }

        public static void ParseCommandString(string command)
        {
            string[] commandParams = command.Split(' ');

            string[] prymaryCommandParams;

            

            if (commandParams.Length > 2 && commandParams[0] == "ls")
            {
                string somestr = commandParams[1] + " " + commandParams[3];

                prymaryCommandParams = somestr.Split(' '); // [0] - path, [1] - page
                //int prymaryCommandParamPage = Convert.ToInt32(prymaryCommandParams[1]);              
                Program.someInt = Convert.ToInt32(prymaryCommandParams[1]);

            }
                            

            if (commandParams.Length > 0)
            {
                switch (commandParams[0])
                {
                    case "cd":
                        if(commandParams.Length > 1 && Directory.Exists(commandParams[1]))
                        {
                            Program.currentDir = commandParams[1];
                        }

                        break;

                    case "ls":
                        if(commandParams.Length > 1 && Directory.Exists(commandParams[1]))
                        {
                            if(commandParams.Length > 3 && commandParams[2] == "-p" && int.TryParse(commandParams[3], out int n))
                            {
                                Drawtree(new DirectoryInfo(commandParams[1]), n);
                            }
                            else
                            {
                                Drawtree(new DirectoryInfo(commandParams[1]), 1);
                            }                         
                        }                        
                        break;

                    case "rm":
                        if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
                        {                            
                            //string comand = Console.ReadLine();
                            Directory.Delete(commandParams[1] + "\\" + commandParams[2]);
                            Drawtree(new DirectoryInfo(commandParams[1]), Program.someInt);
                        }
                        break;
                }
            }
            UpdateConsole();
        }

        /// </summary>
        /// Method is drawing windows in console
        /// </summary>
        /// <param name="x"> Initial position at "X" axis.
        /// <param name="y"> Initial position at "Y" axis.
        /// <param name="width"> Width of window.
        /// <param name="height"> Height of window.
        public static void DrawWindow(int x, int y, int width, int height)
        {
            Console.SetCursorPosition(x, y);

            Console.Write("╔");
            for (int i = 0; i < width - 2; i++)
                Console.Write("═");
            Console.Write("╗");

            Console.SetCursorPosition(x, y + 1);
            for (int i = 0; i < height - 2; i++)
            {
                Console.Write("║");
                for (int j = x + 1; j < x + width - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
            }

            Console.Write("╚");
            for (int i = 0; i < width - 2; i++)
                Console.Write("═");
            Console.Write("╝");
            Console.SetCursorPosition(x, y);
        }
    }
}
