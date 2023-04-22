using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace Gestion_des_produits.Pages
{
    public class AfficheCommand : INotifyPropertyChanged
    {
        private int quantite;
        private string total;

        public string NomProduit { get; set; }
        public string PrixHT { get; set; }

        public int Quantite
        {
            get => quantite;
            set
            {
                if (quantite != value)
                {
                    quantite = value;
                    Total = String.Format("{0:0.##}", (double.Parse(PrixHT) * quantite));
                    OnPropertyChanged(nameof(Quantite));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public string Total
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
                        NomProduit = ligne.NomProduit,
                        PrixHT = ligne.PrixHT,
                        Quantite = 1,
                        Total = ligne.PrixHT
                    });
                    sommeTotal += double.Parse(ligne.PrixHT);
                }
            }

            SommeTotal.Text = sommeTotal.ToString("0.##");
        }

        private void AnnulerButtonClicked(object sender, EventArgs e)
        {
            Close(true);
        } 

        private void ConfirmerButtonClicked(object sender, EventArgs e)
        {
            ConnectionSQL connection = new ConnectionSQL();

            String command = "";

            foreach (var item in AfficheCommandListe)
            {           
                try
                {
                    String query = "SELECT Quantité,code FROM produit where NomProduit=\"" + item.NomProduit + "\";";
                    Debug.WriteLine(query);
                    MySqlDataReader reader = connection.ExecuteQuery(query);
                    while (reader.Read())
                    {                      
                        String query0 = "update produit SET Quantité = " + (reader.GetInt32(0) - item.Quantite) + " where code=" + reader.GetInt32(1) + ";";
                        
                        ConnectionSQL connection1 = new ConnectionSQL();
                        if (connection1.ExecuteNonQuery(query0) >= 0)
                        {
                            Debug.WriteLine("cbon");
                        }
                        else
                        {
                            Debug.WriteLine("non");
                        }
                    }
                    reader.Close();
                     
                    command += item.NomProduit + " | " + item.PrixHT + " | x" + item.Quantite + " | " + item.Total + "\n";
                }
                catch (MySqlException ex)
                {
                    Debug.WriteLine("MySQL error: " + ex.Message);
                }
            }


            command += "Total de la commande : " + SommeTotal.Text ;
            String query1 = "INSERT INTO commande(total,listeDesArticles,date) VALUES(" + float.Parse(SommeTotal.Text) + ",\"" + command + "\",\"" + DateTime.Today.ToString("yyyy-MM-dd") + "\");";
            connection.ExecuteNonQuery(query1);

            connection.Finish();
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
                    sommeTotal += double.Parse(cmd.Total);
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

                somme -= double.Parse(item.Total);
                SommeTotal.Text = somme.ToString("0.##");
            }
        }
    }
}
