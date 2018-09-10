using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace WpfAppHoliday.Logic
{
    class ClassHttpRequest
    {
        public string CreateHttpGet(string requestUriString)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateHttpGetAsync(string requestUriString)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateHttpGetWebClient(string requestUriString)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string responseString = client.DownloadString(requestUriString);
                    return responseString;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateHttpGetWebClientAsync(string requestUriString)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    /*client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
                        (sender, e) =>
                        {
                            responseString = e.Result;
                        }
                    );
                    client.DownloadStringAsync(new Uri(requestUriString));*/
                    string responseString = await client.DownloadStringTaskAsync(requestUriString);
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateHttpGetHttpClientAsync(string requestUriString)
        {
            try
            {
                //client.BaseAddress = new Uri(requestUriString);response.EnsureSuccessStatusCode();
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(requestUriString))
                using (HttpContent content = response.Content)
                {
                    string responseString = await content.ReadAsStringAsync();
                    return responseString;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateUriString(string portal, string countries, string date1, string date2, string vehicles, string food, string airports, string count, int numberPage)
        {
            StringBuilder sb = new StringBuilder(portal);
            if (portal.Equals("https://www.wakacje.pl"))
            {
                sb.Append("/wczasy/");
                sb.Append(countries).Replace("ł", "l").Append("/?");
                if (numberPage > 1)
                    sb.Append("str-").Append(numberPage).Append(",");
                sb.Append("od-");
                sb.Append(date1).Append(",do-");
                sb.Append(date2).Append(",").Append(vehicles).Append(",");
                sb.Append(food).Append(",");
                foreach (string s in airports.Split(','))
                    sb.Append("z-").Append(s).Append(",");
                sb.Append("tanio,");
                if (!count.Equals("2"))
                    sb.Append(count).Append("dorosle");
            }
            return sb.ToString().TrimEnd(',');
        }

        public string GetDomainName(string UriString)
        {
            Uri myUri = new Uri(UriString);
            string host = myUri.Host;
            return host;
        }

    }
}
