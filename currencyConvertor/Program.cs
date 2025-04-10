using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyConvertor
{
    public class CbrResponse
    {
        public DateTime Date { get; set; }
        public Valutes Valute { get; set; }
    }

    public class Valutes
    {
        public CurrencyInfo USD { get; set; }
        public CurrencyInfo EUR { get; set; }
        public CurrencyInfo GBP { get; set; }
    }

    public class CurrencyInfo
    {
        public string CharCode { get; set; }
        public decimal Value { get; set; }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Конвертер валют");
            string url = "https://www.cbr-xml-daily.ru/daily_json.js";
            await CurrencyConvertor(url);
        }

        public static async Task CurrencyConvertor(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    var data = JsonConvert.DeserializeObject<CbrResponse>(json);

                    Console.WriteLine($"\nКурсы валют на {data.Date:dd.MM.yyyy}:\n");
                    Console.WriteLine("USD: " + data.Valute.USD.Value);
                    Console.WriteLine("EUR: " + data.Valute.EUR.Value);
                    Console.WriteLine("GBP: " + data.Valute.GBP.Value);

                    Console.Write("\nВведите сумму в рублях: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal rubAmount))
                    {
                        Console.Write("Выберите валюту (USD/EUR/GBP): ");
                        string currency = Console.ReadLine().ToUpper();
                        decimal rate=0;

                        switch (currency)
                        {
                            case "USD":  rate = data.Valute.USD.Value;
                                break;
                            case "EUR": rate = data.Valute.EUR.Value;
                                break;
                            case "GBP": rate= data.Valute.GBP.Value;
                                break;
                            default:
                                Console.WriteLine("Неизвестно");
                                break;
                        }


                        decimal convertedAmount = rubAmount / rate;
                        Console.WriteLine($"\n{rubAmount} RUB = {convertedAmount:F2} {currency}");
                    }
                    else
                    {
                        Console.WriteLine("Некорректная сумма!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}