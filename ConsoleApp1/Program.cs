using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class ForecastResponse
    {
        public List<ForecastItem> list { get; set; }
        public City city { get; set; }
    }

    public class ForecastItem
    {
        public long dt { get; set; }
        public MainData main { get; set; }
        public List<Weather> weather { get; set; }
        public Wind wind { get; set; }
        public string dt_txt { get; set; }
    }

    public class MainData
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int humidity { get; set; }
    }

    public class Weather
    {
        public string description { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
    }

    public class City
    {
        public string name { get; set; }
    }

    internal class Program
    {
        static readonly string API = "51dd1e0e112972d32bf0a64c9b80cf67";
        static readonly string BASE_URL = "http://api.openweathermap.org/data/2.5/forecast";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите название города:");
            string city = Console.ReadLine();

            try
            {
                string weatherData = await GetWeatherAsync(city);
                DisplayForecast(weatherData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task<string> GetWeatherAsync(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUrl = $"{BASE_URL}?q={city}&appid={API}&units=metric&lang=ru";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception("Не удалось получить данные о погоде. Проверьте название города и подключение к интернету.");
                }
            }
        }

        static void DisplayForecast(string forecastData)
        {
            var forecast = JsonConvert.DeserializeObject<ForecastResponse>(forecastData);

            Console.WriteLine($"\n--- Прогноз погоды в городе {forecast.city.name} ---");

            var dailyForecasts = forecast.list
                .GroupBy(item => DateTime.Parse(item.dt_txt).Date)
                .OrderBy(group => group.Key)
                .Take(5);

            foreach (var day in dailyForecasts)
            {
                Console.WriteLine($"\nДата: {day.Key:dd.MM.yyyy}");

                var avgTemp = day.Average(item => item.main.temp);
                var minTemp = day.Min(item => item.main.temp);
                var maxTemp = day.Max(item => item.main.temp);
                var mainWeather = day.GroupBy(item => item.weather[0].description)
                                   .OrderByDescending(g => g.Count())
                                   .First().Key;
                var avgHumidity = day.Average(item => item.main.humidity);
                var avgWindSpeed = day.Average(item => item.wind.speed);

                Console.WriteLine($"Погода: {mainWeather}");
                Console.WriteLine($"Температура: {avgTemp:0}°C (мин {minTemp:0}°C, макс {maxTemp:0}°C)");
                Console.WriteLine($"Влажность: {avgHumidity:0}%");
                Console.WriteLine($"Ветер: {avgWindSpeed:0.0} м/с");
            }
        }
    }
}