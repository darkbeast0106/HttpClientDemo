using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo
{
    class Program
    {
        static string url = "http://localhost/_oktatas/14s/2021_10_14/?vegpont=";
        
        static void Main(string[] args)
        {

            Console.WriteLine("Listázás előtte: ");
            ListAsync().GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Hozzáadás: ");
            AddAsync().GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Listázás utána: ");
            ListAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task AddAsync()
        {
            using (var client = new HttpClient())
            {
                var data = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("gyarto", "Ford"),
                    new KeyValuePair<string, string>("modell", "Focus"),
                    new KeyValuePair<string, string>("uzembehelyezes", "2011")
                });
                var result = await client.PostAsync(url + "felvesz", data);
                // https://stackoverflow.com/questions/36625881/how-do-i-pass-an-object-to-httpclient-postasync-and-serialize-as-a-json-body/52857689 
                string content = result.Content.ReadAsStringAsync().Result;

                dynamic parsedJson = JsonConvert.DeserializeObject(content);
                Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));
            }
        }

        private static async Task ListAsync()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(url + "listaz");

                dynamic parsedJson = JsonConvert.DeserializeObject(content); // jsonből adatot csinál
                Console.WriteLine( JsonConvert.SerializeObject(parsedJson, Formatting.Indented)); // adatból json-t csinál + formázást hozzárak ha kell.
                // http://www.newtonsoft.com/json/help/html/SerializingJSON.htm
            }
        }
    }
}
