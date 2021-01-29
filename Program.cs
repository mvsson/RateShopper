using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;


namespace RateShopperForBooking.com
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool isWorked = true;
            while(isWorked)
            {
                Console.WriteLine("Вставьте относительный url отеля с букинга,\n" +
                    "то что между 'booking.com/hotel/ru/' и '.html'");
                Console.WriteLine("Например: 'ra-nevskiy-44.ru'");
                string hotelNameInput = Console.ReadLine();
                var hotel = new HotelURLs(hotelNameInput);

                DateTime dateFrom = ParseDate.GetDateTime("С какой даты начать? Введите в формате 'ГГГГ-ММ-ДД':");
                while (dateFrom < DateTime.Now)
                    dateFrom = ParseDate.GetDateTime("Дата начала должна быть в будущем, йопта");
                DateTime dateTo = ParseDate.GetDateTime("По какую дату будем смотреть? Введите в формате 'ГГГГ-ММ-ДД':");
                while (dateTo < dateFrom)
                    dateTo = ParseDate.GetDateTime("Дата конца должна быть после даты начала, йопта");

                int dateStep = ParseDate.GetInt("Введите шаг для выбора дат целым числом: \n" +
                    "Пы.Сы. При вводе '1' будет собирать цены с каждого дня подряд,\n " +
                    "что может быть долгим при большом периоде\n" +
                    "Пы.Сы.Сы. Когда-нибудь оптимизирую. Может быть. ))0");

                ParseDate parsingDates = new ParseDate(dateFrom, dateTo, dateStep);

                await StarterAsync(hotel, parsingDates); // основная программа

                Console.WriteLine("Желаете продолжить? Нажмите Y или N");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.N:
                        isWorked = false;
                        break;
                    case ConsoleKey.Y:
                        continue;
                }
            }
            Console.WriteLine();
        }
        public static async Task StarterAsync(HotelURLs hotel, ParseDate dates)
        {
            List<string> urladdresses = hotel.getUrlsList(dates);
            var pricesList = new List<List<List<string>>>();
            for (int i = 0; i < urladdresses.Count; i++)
            {
                Console.WriteLine($"\nПроисходит магия... Страница: {i + 1}/{urladdresses.Count}");
                var list_ = await PriceParser.GetPricesAsync(urladdresses[i]);
                pricesList.Add(list_);
            }
            Console.WriteLine();
            Console.WriteLine($"Минимальные цены в выбранном отеле:");
            Console.WriteLine();
            Console.WriteLine($"ПыСы: если между отображаемыми датами больше 1 дня, \n" +
                $"в этом диапазоне цены соответствуют ближайшему предыдущему значению.");
            Console.WriteLine();
            DateTime dateCount = dates.From;
            for (int i = 0; i < pricesList.Count; i++)
            {
                try
                {
                    // Этот кусок определяет совпадения между датами идущими подряд
                    // Если данные совпадают с предыдущими, то скипает вывод и переходит к следующей дате
                    if (i != 0 && pricesList[i][0][0] == pricesList[i - 1][0][0]
                        && pricesList[i][0][1] == pricesList[i - 1][0][1])
                    {
                        dateCount = dateCount.AddDays(dates.Step);
                        continue;
                    }
                    Console.Write("Цена на дату: " + dateCount.ToString("yyyy-MM-dd"));
                    Console.Write(pricesList[i][0][0] + pricesList[i][0][1]);
                    dateCount = dateCount.AddDays(dates.Step);
                }
                catch 
                {
                    Console.WriteLine("\nВозникла ошибка, проверьте наличие цен на выбранные даты");
                    break;
                }
            }
        }
    }
}