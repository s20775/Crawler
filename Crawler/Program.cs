using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            if (args.Length == 0)
                throw new ArgumentNullException("Brak argumentów");

            string websiteUrl = args[0];

            if (!(Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute)))
                throw new ArgumentException("Podaj poprawny adres URL");

            HttpClient httpClient = new HttpClient();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);


                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync(); 
                    
                    Regex regex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

                    MatchCollection matchCollection = regex.Matches(content);

                    HashSet<string> hashSet = new HashSet<string>();

                    foreach (Match match in matchCollection)
                        hashSet.Add(match.Value);

                    if (hashSet.Count >= 1)
                        foreach (string str in hashSet)
                            Console.WriteLine(str);
                    else
                        Console.WriteLine("Nie znaleziono adresów e-mail");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd w czasie pobierania strony: " + e.Message);
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
