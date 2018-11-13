using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookLookUp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public async Task<BookDetail> BookDetailses()
        {
            var httpClient = new HttpClient();

            using (var bookStream =
                httpClient.GetStreamAsync("https://openlibrary.org/api/books?bibkeys=ISBN:0385472579&format=json"))
            using (var streamReader = new StreamReader(await bookStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                var fromStream = serializer.Deserialize<Dictionary<string, BookDetail>>(jsonReader);

                return fromStream.GetValueOrDefault("ISBN:0385472579");
            }
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get { return 32 + (int) (TemperatureC / 0.5556); }
            }
        }

        [DataContract]
        public class BookDetail
        {
            [DataMember(Name = "bib_key")] public string BibKey { get; set; }
            [DataMember(Name = "preview")] public string Preview { get; set; }
            [DataMember(Name = "thumbnail_url")] public string ThumbnailUrl { get; set; }
            [DataMember(Name = "preview_url")] public string PreviewUrl { get; set; }
            [DataMember(Name = "info_url")] public string InfoUrl { get; set; }
        }
    }
}