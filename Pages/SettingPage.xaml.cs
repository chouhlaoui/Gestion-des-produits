using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Diagnostics;

namespace Gestion_des_produits.Pages;


public partial class SettingPage : ContentPage
{
    string nom;
    int qnt;
    float prix;
    DateTime date;
    int code;
    readonly ConnectionSQL connection = new ConnectionSQL();

    public SettingPage()
    {
        InitializeComponent();
    }

    private async void Confirmation_Ajout(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(nomprod.Text) || string.IsNullOrEmpty(quantiteprod.Text) || string.IsNullOrEmpty(prixprod.Text))
        { await DisplayAlert("Alert", "Veuillez Remplir tous les champs !", "OK"); }
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

            }
            catch (FormatException)
            { await DisplayAlert("Alert", "Veuillez verifier les champs !", "OK"); }
            catch (MySqlException)
            {
            }
        }

    }

    private async void Confirmation_Suppression(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(codeprod.Text))
        {
            await DisplayAlert("Alert", "Champ du code non rempli", "OK");
        }
        else
        {
            try
            {
                code = int.Parse(codeprod.Text);
                if (connection.ExecuteNonQuery("delete from produit where code = " + code) > 0)
                {
                    codeprod.Text = "";
                    codeprod.Placeholder = "Code Produit";
                    await DisplayAlert("Information", "Suppression effectuée !", "OK");
                }
                else
                {
                    await DisplayAlert("Alert", "Problème lors de la suppression !", "OK");
                }
            }
            catch (FormatException)
            { await DisplayAlert("Alert", "Veuillez verifier le code !", "OK"); }

        }


    }

    private async void Confirmation_Modification(object sender, EventArgs e)
    {
        string query;
        if (string.IsNullOrEmpty(codemod.Text))
        {
            await DisplayAlert("Alert", "Champ du code non rempli", "OK");
        }
        else
        {
            try
            {
                code = int.Parse(codemod.Text);

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
                    codemod.Text = "";
                    codeprod.Placeholder = "Code Produit";
                    nommod.Text = "";
                    nommod.Placeholder = "Produit";
                    prixmod.Text = "";
                    prixmod.Placeholder = "Prix";
                    quantitemod.Text = "";
                    quantitemod.Placeholder = "Quantité";
                    await DisplayAlert("Information", "Modification effectuée !", "OK");
                }
                else
                {
                    await DisplayAlert("Alert", "Problème lors de la modification !", "OK");
                }
            }
            catch (Exception)
            {
                { await DisplayAlert("Alert", "Veuillez verifier les champs !", "OK"); }

            }
        }

    }

}