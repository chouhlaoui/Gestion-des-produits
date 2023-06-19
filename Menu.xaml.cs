using Gestion_des_produits.Pages;

namespace Gestion_des_produits;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
        BindingContext = this;
        NavigationPage.SetHasNavigationBar(this, false);
    }

 


    private async void btn1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListeProduits());


    }

    private async void btn2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AcheterPage());


    }

    private async void btn3(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CommandPage());


    }
}