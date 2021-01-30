using System;
using System.Collections.Generic;

namespace RateShopperConsole
{
    class URLSettings
    {
        string HotelLink { get; }

        public URLSettings(string hotelLink)
        {
            HotelLink = hotelLink;
        }

        public string getHotelPage()
        {
            string _url = $"https://www.booking.com/hotel/ru/{HotelLink}.html";
            return _url;
        }
        public string[] getUrlsList(in dateSettings range)
        {
            string _url = this.getHotelPage();
            List<string> result = new List<string>();
            DateTime checkin = range.Start;
            DateTime checkout = checkin.AddDays(1);
            while (checkin < range.End)
            {
                result.Add(_url + $"?checkin={checkin:yyyy-MM-dd};checkout={checkout:yyyy-MM-dd}");
                checkin = checkin.AddDays(range.Step);
                checkout = checkout.AddDays(range.Step);
            }
            return result.ToArray();
        }
        // ?checkin=2021-03-13;checkout=2021-03-14
    }
}
