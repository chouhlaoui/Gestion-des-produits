using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using static Gestion_des_produits.Pages.ListeProduits;

namespace Gestion_des_produits.Pages;

public partial class ModfPopup : Popup
{
    int code;
    public ModfPopup(Produit item)
	{
		InitializeComponent();
		
		BindingContext = item;
        try
        {
            code = item.Code;
        }
        catch (Exception) {}

    }
    private void Confirmation_Modification(object sender, EventArgs e) {

        try
        {
            using (var dbContext = new AppDB())
            {
                Produit updateProd = dbContext.Produits.FirstOrDefault(p => p.Code == code);
                if (updateProd != null)
                {
                    if (nommod.Text != null)
                    {
                        updateProd.NomProduit = nommod.Text;
                    }
                    if (prixmod.Text != null)
                    {
                        updateProd.Prix = float.Parse(prixmod.Text);
                    }
                    if (quantitemod.Text != null)
                    {
                        updateProd.Quantité = int.Parse(quantitemod.Text);
                    }

                    dbContext.SaveChanges();
                }
                Close(true);
            }
        }
        catch (Exception)
        {

        }
        
    }
    private void Annuler(object sender, EventArgs e) => Close(false);
}
