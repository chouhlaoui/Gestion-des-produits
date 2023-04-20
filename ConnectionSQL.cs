using MySql.Data.MySqlClient;

//CEtte classe est destinée pour etablir la connexion avec la base de données

namespace Gestion_des_produits
{
    public class ConnectionSQL
    {
        MySqlConnection connection;

        public ConnectionSQL()
        {
            connection = new MySqlConnection("server=localhost;user id=root;password=admin;database=projet_maui");
            connection.Open();
        }

        public MySqlDataReader ExecuteQuery(string sql)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                return command.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                // Gérer l'exception ici
                Console.WriteLine("Erreur lors de l'exécution de la requête : " + ex.Message);
                return null;
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                return command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // Gérer l'exception ici
                Console.WriteLine("Erreur lors de l'exécution de la requête : " + ex.Message);
                return -1;
            }

        }
    }
}
