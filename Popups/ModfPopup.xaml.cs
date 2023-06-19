using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using static Gestion_des_produits.Pages.ListeProduits;

namespace Gestion_des_produits.Pages;

public partial class ModfPopup : Popup
{
    int code;
    readonly ConnectionSQL connection = new ConnectionSQL();

    public ModfPopup(MyItem item)
	{
		InitializeComponent();
		
		BindingContext = item;

        try
        {
            code = int.Parse(item.Code);
        }
        catch (Exception) {}

    }
    private void Confirmation_Modification(object sender, EventArgs e) {
        string query;
        

        try
            {

                if (!(string.IsNullOrEmpty(nommod.Text)) && !(string.IsNullOrEmpty(prixmod.Text)) && !(string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET NomProduit=\"" + nommod.Text + "\", PrixHT=" + float.Parse(prixmod.Text) + ",Quantité = " + int.Parse(quantitemod.Text) + " where code=" + code;
                }
                else if (!(string.IsNullOrEmpty(nommod.Text)) && !(string.IsNullOrEmpty(prixmod.Text)) && (string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET NomProduit=\"" + nommod.Text + "\", PrixHT=" + float.Parse(prixmod.Text) + " where code=" + code;

                }
                else if (!(string.IsNullOrEmpty(nommod.Text)) && (string.IsNullOrEmpty(prixmod.Text)) && !(string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET NomProduit=\"" + nommod.Text + "\",Quantité = " + int.Parse(quantitemod.Text) + " where code=" + code;

                }
                else if ((string.IsNullOrEmpty(nommod.Text)) && !(string.IsNullOrEmpty(prixmod.Text)) && !(string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET PrixHT=" + float.Parse(prixmod.Text) + ",Quantité = " + int.Parse(quantitemod.Text) + " where code=" + code;
                }
                else if ((string.IsNullOrEmpty(nommod.Text)) && (string.IsNullOrEmpty(prixmod.Text)) && !(string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET Quantité = " + int.Parse(quantitemod.Text) + " where code=" + code;

                }
                else if (!(string.IsNullOrEmpty(nommod.Text)) && (string.IsNullOrEmpty(prixmod.Text)) && (string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET NomProduit=\"" + nommod.Text + "\" where code=" + code;

                }
                else if ((string.IsNullOrEmpty(nommod.Text)) && !(string.IsNullOrEmpty(prixmod.Text)) && (string.IsNullOrEmpty(quantitemod.Text)))
                {
                    query = "update produit SET PrixHT=" + float.Parse(prixmod.Text) + " where code=" + code;
                }
                else { query = ""; }

                Debug.WriteLine(query);
                if (connection.ExecuteNonQuery(query) > 0)
                {
                    Close(true);
                }
                else
                {
                    Close(false);
                }
        }
            catch (Exception)
            {

            }
        

    }
    private void Annuler(object sender, EventArgs e) => Close(false);
}
