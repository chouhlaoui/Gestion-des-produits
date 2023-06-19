using CommunityToolkit.Maui.Views;
using System.Diagnostics;

namespace Gestion_des_produits.Popups;

public partial class CommandPopup : Popup
{
    public class Command
    {
        public string Prod { get; set; }
        public string Unitaire { get; set; }
        public string Qte { get; set; }
        public string Tot { get; set; }
    }

    public List<Command> commands { get; set; }
    public string total { get; set; }
    public CommandPopup(string ch)
    {
        InitializeComponent();
        commands = new List<Command>();
        Detach(ch);
        BindingContext = this;
    }

    void Detach(string ch)
    {
        if (!string.IsNullOrEmpty(ch))
        {
            string[] listecmd = ch.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string cmd in listecmd)
            {
                if (cmd.Contains('|'))
                {
                    string[] cmd1 = cmd.Split(new[] { '|' });

                    Command c = new Command
                    {
                        Prod = cmd1[0],
                        Unitaire = cmd1[1],
                        Qte = cmd1[2],
                        Tot = cmd1[3]
                    };

                    commands.Add(c);

                }
                else
                {
                    total = cmd;
                }
            }
        }
    }

    private void Out(object sender, EventArgs e) => Close();
}
