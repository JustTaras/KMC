using System;
using System.Text;

class Program
{
    static void Main()
    {
        string text = "Дит’ясла";

        foreach (char c in text)
        {
            int codePoint = c;

            byte[] bytes = Encoding.UTF8.GetBytes(new char[] { c });

            Console.Write($"'{c}' -> U+{codePoint:X4} -> ");

            foreach (byte b in bytes)
            {
                Console.Write($"{b:X2} ");
            }

            Console.WriteLine($"({bytes.Length} байт)");
        }

        Console.WriteLine($"\nКількість символів: {text.Length}");
        Console.WriteLine($"Кількість байтів UTF-8: {Encoding.UTF8.GetByteCount(text)}");
    }
}