using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Microsoft.EntityFrameworkCore;

namespace Gestion_des_produits.Pages
{
    public class AfficheCommand : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private int quantite;
        private float total;

        public string NomProduit { get; set; }
        public float PrixHT { get; set; }

        public int Quantite
        {
            get => quantite;
            set
            {
                if (quantite != value)
                {
                    quantite = value;
                    Total = PrixHT * quantite;
                    OnPropertyChanged(nameof(Quantite));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public float Total
        {
            get => total;
            set
            {
                if (total != value)
                {
                    total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class PopupPage : Popup
    {

        public ObservableCollection<AfficheCommand> AfficheCommandListe { get; set; } = new ObservableCollection<AfficheCommand>();

        public PopupPage(ObservableCollection<Ligne> L)
        {
            InitializeComponent();
            BindingContext = this;

            double sommeTotal = 0;
            foreach (Ligne ligne in L)
            {
                if (ligne.Quantite.Equals("0")){}
                else
                {
                    AfficheCommandListe.Add(new AfficheCommand
                    {
                        ID = ligne.Code,
                        NomProduit = ligne.NomProduit,
                        PrixHT = ligne.PrixHT,
                        Quantite = 1,
                        Total = ligne.PrixHT
                    });
                    sommeTotal += ligne.PrixHT;
                }
            }

            SommeTotal.Text = sommeTotal.ToString("0.##");
        }

        private void AnnulerButtonClicked(object sender, EventArgs e) => Close(false);

        private void ConfirmerButtonClicked(object sender, EventArgs e)
        {
            String command = "";

            using (var db = new AppDB())
            {
                 foreach (var item in AfficheCommandListe)
                 {
                        var prod = db.Produits.FirstOrDefault(p => p.Code == item.ID);
                        if (prod != null)
                        {
                            prod.Quantité = prod.Quantité - item.Quantite;
                            db.SaveChanges();
                        }

                        command += item.NomProduit + " | " + item.PrixHT + " | x" + item.Quantite + " | " + item.Total + "\n";
                 }


            }

            command += "Total de la commande : " + SommeTotal.Text ;
            using (var db = new AppDB())
            {

                db.Comds.Add(new Comd
                {
                    Date = DateTime.Today.ToString("yyyy-MM-dd"),
                    listeArticles = command,
                    Total = float.Parse(SommeTotal.Text)
                });
                db.SaveChanges();
            }

            Close(true);
        }

        void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            var stepper = sender as Stepper;
            if (stepper?.BindingContext is AfficheCommand afficheCommand)
            {
                afficheCommand.Quantite = (int)e.NewValue;

                double sommeTotal = 0;
                foreach (AfficheCommand cmd in AfficheCommandListe)
                {
                    sommeTotal += cmd.Total;
                }
                SommeTotal.Text = sommeTotal.ToString("0.##");
            }
        }

        private void ONItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            double somme;
            if (e.SelectedItem is AfficheCommand item)
            {
                AfficheCommandListe.Remove(item);
                somme = double.Parse(SommeTotal.Text);

                somme -= item.Total;
                SommeTotal.Text = somme.ToString("0.##");
            }
        }
    }
}
