using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppHoliday.UserControls;
using WpfAppHoliday.Logic;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace WpfAppHoliday
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private List<string> country, airports, vehicles, food;
        private Dictionary<string, Dictionary<string, string>> portalsSearch;
        private HashSet<ClassOffert> listOfferts;
        private ClassHttpRequest classHttp;
        private ClassHtmlAgilityPack classHtmlAgilityPack;
        private ClassSendMail classSendMail;
        private ClassTextFiles classTextFile;
        private int numberSearchOfferts, numberPage;
        private bool error;
        private StringBuilder createBodyMail;
        private DispatcherTimer timer;
        //private ClassDispatcherTimerExt timerExt;

        private void SortListOfferts<T>(HashSet<T> set)
        {
            T[] arrO = set.ToArray();
            Array.Sort(arrO);
            set.Clear();
            set.UnionWith(arrO);
        }

        public MainWindow()
        {
            InitializeComponent();
            country = new List<string>() { "Grecja", "Hiszpania", "Włochy", "Cypr", "Portugalia" };
            airports = new List<string>() { "Katowic", "Lodzi", "Wroclawia", "Warszawy" };
            vehicles = new List<string>() { "Samolotem", "Autokarem", "Wlasny" };
            food = new List<string>() { "all-inclusive", "HB", "BB", "wlasne" };

            comboCountry.ItemsSource = country;
            comboAirports.ItemsSource = airports;
            comboVehicles.ItemsSource = vehicles;
            comboFood.ItemsSource = food;
            //comboCountry.SelectedItems = selectedCountry;
          
            classHttp = new ClassHttpRequest();
            classHtmlAgilityPack = new ClassHtmlAgilityPack();
            //classSendMail = new ClassSendMail();
            listOfferts = new HashSet<ClassOffert>();
            createBodyMail = new StringBuilder();
            classTextFile = new ClassTextFiles();
            portalsSearch = new Dictionary<string, Dictionary<string, string>>()
            {
                {"https://www.wakacje.pl", new Dictionary<string, string>()
                    {
                        {"div", "class:offer clearfix,offer clearfix promoOffer" },
                        {"b","class:price" },
                        {"span","class:name,country,dur" },
                        {"a","class:imgCont wt" }
                    }
                }
            };

            initializeSampleData();
            initializeTabControlOfferts();
           
            /*timerExt = new ClassDispatcherTimerExt();
            timerExt.IsReentrant = false;
            timerExt.Interval = TimeSpan.FromMinutes(1);
            timerExt.TickTask = async () => { await searchOffertsAsync(); };
            timerExt.Start();*/
            //Closing += (s, e) => MessageBox.Show("Aplikacja nadal działa w tle");
        }

        private void initializeSampleData()
        {
            comboCountry.SelectedItems = new List<string>() { "Grecja", "Hiszpania" };
            comboAirports.SelectedItems = new List<string>() { "Wroclawia", "Warszawy" };
            comboVehicles.SelectedItems = new List<string>() { "Samolotem", "Autokarem", };
            comboFood.SelectedItems = new List<string>() { "all-inclusive", "HB", };
            time1.SelectedDate = DateTime.Today.AddDays(1);
            time2.SelectedDate = DateTime.Today.AddDays(8);
        }

        private void addListOffertsToTabControl(string portal, HashSet<ClassOffert> listOfferts)
        {
            string s = classHttp.GetDomainName(portal);
            TabItem tabItem = tabControlOfferts.Items.WhereExt((i) => i.Header.ToString().Equals(s));
            Grid grid = tabItem.Content as Grid;
            createBodyMail.Clear();
            if (grid != null)
            {
                TabControl tabControl = grid.Children[0] as TabControl;
                if (tabControl != null)
                {
                    foreach (TabItem tabItem2 in tabControl.Items)
                    {
                        ListView listV = createListView(tabItem2.Header.ToString());
                        createBodyMail.Append(tabItem2.Header.ToString()).Append(":").Append(Environment.NewLine);
                        listV.PreviewMouseDoubleClick += (sender, e) =>
                        {
                            var item = (sender as ListView).SelectedItem;
                            if (item != null)
                            {
                                ClassOffert o = (item as ClassOffert);
                                if (o != null)
                                    System.Diagnostics.Process.Start(o.Link);
                            }
                        };
                        var l = listOfferts.Where((offert)=>offert.MiejscePobytu.ToLower().Contains(tabItem2.Header.ToString().ToLower()));
                        for (int i = 0; i < l.Count(); i++)
                        {
                            l.ElementAt(i).Pozycja = (i + 1);
                            if(!l.ElementAt(i).Link.Contains(portal))
                                l.ElementAt(i).Link = portal + l.ElementAt(i).Link;
                            createBodyMail.Append(l.ElementAt(i)).Append(Environment.NewLine);
                        }
                        listV.ItemsSource = l;
                        Grid grid2 = tabItem2.Content as Grid;
                        grid2.Children.Add(listV);
                    }
                }
            }
        }

        private void ListV_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void checkTabControl()
        {
            foreach(TabItem tabItem in tabControlOfferts.Items)
            {
                Grid grid = tabItem.Content as Grid;
                if(grid  != null)
                {
                    TabControl tabControl = grid.Children[0] as TabControl;
                    if(tabControl != null)
                    {
                        HashSet<string> setCountry = new HashSet<string>(getInformationsCountryAirportsVehiclesFood(comboCountry).Split(','));
                        foreach(TabItem tabItem2 in tabControl.Items.ReverseExt())
                        {
                            Grid grid2 = tabItem2.Content as Grid;
                            if (grid2 != null)
                                grid2.Children.Clear();
                            string tabItemH = tabItem2.Header.ToString().ToLower();
                            bool remove = true;
                            foreach(string s in setCountry)
                            {
                                if (tabItemH.Equals(s))
                                {
                                    remove = false;
                                    setCountry.Remove(s);
                                    break;
                                }
                            }
                            if (remove)
                                tabControl.Items.Remove(tabItem2);
                        }
                        /*for (int index = 0; index < tabControl.Items.Count;)
                        {
                            string tabItemH = ((TabItem)tabControl.Items[index])?.Header.ToString().ToLower();
                            bool remove = true;                          
                            foreach (string s in setCountry)
                            {
                                if (tabItemH.Equals(s))
                                {
                                    remove = false;
                                    setCountry.Remove(s);
                                    break;
                                }
                            }
                            if (remove)
                                tabControl.Items.Remove(tabControl.Items[index]);
                            else
                                index++;
                        }*/
                        if (setCountry.Count != 0)
                        {
                            foreach (string country in setCountry)
                            {
                                TabItem tab = new TabItem();
                                tab.Header = country.ToUpper();
                                tab.Content = new Grid() { Background = Brushes.LightGray };
                                tabControl.Items.Add(tab);
                            }
                        }
                    }
                }
            }
        }

        private void initializeTabControlOfferts()
        {
            foreach (string s in portalsSearch.Keys)
            {
                TabItem tabItem = new TabItem(), tabItem2 = null;
                tabItem.Header = classHttp.GetDomainName(s);
                Grid grid = new Grid() { Background = Brushes.LightGray };
                TabControl tabControl = new TabControl();
                foreach(string country in getInformationsCountryAirportsVehiclesFood(comboCountry).Split(','))
                {
                    tabItem2 = new TabItem();
                    tabItem2.Header = country.ToUpper();
                    tabItem2.Content = new Grid() { Background = Brushes.LightGray };
                    tabControl.Items.Add(tabItem2);
                }
                grid.Children.Add(tabControl);
                tabItem.Content = grid;
                tabControlOfferts.Items.Add(tabItem);
            }
        }

        private void addColumn(GridView gridView, string path)
        {
            gridView.Columns.Add(
                new GridViewColumn
                {
                    
                    DisplayMemberBinding = new Binding(path),
                    Header = string.Concat(path.Select((c)=> char.IsUpper(c) ? " "+c:c.ToString())).TrimStart(' '),
                    Width = double.NaN
                });
        }

        private ListView createListView(string country)
        {
            ListView listView = new ListView();
            listView.Name = "ListOfferts" + country;

            GridView myGridView = new GridView();
            myGridView.AllowsColumnReorder = true;
            myGridView.ColumnHeaderToolTip = "Wakacyjne oferty";
            addColumn(myGridView, "Pozycja");
            addColumn(myGridView, "MiejscePobytu");
            addColumn(myGridView, "Apartament");
            addColumn(myGridView, "Cena");
            addColumn(myGridView, "CzasPobytu");
            addColumn(myGridView, "Link");

            listView.View = myGridView;
            return listView;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            TextBox textNumberOfferts = sender as TextBox;
            e.Handled = (textNumberOfferts.Text.Length == 0 && e.Text[0] == '0' || textNumberOfferts.Text.Length == 3) ? true : regex.IsMatch(e.Text);
        }

        private string getInformationsCountryAirportsVehiclesFood(UserControlComboBox comboBox)
        {
            string information = null;
            if (comboBox.Text != "Wszystkie")
            {
                if (comboBox.Text.Length != 0)
                {
                    if(comboBox.Name.Equals(comboFood.Name))
                        information = comboBox.Text.Replace(" ", "");
                    else
                        information = comboBox.Text.ToLower().Replace(" ", "");
                }
                else
                    MessageBox.Show("Wybierz opcje");
            }
            else
            {
                StringBuilder newS = new StringBuilder();
                bool f = comboBox.Name.Equals(comboFood.Name);
                foreach (string s in comboCountry.SelectedItems)
                {
                    if (f)
                        newS.Append(s).Append(',');
                    else
                        newS.Append(s.ToLower()).Append(',');
                }
                newS.Remove(newS.Length - 1, 1);
                information = newS.ToString();
            }
            return information;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioB = sender as RadioButton;
            if(radioB != null)
            {
                TextBlock text = radioB.Content as TextBlock;
                if (text != null)
                {
                    setApp(radioB, text);
                }
            }
            
        }

        private void setApp(RadioButton radrioB, TextBlock text)
        {
            if(radrioB.GroupName.Equals("startApp"))
            {
                if(text.Text.Equals("Automatycznie"))
                {
                    if (timer == null)
                    {
                        timer = new DispatcherTimer();
                        int time = int.Parse(((ComboBoxItem)comboTime.SelectedItem).Content.ToString());
                        timer.Interval = TimeSpan.FromMinutes(time);
                        timer.Tick += async (s, e) => { await searchOffertsAsync(); };
                        timer.Start();
                    }
                }
                else
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer = null;
                    }
                }
            }
        }

        private string getInformationsDataTime(DatePicker dataPicker)
        {
            DateTime? selectedDate = dataPicker.SelectedDate;
            string formattedData = null;
            if (selectedDate.HasValue)
                formattedData = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            else
                MessageBox.Show("Wybierz date");
            return formattedData;
        }

        private async Task searchOffertsAsync()
        {
            //string s = classHttp.CreateHttpGet(uriString);
            //string s = classHttp.CreateHttpGetWebClient(uriString);
            //string s = await classHttp.CreateHttpGetWebClientAsync(uriString);
            //string s = await classHttp.CreateHttpGetHttpClientAsync(uriString);
            try
            {
                numberSearchOfferts = int.Parse(textNumberOfferts.Text);
                numberPage = 1;
                checkTabControl();
                textInfo.Foreground = textInfoMail.Foreground = Brushes.Green;
                textInfo.Text = "Wyszukiwanie ofert";
                textInfoMail.Text = "";
                string stringHTML, portal = portalsSearch.Keys.First();
                HashSet<ClassOffert> list = null;
                listOfferts.Clear();
                createBodyMail.Clear();
                ClassOffert.CalkowitaLicza = 0;
                if (portalsSearch.Count == 1)
                {
                    do
                    {
                        string uriString = classHttp.CreateUriString(portal, getInformationsCountryAirportsVehiclesFood(comboCountry)
                           , getInformationsDataTime(time1), getInformationsDataTime(time2), getInformationsCountryAirportsVehiclesFood(comboVehicles), getInformationsCountryAirportsVehiclesFood(comboFood),
                           getInformationsCountryAirportsVehiclesFood(comboAirports), ((ComboBoxItem)comboPeople.SelectedItem).Content.ToString(), numberPage);
                        //MessageBox.Show(uriString);
                        stringHTML = await classHttp.CreateHttpGetAsync(uriString);
                        classHtmlAgilityPack.LoadDocumentHTML(ref stringHTML);
                        //list = classHtmlAgilityPack.SearchElements(portalsSearch[portal]);                      
                        list = await classHtmlAgilityPack.SearchElementsTask(portalsSearch[portal]);
                        if (list.Count == 0)
                        {
                            error = true;
                            break;
                        }
                        listOfferts.UnionWith(list);
                        SortListOfferts(listOfferts);
                        addListOffertsToTabControl(portal, listOfferts);
                        numberPage++;
                    } while ((numberSearchOfferts -= list.Count) > 0);
                }
                else
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> pairs in portalsSearch)
                    {
                        string uriString = classHttp.CreateUriString(pairs.Key, getInformationsCountryAirportsVehiclesFood(comboCountry)
                            , getInformationsDataTime(time1), getInformationsDataTime(time2), getInformationsCountryAirportsVehiclesFood(comboVehicles), getInformationsCountryAirportsVehiclesFood(comboFood),
                            getInformationsCountryAirportsVehiclesFood(comboAirports), ((ComboBoxItem)comboPeople.SelectedItem).Content.ToString(), numberPage);
                        stringHTML = await classHttp.CreateHttpGetAsync(uriString);
                        classHtmlAgilityPack.LoadDocumentHTML(ref stringHTML);
                        classHtmlAgilityPack.SearchElements(pairs.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                textInfo.Foreground = Brushes.Red;
                textInfo.Text = ex.Message;
                return;
            }

            if (error && listOfferts.Count == 0)
            {
                textInfo.Foreground = Brushes.Red;
                textInfo.Text = "Niestety nie pobrano zadnej oferty";
            }
            else
            {
                if (!error)
                    textInfo.Text = "Sukces pobrano wszystkie oferty";
                else if (listOfferts.Count != 0 && numberSearchOfferts > 0)
                {
                    textInfo.Foreground = Brushes.Orange;
                    textInfo.Text = "Niestety tylko tyle ofert znaleziono";
                }

                if (checkMail.IsChecked.HasValue && checkMail.IsChecked.Value)
                {
                    textInfoMail.Text = "Wysylam Mail";
                    try
                    {
                        classTextFile.CreateFile(createBodyMail.ToString(), "oferty");
                        classSendMail.PathFile = classTextFile.PathFile;
                        await classSendMail.SendMailTask("", subject: "Wakacyjne oferty", body: createBodyMail.ToString(), attach: true);
                    }
                    catch (Exception ex)
                    {
                        textInfoMail.Foreground = Brushes.Red;
                        textInfoMail.Text = ex.Message;
                        return;
                    }
                    textInfoMail.Text = "Mail zostal wyslany";
                }
            }
        }

        private async void UserControlButton_Click(object sender, RoutedEventArgs e)
        {
            await searchOffertsAsync();
        }

        private void correctCombineCountry()
        {
            string[] vals = getInformationsCountryAirportsVehiclesFood(comboCountry).Split(',');
            foreach (string[] v in ClassPermutations.Permutations(vals))
                MessageBox.Show(string.Join(",", v));
        }
    }
}
