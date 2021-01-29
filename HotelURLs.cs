using System;
using System.Collections.Generic;
using System.Text;

namespace RateShopperForBooking.com
{
    class HotelURLs
    {
        string HotelLink { get; }

        public HotelURLs(string hotelLink)
        {
            HotelLink = hotelLink;
        }

        public string getHotelPage()
        {
            string _url = $"https://www.booking.com/hotel/ru/{HotelLink}.html";
            return _url;
        }
        public List<string> getUrlsList(in ParseDate range)
        {
            string _url = this.getHotelPage();
            List<string> result = new List<string>();
            DateTime checkin = range.From;
            DateTime checkout = checkin.AddDays(1);
            while (checkin < range.To)
            {
                result.Add(_url + $"?checkin={checkin:yyyy-MM-dd};checkout={checkout:yyyy-MM-dd}");
                checkin = checkin.AddDays(range.Step);
                checkout = checkout.AddDays(range.Step);
            }
            return result;
        }
        // ?checkin=2021-03-13;checkout=2021-03-14
    }
}
