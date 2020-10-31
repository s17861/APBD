using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HenloWorld
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(args[0]);

            if(response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                var re = new Regex("[a-z0-9]+@[a-z.]+");

                MatchCollection matches = re.Matches(html);
                foreach(var match in matches)
                {
                    Console.WriteLine(match);
                }
            }
        }
    }
}
