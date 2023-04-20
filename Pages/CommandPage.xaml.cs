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
                    Date = reader.GetString(3)
                });
                Debug.WriteLine(reader.GetTextReader(2).ReadToEnd());
            }
            reader.Close();
        }
        catch (MySqlException)
        {
            Console.WriteLine("Error: Unable to connect to the database.");
        }

    }

    private void SearchBarTextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        /* string searchText = e.NewTextValue;

         // Filter the list based on the search text
         if (string.IsNullOrEmpty(searchText))
         {
             searchResults.ItemsSource = ListeVente;
         }
         else
         {
             searchResults.ItemsSource = ListeVente.Where(item => item.NomProduit.ToLower().Contains(searchText.ToLower()));
         }*/
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        ListeCommand.Clear();
        searchbar.Text = "";
        searchbar.Placeholder = "Effectuer une recherche avec le nom du produit";
        Afficher();
    }

    private async void OnShowButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        LigneCommand selectedCommand = (LigneCommand)button.CommandParameter;
        if (selectedCommand != null)
        {
            await DisplayAlert("Details", selectedCommand.ListeDesAchats, "OK");
        }
    }


}