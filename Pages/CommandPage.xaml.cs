using CommunityToolkit.Maui.Views;
using Gestion_des_produits.Popups;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Gestion_des_produits.Pages;


public partial class CommandPage : ContentPage
{


    public ObservableCollection<Comd> ListeCommand { get; set; } = new ObservableCollection<Comd>();
    public CommandPage()
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
            var list = db.Comds.ToList();
            foreach (var c in list)
            {
                ListeCommand.Add(c);
            }
        }

    }

    private void RechID(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

         // Filter the list based on the search text
         if (string.IsNullOrEmpty(searchText))
         {
             cmndliste.ItemsSource = ListeCommand;
         }
         else
         {
             cmndliste.ItemsSource = ListeCommand.Where(item => item.ID.ToString().Contains(searchText));
         }
    }
    private void RechDate(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
         string searchText = e.NewTextValue;

         if (string.IsNullOrEmpty(searchText))
         {
             cmndliste.ItemsSource = ListeCommand;
         }
         else
         {
             cmndliste.ItemsSource = ListeCommand.Where(item => item.Date.ToLower().Contains(searchText.ToLower()));
         }
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        ListeCommand.Clear();
        rechercheID.Text = "";
        rechercheID.Placeholder = "Effectuer une recherche avec l'ID de la commande";
        rechercheDate.Text = "";
        rechercheDate.Placeholder = "Effectuer une recherche avec la date de la commande";
        Afficher();
    }

    private async void OnShowButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        Comd selectedCommand = (Comd)button.CommandParameter;
        if (selectedCommand != null)
        {
            await this.ShowPopupAsync(new CommandPopup(selectedCommand.listeArticles));

        }
    }
    


}