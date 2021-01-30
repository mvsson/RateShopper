using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RateShopperConsole
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
                var hotel = new URLSettings(hotelNameInput);

                DateTime dateFrom = dateSettings.GetDateTime("С какой даты начать? Введите в формате 'ГГГГ-ММ-ДД':");
                while (dateFrom < DateTime.Now)
                    dateFrom = dateSettings.GetDateTime("Дата начала должна быть в будущем, йопта");
                DateTime dateTo = dateSettings.GetDateTime("По какую дату будем смотреть? Введите в формате 'ГГГГ-ММ-ДД':");
                while (dateTo < dateFrom)
                    dateTo = dateSettings.GetDateTime("Дата конца должна быть после даты начала, йопта");

                int dateStep = dateSettings.GetInt("Введите шаг для выбора дат целым числом: \n" +
                    "Пы.Сы. При вводе '1' будет собирать цены с каждого дня подряд,\n " +
                    "что может быть долгим при большом периоде\n" +
                    "Пы.Сы.Сы. Когда-нибудь оптимизирую. Может быть. ))0");

                dateSettings parsingDates = new dateSettings(dateFrom, dateTo, dateStep);

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

        public static async Task StarterAsync(URLSettings hotel, dateSettings dates)
        {
            var urlAddresses = hotel.getUrlsList(dates);
            var pricesList = new List<List<List<string>>>();
            for (int i = 0; i < urlAddresses.Length; i++)
            {
                Console.WriteLine($"\nПроисходит магия... Страница: {i + 1}/{urlAddresses.Length}");
                var list_ = await PriceParser.GetPricesAsync(urlAddresses[i]);
                pricesList.Add(list_);
            }
            Console.WriteLine();
            Console.WriteLine($"Минимальные цены в выбранном отеле:");
            Console.WriteLine();
            Console.WriteLine($"ПыСы: если между отображаемыми датами больше 1 дня, \n" +
                $"в этом диапазоне цены соответствуют ближайшему предыдущему значению.");
            Console.WriteLine();
            DateTime dateCount = dates.Start;
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