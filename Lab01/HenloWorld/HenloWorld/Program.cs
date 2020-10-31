using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HenloWorld
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if(args.Length < 1)
            {
                throw new ArgumentNullException("args", "no args passed to the program");
            }
            if(!IsValidURL(args[0]))
            {
                throw new ArgumentException("Twój argument jest inwalidą", "args[0]");
            }
            var client = new HttpClient();
            var response = await client.GetAsync(args[0]);

            if(response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                var re = new Regex("[a-z0-9]+@[a-z.]+");

                MatchCollection matches = re.Matches(html);
                if(matches.Count == 0)
                {
                    Console.WriteLine("No e-mail address found");
                }
                foreach(var match in matches.OfType<Match>().Select(m => m.Value).Distinct())
                {
                    Console.WriteLine(match);
                }
            }
            else
            {
                Console.WriteLine("An error occured while trying to download data");
                Console.WriteLine(response.StatusCode);
            }
            client.Dispose();
        }

        public static bool IsValidURL(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
