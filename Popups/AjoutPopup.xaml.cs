using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using MySql.Data.MySqlClient;
using System.Drawing;
using Microsoft.Maui.Controls;
using static Gestion_des_produits.Pages.ListeProduits;
using CommunityToolkit.Maui.Alerts;

namespace Gestion_des_produits.Pages;

public partial class AjoutPopup : Popup
{
    string nom;
    int qnt;
    float prix;
    DateTime date;
    int code;
    readonly ConnectionSQL connection = new();
    public AjoutPopup()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private async void Confirmation_Ajout(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(nomprod.Text) || string.IsNullOrEmpty(quantiteprod.Text) || string.IsNullOrEmpty(prixprod.Text)) {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var toast = Toast.Make("Veuillez Remplir tous les champs !", ToastDuration.Short);

            await toast.Show(cancellationTokenSource.Token);
        }
        else
        {
            try
            {
                nom = nomprod.Text;
                qnt = int.Parse(quantiteprod.Text);
                prix = float.Parse(prixprod.Text);
                date = Delai.Date;
                string DATE = date.ToString("yyyy-MM-dd");
                string query = "insert into produit(NomProduit, Delai, PrixHT,Quantité) values(\"" + nom + "\",\"" + DATE + "\"," + prix + "," + qnt + ")";
                connection.ExecuteNonQuery(query);
                prixprod.Text = "";
                prixprod.Placeholder = "Prix";
                quantiteprod.Text = "";
                quantiteprod.Placeholder = "Quantité";
                nomprod.Text = "";
                nomprod.Placeholder = "Produit";
                Close(true);
            }
            catch (FormatException)
            { await Application.Current.MainPage.DisplayAlert("Alert", "Veuillez verifier les champs !", "OK"); }
            catch (MySqlException)
            {
            }
        }

    }

    private void Annuler(object sender, EventArgs e) => Close(false);

}