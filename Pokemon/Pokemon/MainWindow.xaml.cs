using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Pokemon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PokemonAttributes poke;
        public MainWindow()
        {
            InitializeComponent();

            PokemonList poke = new PokemonList();

            using (var client = new HttpClient())
            {
                HttpResponseMessage result = client.GetAsync("https://pokeapi.co/api/v2/pokemon?offset=0&limit=1200").Result;

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string json = result.Content.ReadAsStringAsync().Result;


                    poke = JsonConvert.DeserializeObject<PokemonList>(json);
                    foreach (var p in poke.results)
                    {
                        PokemonCBO.Items.Add(p);
                    }
                }
                else
                {
                     MessageBox.Show($"ERROR:{result.StatusCode}");
                }
            }
        }

        private void PokemonCBO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Results selected = (Results)PokemonCBO.SelectedItem;

            using (var client = new HttpClient())
            {
                var pic = client.GetStringAsync(selected.url).Result;

                PokemonAttributes poke = JsonConvert.DeserializeObject<PokemonAttributes>(pic);

                Image.Source = new BitmapImage(new Uri(poke.sprites.front_default));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Image.Source = new BitmapImage(new Uri(poke.sprites.back_default));
        }
    }
    
}
