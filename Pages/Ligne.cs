using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_des_produits.Pages
{
    public class Ligne
    {
        public string NomProduit { get; set; }
        public string PrixHT { get; set; }
        public string Quantite { get; set; }
        public bool Check { get; set; }

    }
}
