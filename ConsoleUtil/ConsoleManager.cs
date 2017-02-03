using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConsoleUtil
{
    public class ConsoleManager
    {
        private static ConsoleManager instance;

        private ConsoleManager() { }

        public static ConsoleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleManager();
                }
                return instance;
            }
        }

        public String GetCommandLineValue()
        {
            return Console.ReadLine();
        }

        public ConsoleKeyInfo GetCommandLineKey()
        {
            return Console.ReadKey();
        }

        public string AnonymousChoice()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    //Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        public string LinePromptSaver()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write(info.KeyChar);
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        public string LoopSinceChoseOrExit(List<string> list, string exit)
        {
            String result;
            while (!list.Contains(result = LinePromptSaver()) && !result.Equals(exit))
            {
            }
            return result;
        }

        public void LoopSinceExit(string msg, string exit, Action action)
        {
            String exitter = "";
            while (true)
            {
                Console.Clear();
                action.Invoke();
                
                Console.WriteLine(msg);
                exitter = LinePromptSaver();
                if (exitter == exit)
                {
                    break;
                }
            }
        }

        public void LoopSinceExitOrValidate(string exit, string validate, Action action, Action onExit, Action onValidate)
        {
            String result;
            do
            {
                Console.Clear();
                action.Invoke();
            } while ((result = LinePromptSaver()) != exit && result != validate );
            if (result == exit)
            {
                onExit.Invoke();
            }
            else
            {
                onValidate.Invoke();
            }
        }

        public String LoopSinceChoseOrExit(Dictionary<string, object>.KeyCollection keys, String exit)
        {
            String result;
            while (!keys.Contains(result = LinePromptSaver()) && !result.Equals(exit))
            {
            }
            return result;
        }

        public void LoopSinceExit(String exit, Action action)
        {
            do
            {
                Console.Clear();
                action.Invoke();
            } while (!LinePromptSaver().Equals(exit)) ;
        }
        
        public String DetectExit()
        {
            String saveInput = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    saveInput += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(saveInput))
                    {
                        // remove one character from the list of password characters
                        saveInput = saveInput.Substring(0, saveInput.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                Console.ReadKey(false);
                if (saveInput == "exit")
                {
                    Environment.Exit(0);
                }
            }
            Console.WriteLine();
            return saveInput;
        }
        

        public void Issue(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
        }
    }
}
