using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recipes
{
    public partial class MainWindow : Window
    {
        private int desired_id = 0;

        /**
         * Method managing the action of the "Previous" button
         * -> Browse the list backwards to find the last recipes viewed
         * -> Refresh the screen at each step
         * 
         */
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if(this.desired_id > 0)
            {
                this.desired_id--;
            }

            this.Refresh(this.desired_id+1);
        }

        /**
         * Method managing the action of the "Next" button
         * -> Browse the list forward to find most recently added recipes
         * -> If it reached the end of the list, request for a new element to add
         * 
         */
        private void Next_Click(object sender, RoutedEventArgs e)
        {

            if(this.desired_id != ManageAPI.recipesList.Count - 1)
            {
                this.desired_id++;
            }

            this.Refresh(this.desired_id+1);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ManageAPI.Request();

            this.desired_id = ManageAPI.recipesList.Count - 1;
            this.Refresh(ManageAPI.recipesList.Count);
        }

        /**
         * When called, it refreshed the content of the main window
         * 
         */
        private void Refresh(int i)
        {
            if ( ManageAPI.recipesList != null)
            {
                icIngredients.ItemsSource = ManageAPI.recipesList[this.desired_id].Ingredients;
                Healthscore.DataContext = ManageAPI.recipesList[this.desired_id];
                Vegetarian.IsChecked = ManageAPI.recipesList[this.desired_id].Vegetarian;
                Vegan.IsChecked = ManageAPI.recipesList[this.desired_id].Vegan;
                GlutenFree.IsChecked = ManageAPI.recipesList[this.desired_id].Glutenfree;
                Cheap.IsChecked = ManageAPI.recipesList[this.desired_id].Cheap;
                VeryPopular.IsChecked = ManageAPI.recipesList[this.desired_id].Verypopular;
                Instructions.NavigateToString(ManageAPI.recipesList[this.desired_id].Instructions);
                Name.DataContext = ManageAPI.recipesList[this.desired_id];
                Source.DataContext = ManageAPI.recipesList[this.desired_id];
                cookingTime.Content = "Temps de préparation : " + ManageAPI.recipesList[this.desired_id].ReadyInMinutes;
                page.Content = i.ToString() + " / " + ManageAPI.recipesList.Count;
            }
            
        }
        public MainWindow()
        {
            InitializeComponent();

            ManageAPI.Request();

            this.Refresh(ManageAPI.recipesList.Count);
        }
    }
}

    public static class ManageAPI
    {
        public static List<Recipe> recipesList { get; set; } = new List<Recipe>();

        /**
         * Request the spoonacular API to get a random recipe
         * -> It adds a random recipe to the list recipesList
         * -> It also sets the new desired_id to the end of the list (focus on the new recipe)
         * 
         */
        public static void Request()
        {
            Console.WriteLine("Fetching ...");

            // Requesting the spoonacular API for a random recipe
            HttpWebRequest request = HttpWebRequest.CreateHttp("https://api.spoonacular.com/recipes/random?number=1&apiKey=9bd703da55f9485dafa1a28c264c5d72 ");

            // Setting TimeOut to 1 minute
            request.Timeout = 60 * 1000;

            // Getting JSON Response from the API
            string responseBodyFromRemoteServer;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    Console.WriteLine("Successfuly feteched ! ");
                    responseBodyFromRemoteServer = reader.ReadToEnd();
                }
            }
            JObject json = JObject.Parse(responseBodyFromRemoteServer);

            int i = 0;
            string s = "";
            List<Ingredient> ingredients = new List<Ingredient>();

            while (s != null)
            {
                s = (string)json.SelectToken("recipes[0].extendedIngredients[" + i.ToString() + "].name");
                if(s != null)
                {
                    ingredients.Add(
                        new Ingredient()
                        {
                            Title = s,
                            Quantity = (string)json.SelectToken("recipes[0].extendedIngredients[" + i.ToString() + "].measures.metric.amount") + " " + (string)json.SelectToken("recipes[0].extendedIngredients[" + i.ToString() + "].measures.metric.unitLong")
                        }
                    );
                    i++;
                }
            }

            // Parsing only desired fields and adding the new recipe to the list recipesList
            recipesList.Add(
                new Recipe()
                {
                    Ingredients = ingredients,
                    HealthScore = (int)json.SelectToken("recipes[0].healthScore"),
                    Vegetarian = (bool)json.SelectToken("recipes[0].vegetarian"),
                    Vegan = (bool)json.SelectToken("recipes[0].vegan"),
                    Glutenfree = (bool)json.SelectToken("recipes[0].glutenFree"),
                    Cheap = (bool)json.SelectToken("recipes[0].cheap"),
                    Verypopular = (bool)json.SelectToken("recipes[0].veryPopular"),
                    Instructions = (string)json.SelectToken("recipes[0].instructions"),
                    ReadyInMinutes = (int)json.SelectToken("recipes[0].readyInMinutes"),
                    SourceUrl = (string)json.SelectToken("recipes[0].sourceUrl"),
                    Title = (string)json.SelectToken("recipes[0].title")
                }
            );
        }
}

    public class Ingredient
    {
        public string Title { get; set; }
        public string Quantity { get; set; }
    }

    /**
        * Recipe Class
        * Contain some of the fields of the response JSON from spoonacular API
    */
    
    public class Recipe
    {
        public List<Ingredient> Ingredients { get; set; } = null;
        public int HealthScore { get; set; } = -1;
        public bool Vegetarian { get; set; } = false;
        public bool Vegan { get; set; } = false;
        public bool Glutenfree { get; set; } = false;
        public bool Cheap { get; set; } = false;
        public bool Verypopular { get; set; } = false;
        public string Instructions { get; set; } = "";
        public int ReadyInMinutes { get; set; } = -1;
        public string SourceUrl { get; set; } = "No URL provided";
        public string Title { get; set; } = "No title provided";
    }