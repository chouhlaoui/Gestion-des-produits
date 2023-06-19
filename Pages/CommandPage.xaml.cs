using CommunityToolkit.Maui.Views;
using Gestion_des_produits.Popups;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Gestion_des_produits.Pages;


public partial class CommandPage : ContentPage
{

    public class LigneCommand
    {
        public string IDCommand { get; set; }
        public string ListeDesAchats { get; set; }
        public string Total { get; set; }
        public string Date { get; set; }


    }

    public ObservableCollection<LigneCommand> ListeCommand { get; set; } = new ObservableCollection<LigneCommand>();
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
        #pragma warning disable IDE0090 // Use 'new(...)'
        ConnectionSQL connection = new ConnectionSQL();
        #pragma warning restore IDE0090 // Use 'new(...)'

        try
        {
            string query = "SELECT * FROM commande";
            MySqlDataReader reader = connection.ExecuteQuery(query);

            while (reader.Read())
            {
                ListeCommand.Add(new LigneCommand
                {

                    IDCommand = reader.GetString(0),
                    Total = reader.GetString(1),
                    ListeDesAchats = reader.GetTextReader(2).ReadToEnd(),
                    Date = reader.GetDateTime(3).ToString("dd/MM/yyyy")
                }) ;
            }
            reader.Close();
        }
        catch (MySqlException)
        {
            Console.WriteLine("Error: Unable to connect to the database.");
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
             cmndliste.ItemsSource = ListeCommand.Where(item => item.IDCommand.ToLower().Contains(searchText.ToLower()));
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
        LigneCommand selectedCommand = (LigneCommand)button.CommandParameter;
        if (selectedCommand != null)
        {
            await this.ShowPopupAsync(new CommandPopup(selectedCommand.ListeDesAchats));

        }
    }
    


}