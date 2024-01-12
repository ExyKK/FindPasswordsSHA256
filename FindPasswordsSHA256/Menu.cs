using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FindPasswordsSHA256
{
    class Menu
    {
        private readonly string[] menuItems = new string[] { "Ввести хэш", "Считать хэши из файла", "Выход" };
        private readonly string[] menuThreadOptions = new string[] { "Однопоточный", "Многопоточный", "Назад" };

        public void Run()
        {
            Console.WriteLine("Меню\n");
            ShowMenuItems(menuItems);

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        ClearAndWrite(menuItems[0]);
                        ShowMenuItems(menuThreadOptions);                        
                        bool exit = false;
                        while (!exit)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.D1:
                                    BruteforceHash.SingleThread(HashEnter().ToUpper()); // Один поток, ввод хэша
                                    break;

                                case ConsoleKey.D2:
                                    BruteforceHash.MultiThreads(HashEnter().ToUpper()); // Многопоточный, ввод хэша
                                    break;

                                case ConsoleKey.D3:
                                    Exit(ref exit, menuThreadOptions[2], menuItems); // Назад
                                    break;
                            }
                        }
                        break;

                    case ConsoleKey.D2:
                        ClearAndWrite(menuItems[1]);
                        ShowMenuItems(menuThreadOptions);
                        bool exitFile = false;
                        while (!exitFile)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.D1:
                                    BruteforceHash.SingleThread(HashFromFile(), true); // Один поток, хэш из файла
                                    break;

                                case ConsoleKey.D2:
                                    BruteforceHash.MultiThreads(HashFromFile(), true); // Многопоточный, хэш из файла
                                    break;

                                case ConsoleKey.D3:
                                    Exit(ref exitFile, menuThreadOptions[2], menuItems); // Назад
                                    break;
                            }
                        }
                        break;

                    case ConsoleKey.D3:
                        ClearAndWrite(menuItems[2]);                 
                        return;

                    default:
                        ClearAndWrite("Введите цифру для выбора");
                        ShowMenuItems(menuItems);
                        break;
                }
            }
        }

        private void ShowMenuItems(string[] menuItems)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i]}");
            }
            Console.WriteLine();
        }

        private void ClearAndWrite(string value)
        {
            Console.Clear();
            Console.WriteLine(value + '\n');
        }

        private void Exit(ref bool exit, string value, string[] items)
        {
            ClearAndWrite(value);
            ShowMenuItems(items);
            exit = true;
        }

        private string HashEnter()
        {
            while (true)
            {
                ClearAndWrite("Введите хэш");
                string hash = Console.ReadLine();

                while (!Regex.IsMatch(hash, @"^[A-Za-z0-9]*$") || string.IsNullOrEmpty(hash))
                {
                    ClearAndWrite("Хэш может содержать только цифры и буквы от A до F в любом регистре\nВведите хэш повторно");
                    hash = Console.ReadLine();
                }

                return hash;
            }
        }

        private string HashFromFile()
        {
            while (true)
            {
                ClearAndWrite("Введите путь до файла с хэшами");
                string path = Console.ReadLine()?.Trim('"');

                while (!File.Exists(path) || string.IsNullOrEmpty(path))
                {
                    ClearAndWrite("Проверьте правильность введённого пути\nВведите путь повторно");
                    path = Console.ReadLine()?.Trim('"');
                }

                return path;
            }
        }
    }
}
