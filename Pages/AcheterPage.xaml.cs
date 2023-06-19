using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;

namespace Gestion_des_produits.Pages;


public partial class AcheterPage : ContentPage
{
     ConnectionSQL connection;
    

    public ObservableCollection<Ligne> ListeVente { get; set; } = new ObservableCollection<Ligne>();
    public AcheterPage()
    {
        InitializeComponent();
        BindingContext = this;
        NavigationPage.SetHasNavigationBar(this, false);

        Afficher();
    }

    //Connexion à la base de données et lecture des données
    public void Afficher()
    {
        connection = new ConnectionSQL();
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
            connection.Finish();
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

        ObservableCollection<Ligne> ListeCommand = new ObservableCollection<Ligne>();  

        float total = 0;

        foreach (Ligne item in searchResults.ItemsSource)
        {
            bool isChecked = item.Check;
            if (isChecked)
            {
                ListeCommand.Add(item);
                total += float.Parse(item.PrixHT);
            }
        }
        if (total > 0)
        {
            this.ShowPopup(new PopupPage(ListeCommand));
        }


    }


}