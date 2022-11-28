using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorldCupApp.Interfejsy;

namespace WorldCupApp.Manager
{
    public class ManagerSerwis : ISerwis
    {
        private HttpClient klient;
        private HttpRequestMessage zapytanie;

        public ManagerSerwis()
        {
            klient = new HttpClient();
        }

        public async void PobierzWynik()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api-football-v1.p.rapidapi.com/v3/timezone"),
                Headers =
    {
        { "X-RapidAPI-Key", "a10bc38ccdmsh5812842a138837dp146d1ejsnb0baee706b4d" },
        { "X-RapidAPI-Host", "api-football-v1.p.rapidapi.com" },
    },
            };
            using (var response = await klient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
    }
}
