using System;
using System.Collections.Generic;
using System.Text;

namespace RateShopperForBooking.com
{
    class ParseDate
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public int Step { get; }

        public ParseDate(in DateTime parseFrom, in DateTime parseTo, in int parseStep)
        {
            From = parseFrom;
            To = parseTo;
            Step = parseStep;
        }

        public static DateTime GetDateTime(string writeLine = "Введите дату в формате 'ГГГГ-ММ-ДД': ")
        {
            string input;
            bool isparse;
            DateTime date;
            do
            {
                Console.WriteLine(writeLine);
                input = Console.ReadLine();
                isparse = DateTime.TryParse(input, out date);
            } while (!isparse);
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
