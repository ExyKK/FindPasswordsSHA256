using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FindPasswordsSHA256
{
    class BruteforceHash
    {
        private static readonly char[] alphabet =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
            's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        public static void SingleThread(string hashOrPath, bool isFromFile=false)
        {
            DateTime start = DateTime.Now;
            int length = alphabet.Length;
            int linesCount = 0;

            for (int ch1 = 0; ch1 < length; ch1++)
            {
                string a = Convert.ToString(alphabet[ch1]);
                for (int ch2 = 0; ch2 < length; ch2++)
                {
                    string b = Convert.ToString(alphabet[ch2]);
                    for (int ch3 = 0; ch3 < length; ch3++)
                    {
                        string c = Convert.ToString(alphabet[ch3]);
                        for (int ch4 = 0; ch4 < length; ch4++)
                        {
                            string d = Convert.ToString(alphabet[ch4]);
                            for (int ch5 = 0; ch5 < length; ch5++)
                            {
                                string e = Convert.ToString(alphabet[ch5]);
                                string password = a + b + c + d + e;
                                string hashed = Hash.GetStringSha256Hash(password);

                                if (isFromFile)
                                {
                                    foreach (string line in File.ReadLines(hashOrPath, Encoding.Default))
                                    {
                                        if (!line.ToUpper().Contains(hashOrPath)) continue;

                                        Console.WriteLine($"Найден пароль: {password}");
                                        Console.WriteLine(DateTime.Now - start);
                                        linesCount++;
                                        break;
                                    }

                                    if (linesCount == File.ReadAllLines(hashOrPath).Length)
                                    {
                                        ch1 = ch2 = ch3 = ch4 = ch5 = length;
                                    }
                                }
                                else
                                {
                                    if (hashOrPath == hashed)
                                    {
                                        Console.WriteLine($"Найден пароль: {password}");
                                        Console.WriteLine(DateTime.Now - start);
                                        ch1 = ch2 = ch3 = ch4 = ch5 = length;
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
        }

        public static void MultiThreads(string hashOrPath, bool isFromFile = false)
        {
            bool flag = false;
            DateTime start = DateTime.Now;

            Parallel.For(0, 26, a =>
            {
                byte[] password = new byte[5];
                password[0] = (byte)(97 + a);
                int linesCount = 0;

                for (password[1] = 97; password[1] < 123; password[1]++)
                {
                    for (password[2] = 97; password[2] < 123; password[2]++)
                    {
                        for (password[3] = 97; password[3] < 123; password[3]++)
                        {
                            for (password[4] = 97; password[4] < 123; password[4]++)
                            {
                                string passwordString = Encoding.ASCII.GetString(password);
                                string hashed = Hash.GetStringSha256Hash(passwordString);

                                if (isFromFile)
                                {
                                    foreach (string line in File.ReadLines(hashOrPath, Encoding.Default))
                                    {
                                        if (!line.ToUpper().Contains(hashed)) continue;

                                        Console.WriteLine($"Найден пароль {passwordString}");
                                        Console.WriteLine(DateTime.Now - start);
                                        linesCount++;
                                        if (linesCount == File.ReadAllLines(hashOrPath).Length) flag = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (hashOrPath != hashed) continue;

                                    Console.WriteLine($"Найден пароль: {passwordString}");
                                    Console.WriteLine(DateTime.Now - start);
                                    flag = true;
                                    break;
                                }                                
                            }

                            if (flag) break;
                        }

                        if (flag) break;
                    }

                    if (flag) break;
                }
            });
        }
    }
}
