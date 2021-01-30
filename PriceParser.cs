using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace RateShopperConsole
{
    class PriceParser
    {
        public static async Task<List<List<string>>> GetPricesAsync(string urladdress)
        {
            var config = Configuration.Default.WithDefaultLoader(); // конфиг для AngleSharp
            var document = await BrowsingContext.New(config).OpenAsync(urladdress); // DOM исходник веб страницы
            var blocks = GetParse(in document, "tr", "js-rt-block-row "); // получаем блоки с категориями номеров и ценами
            List<List<string>> result = new List<List<string>> ();
            foreach (var item in blocks)
            { 
                List<string> _ = new List<string>(); 
                // парсим названия категорий
                var category = GetParse(in item, "span", "hprt-roomtype-icon-link "); 
                foreach (var item_ in category)
                { _.Add(item_.TextContent); }
                // парсим цены
                var price = GetParse(in item, "div", "bui-price-display__value prco-inline-block-maker-helper prco-font16-helper ");
                foreach (var item_ in price)
                { _.Add(item_.TextContent); }
                result.Add(_);
            }
            return result;
        }

        private static IEnumerable<IElement> GetParse(in IElement source, string htmlteg, string htmlclass)
        {
            var block = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return block;
        }
        private static IEnumerable<IElement> GetParse(in IDocument source, string htmlteg, string htmlclass)
        {
            var block = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return block;
        }
        private static IEnumerable<IElement> GetParse(in IHtmlDocument source, string htmlteg, string htmlclass)
        {
            var block = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return block;
        }
    }
    
}
