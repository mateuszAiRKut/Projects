using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WpfAppHoliday.Logic
{
    class ClassHtmlAgilityPack
    {
        private HtmlDocument documentHTML;

        public ClassHtmlAgilityPack()
        {
            documentHTML = new HtmlDocument();
        }

        public void LoadDocumentHTML(ref string stringHtml)
        {
            documentHTML.LoadHtml(stringHtml);
        }

        public void LoadDocumentHTMLWeb(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            documentHTML = htmlWeb.Load(url);
        }

        public async Task LoadDocumentHTMLWebAsync(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            documentHTML = await htmlWeb.LoadFromWebAsync(url);
            return;
        }

        public Task<HashSet<ClassOffert>> SearchElementsTask(Dictionary<string, string> elementsAttributes)
        {
            HashSet<ClassOffert> listOfferts = null;
            Func<HashSet<ClassOffert>> action = new Func<HashSet<ClassOffert>>(
                () =>
                {
                    listOfferts = SearchElements(elementsAttributes);
                    return listOfferts;
                });
            Task<HashSet<ClassOffert>> task = new Task<HashSet<ClassOffert>>(action);
            task.Start();
            return task;
        }

        public HashSet<ClassOffert> SearchElements(Dictionary<string, string> elementsAttributes)
        {
            string key = elementsAttributes.Keys.First(), value = elementsAttributes.Values.First();
            string attr = value.Substring(0, value.IndexOf(":")),
                attrVal = value.Substring(value.IndexOf(":") + 1, value.Length - value.IndexOf(":") - 1);
            string[] attrVals = attrVal.Split(',');
            HashSet<HtmlNode> listNodes = new HashSet<HtmlNode>();
            HashSet<ClassOffert> listaOfferts = new HashSet<ClassOffert>();
            foreach (string val in attrVals)
            {
                StringBuilder query = new StringBuilder("//");
                query.Append(key).Append("[@").Append(attr).Append("='").Append(val).Append("']");
                //StringBuilder query = new StringBuilder("//");
                //query.Append(pair.Key).Append("[contains(@").Append(attr).Append(",'").Append(val).Append("')]");
                IEnumerable<HtmlNode> col = documentHTML.DocumentNode.SelectNodes(query.ToString());
                //IEnumerable<HtmlNode> col = documentHTML.DocumentNode.SelectNodes("//"+pair.Key).Where(d => d.Attributes[attr] != null && d.Attributes[attr].Value.Contains(val));
                if (col != null)
                    listNodes.UnionWith(col);
            }

            foreach (HtmlNode node in listNodes)
            {
                StringBuilder sbOffert = new StringBuilder();
                foreach (KeyValuePair<string, string> pair in elementsAttributes.Where(pair => pair.Key != key))
                {
                    key = pair.Key;
                    value = pair.Value;
                    attr = value.Substring(0, value.IndexOf(":"));
                    attrVal = value.Substring(value.IndexOf(":") + 1, value.Length - value.IndexOf(":") - 1);
                    attrVals = attrVal.Split(',');
                    foreach (string val in attrVals)
                    {
                        StringBuilder query = new StringBuilder(".//");
                        query.Append(key).Append("[@").Append(attr).Append("='").Append(val).Append("']");
                        HtmlNode nodeIn = node.SelectNodes(query.ToString())?.First();
                        if (nodeIn != null)
                        {
                            if (nodeIn.Name.Equals("a"))
                               sbOffert.Append(nodeIn.Attributes["href"]?.Value);
                            else
                               sbOffert.Append(nodeIn.InnerText.Replace("&nbsp;", " ")).Append("#");
                        }
                    }

                }
                listaOfferts.Add(new ClassOffert(sbOffert.ToString()));
            }
            return listaOfferts;
        }
    }

    class ClassOffert : IComparable<ClassOffert>
    {
        private string[] array;

        public static int CalkowitaLicza { get; set; }
        public int Pozycja { get; set; }
        public string Cena { get; set; }
        public string Apartament { get; set; }
        public string MiejscePobytu { get; set; }
        public string CzasPobytu { get; set; }
        public string Link { get; set; }

        public string this[string index]
        {
            set
            {
                switch (index)
                {
                    case "a":
                        Link = value;
                        break;
                    case "b":
                        Cena = value;
                        break;
                    case "span:name":
                        Apartament = value;
                        break;
                    case "span:country":
                        MiejscePobytu = value;
                        break;
                    case "span:dur":
                        CzasPobytu = value;
                        break;
                }
            }
        }

        public ClassOffert(string s)
        {
            string[] arrayValues = s.Split('#');
            Pozycja = ++CalkowitaLicza;
            array = new string[5];
            if (arrayValues.Length == 5)
            {
                Cena = arrayValues[0];
                Apartament = arrayValues[1];
                MiejscePobytu = arrayValues[2];
                CzasPobytu = arrayValues[3];
                Link = arrayValues[4];
            }
        }

        public int getNumber(string s)
        {
            int number = 0;
            /*for(int i=s.Length-1, pow = 0; i>=0; i--)
            {
                if(Char.IsDigit(s[i]))
                    number+=(int)Math.Pow(10,pow++)*(int)Char.GetNumericValue(s[i]);
            }
            string result="";
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsDigit(s[i]))
                    result += s[i];
            }
            number = int.Parse(result);*/
            //number = int.Parse(System.Text.RegularExpressions.Regex.Match(s, @"\d+").Value);
            //number = int.Parse(string.Join("", s.ToCharArray().Where(Char.IsDigit)));
            number = int.Parse(new String(s.Where(Char.IsDigit).ToArray()));
            return number;
        }

        public int CompareTo(ClassOffert other)
        {
            int price1 = 0, price2 = 0;
            try
            {
                price1 = int.Parse(other.Cena.Replace("zł", "").Replace(" ", ""));
                price2 = int.Parse(Cena.Replace("zł", "").Replace(" ", ""));
            }catch
            {
                price1 = getNumber(other.Cena);
                price2 = getNumber(other.Cena);
            }
            return price2.CompareTo(price1);
        }

        public override string ToString()
        {
            return string.Join(" ", new string[] {Pozycja.ToString(), MiejscePobytu, Apartament, Cena, CzasPobytu, Link });
        }
    }
}



