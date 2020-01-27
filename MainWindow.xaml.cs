using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
//using Json.NET;

namespace Constellations
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 


    class VM : INotifyPropertyChanged
    {
        public VM()
        {
            myValue = "hello";
            myWallpaper = "https://wallpapercave.com/wp/wp2647152.jpg";
            //  https://images4.alphacoders.com/272/272488.jpg
            //  https://images5.alphacoders.com/413/thumb-1920-413076.jpg
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string myValue;
        public string MyValue
        {
            get { return myValue; }
            set
            {
                myValue = value;
                OnPropertyChanged("MyValue");
            }
        }
        private string myWallpaper;
        public string MyWallpaper
        {
            get { return myWallpaper; }
            set
            {
                myWallpaper = value;
                OnPropertyChanged("MyWallpaper");
            }
        }
    }
    class BackGround : INotifyPropertyChanged
    {
        public BackGround()
        {
            myWallpaper = "https://wallpapercave.com/wp/wp2647152.jpg";
        }
        public BackGround(string image_url)
        {
            myWallpaper = image_url;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string myWallpaper;
        public string MyWallpaper
        {
            get { return myWallpaper; }
            set
            {
                myWallpaper = value;
                OnPropertyChanged("MyWallpaper");
            }
        }
    }

    public partial class MainWindow : Window
    {
        //static string pathtest = "https://www.wikidata.org/wiki/Q3427";
        private static string url = "https://query.wikidata.org/sparql?query=SELECT%20%3F_toile%20%3F_toileLabel%20WHERE%20%7B%0A%20%20SERVICE%20wikibase%3Alabel%20%7B%20bd%3AserviceParam%20wikibase%3Alanguage%20%22%5BAUTO_LANGUAGE%5D%2Cen%22.%20%7D%0A%20%20%3F_toile%20wdt%3AP31%20wd%3AQ523.%0A%7D%0ALIMIT%20100";
        private static readonly HttpClient client = new HttpClient();

        private static async Task Main()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            //JObject o;
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                string responseBody = await client.GetStringAsync(url);
                /*
                o = JObject.Parse(responseBody);
                Debug.WriteLine("firstname:" + o["id"][0]);
                //*/
                Console.WriteLine(responseBody);
                //return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private static async Task<string> Getresult(string url1)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url1);
                response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                string responseBody = await client.GetStringAsync(url1);
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new VM();
            //this.DataContext = new BackGround();
            //txtbox1.TextWrapping = TextWrapping.Wrap;
            

            List<List<string>> etoiles = new List<List<string>>();
            //byte[] response = client.GetByteArrayAsync(url);
            //Console.WriteLine(Main());
            //Main();
        }
    }
}
