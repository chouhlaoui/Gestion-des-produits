using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Gestion_des_produits.Pages;


public partial class AcheterPage : ContentPage
{    

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
        using(var db = new AppDB())
        {
            var produits = db.Produits.ToList();

            foreach (var produit in produits)
            {
                ListeVente.Add(new Ligne
                {
                    Code = produit.Code,
                    NomProduit = produit.NomProduit,
                    PrixHT = produit.Prix,
                    Quantite = produit.Quantité
                });
            }
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

    private async void Button_Clicked(object sender, EventArgs e)
    {

        ObservableCollection<Ligne> ListeCommand = new ObservableCollection<Ligne>();  

        float total = 0;

        foreach (Ligne item in searchResults.ItemsSource)
        {
            bool isChecked = item.Check;
            if (isChecked)
            {
                ListeCommand.Add(item);
                total += item.PrixHT;
            }
        }
        if (total > 0)
        {
            bool x = (bool) await this.ShowPopupAsync(new PopupPage(ListeCommand));
            if (x)
            {
                ListeVente.Clear();
                Afficher();
            }
            else
            {
                foreach(var item in ListeVente)
                {
                    item.Check = false;
                }
            }
        }


    }


}