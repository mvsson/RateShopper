using System;

namespace RateShopperConsole
{
    class dateSettings
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public int Step { get; }

        public dateSettings(in DateTime start, in DateTime end, in int parseStep)
        {
            Start = start;
            End = end;
            Step = parseStep;
        }

        public static DateTime GetDateTime(string writeLine = "Введите дату в формате 'ГГГГ-ММ-ДД': ")
        {
            string input;
            bool isparsed;
            DateTime date;
            do
            {
                Console.WriteLine(writeLine);
                input = Console.ReadLine();
                isparsed = DateTime.TryParse(input, out date);
            } while (!isparsed);
            return date;
        }

        public static int GetInt(string writeLine = "Введите целое число: ")
        {
            string input;
            bool isparse;
            int step;
            do
            {
                Console.WriteLine(writeLine);
                input = Console.ReadLine();
                isparse = int.TryParse(input, out step);
            } while (!isparse);
            return step;
        }
    }
}
