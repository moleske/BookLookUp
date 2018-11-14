using System.Collections.Generic;
using System.IO;
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
        [HttpGet("[action]")]
        public async Task<BookDetail> BookDetailses(string isbn)
        {
            var httpClient = new HttpClient();

            using (var bookStream =
                httpClient.GetStreamAsync($"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json"))
            using (var streamReader = new StreamReader(await bookStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                var fromStream = serializer.Deserialize<Dictionary<string, BookDetail>>(jsonReader);

                return fromStream.GetValueOrDefault($"ISBN:{isbn}");
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