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
   
    public AjoutPopup()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private async void Confirmation_Ajout(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(nomprod.Text) || string.IsNullOrEmpty(quantiteprod.Text) || string.IsNullOrEmpty(prixprod.Text)) 
        {    
            await Application.Current.MainPage.DisplayAlert("Alert", "Veuillez Remplir tous les champs !", "OK");
        }
        else
        {
            try
            {
                using (var dbContext = new AppDB())
                {
                    var newProduit = new Produit
                    {
                        NomProduit = nomprod.Text,
                        Delai = Delai.Date.ToString("dd-MM-yyyy"),
                        Prix = float.Parse(prixprod.Text),
                        Quantité = int.Parse(quantiteprod.Text)
                    };

                    dbContext.Produits.Add(newProduit);
                    dbContext.SaveChanges();
                }
                
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
            
        }

    }

    private void Annuler(object sender, EventArgs e) => Close(false);

}