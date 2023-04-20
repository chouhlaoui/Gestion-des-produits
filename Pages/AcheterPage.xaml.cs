using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.ObjectModel;
namespace Gestion_des_produits.Pages;


public partial class AcheterPage : ContentPage
{
    readonly ConnectionSQL connection = new ConnectionSQL();
    public class Ligne
    {
        public string NomProduit { get; set; }
        public string PrixHT { get; set; }
        public string Quantite { get; set; }
        public bool Check { get; set; }


    }

    public ObservableCollection<Ligne> ListeVente { get; set; } = new ObservableCollection<Ligne>();
    public AcheterPage()
    {
        InitializeComponent();
        BindingContext = this;
        Afficher();
    }

    //Connexion à la base de données et lecture des données
    public void Afficher()
    {
        try
        {
            string query = "SELECT * FROM produit";
            MySqlDataReader reader = connection.ExecuteQuery(query);

            while (reader.Read())
            {
                ListeVente.Add(new Ligne
                {
                    NomProduit = reader.GetString(1),
                    PrixHT = reader.GetString(3),
                    Quantite = reader.GetString(4)
                });
            }
            reader.Close();
        }
        catch (MySqlException)
        {
            Console.WriteLine("Error: Unable to connect to the database.");
        }

    }

    private void SearchBar_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        // Filter the list based on the search text
        if (string.IsNullOrEmpty(searchText))
        {
            searchResults.ItemsSource = ListeVente;
        }
        else
        {
            searchResults.ItemsSource = ListeVente.Where(item => item.NomProduit.ToLower().Contains(searchText.ToLower()));
        }
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        ListeVente.Clear();
        searchbar.Text = "";
        searchbar.Placeholder = "Effectuer une recherche avec le nom du produit";
        Afficher();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        float total = 0;
        string lesachats = "";

        foreach (Ligne item in searchResults.ItemsSource)
        {
            bool isChecked = item.Check;
            if (isChecked)
            {
                total += float.Parse(item.PrixHT);
                lesachats = lesachats + item.NomProduit + "\n";
            }
        }
        if (total > 0)
        {
            string query = "INSERT INTO commande(total,listeDesArticles,date) VALUES(" + total + ",\"" + lesachats + "\",\"" + DateTime.Today.ToString("yyyy-MM-dd") + "\");";
            connection.ExecuteNonQuery(query);
        }


    }


}