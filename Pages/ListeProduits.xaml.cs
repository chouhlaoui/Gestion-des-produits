using System.Collections.ObjectModel;
using System.Drawing;
using MySql.Data.MySqlClient;
using Mysqlx.Connection;

namespace Gestion_des_produits.Pages;


//Definition d'une ligne de la liste des produits 


public partial class ListeProduits : ContentPage
{
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
        ConnectionSQL connection = new ConnectionSQL();

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
                    Delai = reader.GetString(2).Remove(9),
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
    public ObservableCollection<MyItem> MyItems { get; set; } = new ObservableCollection<MyItem>();
    public ListeProduits()
    {
        InitializeComponent();
        BindingContext = this;
        Afficher();
    }

    //Pour toujours garder la liste mise à jour en changeant de page en page
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MyItems.Clear();
        Afficher();
    }

}