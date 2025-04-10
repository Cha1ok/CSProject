using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsParcer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Парсер новостей");
            string url = "http://feeds.bbci.co.uk/news/world/rss.xml";

            try
            {
                NewsParcer(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void NewsParcer(string url)
        {
            using(var reader= XmlReader.Create(url)) {
                var feed=SyndicationFeed.Load(reader);
                Console.WriteLine($"\nпоследние новости из {feed.Title.Text}:\n");
                foreach(var item in feed.Items)
                {
                    Console.WriteLine($"📌 {item.Title.Text}");
                    Console.WriteLine($"🔗 {item.Links[0].Uri}");
                    Console.WriteLine($"📅 {item.PublishDate.DateTime.ToShortDateString()}");
                    Console.WriteLine("---");
                }
            }
        }
    }
}
