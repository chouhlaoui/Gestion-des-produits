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
    public Produit selected;
    public ObservableCollection<Produit> MyItems { get; set; } = new ObservableCollection<Produit>();


    //Connexion à la base de données et lecture des données
    public void Afficher()
    {
        using (var dbContext = new AppDB())
        {
            var produits = dbContext.Produits.ToList();

            foreach (var produit in produits)
            {
                MyItems.Add(produit);
            }
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
        if (e.SelectedItem is Produit item)
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

    private void Supp(object sender, EventArgs e)
    {
        using (var dbContext = new AppDB())
        {
            var produitToDelete = dbContext.Produits.FirstOrDefault(p => p.Code == selected.Code);
            if (produitToDelete != null)
            {
                dbContext.Produits.Remove(produitToDelete);
                dbContext.SaveChanges();
            }
        }
        selected = null;
    }

    private async void Modf(object sender, EventArgs e)
    {
        bool test = false;
        if (selected != null)
        {
            test = (bool)await this.ShowPopupAsync(new ModfPopup(selected));
        }
        if (test)
        {
            MyItems.Clear();
            Afficher();
            selected = null;
        }

    }
}