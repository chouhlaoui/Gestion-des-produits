using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using CommunityToolkit.Maui.Views;
using MySql.Data.MySqlClient;
using Mysqlx.Connection;

namespace Gestion_des_produits.Pages;


//Definition d'une ligne de la liste des produits 


public partial class ListeProduits : ContentPage
{
    public MyItem selected;
    ConnectionSQL connection = new ConnectionSQL();
    public ObservableCollection<MyItem> MyItems { get; set; } = new ObservableCollection<MyItem>();

    public class MyItem
    {
        public string Code { get; set; }
        public string NomProduit { get; set; }
        public string Delai { get; set; }
        public string PrixHT { get; set; }
        public string Quantite { get; set; }

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
                MyItems.Add(new MyItem
                {
                    Code = reader.GetString(0),
                    NomProduit = reader.GetString(1),
                    Delai = reader.GetDateTime(2).ToString("dd/MM/yyyy"),
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


    //Définition de la liste 
    public ListeProduits()
    {
        InitializeComponent();
        BindingContext = this;
        NavigationPage.SetHasNavigationBar(this, false);

        Afficher();
    }

    //Pour toujours garder la liste mise à jour en changeant de page en page
    protected override void OnAppearing()
    {
        base.OnAppearing();
        selected = null;
        MyItems.Clear();
        Afficher();
    }

    private void ONItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is MyItem item)
        {
            selected = item;
        }
    }

    private async void Ajout(object sender, EventArgs e)
    {
        bool test = (bool)await this.ShowPopupAsync(new AjoutPopup());
        MyItems.Clear();
        Afficher();
    }

    private async void Supp(object sender, EventArgs e)
    {

        if (connection.ExecuteNonQuery("delete from produit where code = " + selected.Code) > 0)
        {
            await DisplayAlert("Information", "Suppression effectuée !", "OK");
            MyItems.Clear();
            Afficher();
        }
        else
        {
            await DisplayAlert("Alert", "Problème lors de la suppression !", "OK");
        }
        selected = null;


    }

    private async void Modf(object sender, EventArgs e)
    {
        bool test = false;
        if (selected != null)
        {
            test = (bool)await this.ShowPopupAsync(new ModfPopup(selected));
            Debug.Write(selected.Quantite);
        }
        if (test)
        {
            MyItems.Clear();
            Afficher();
            selected = null;

        }

    }
}