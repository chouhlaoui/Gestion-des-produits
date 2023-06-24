using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_des_produits.Pages
{
    public class Ligne
    {
        public int Code { get; set; }
        public string NomProduit { get; set; }
        public float PrixHT { get; set; }
        public int Quantite { get; set; }
        public bool Check { get; set; }

    }
}
