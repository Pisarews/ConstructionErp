using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using JsonLibrary; 

namespace ConstructionERP
{

    public class Services
    {

        public string nazwaUslugi { get;  set; }

        public decimal kwotaJednostkowa { get; set; }

        public int idUslugi { get; set; }

        public bool Equals(Services other)
        {
            if (other == null) return false;
            return (this.idUslugi.Equals(other.idUslugi));
        }

        public static bool isChanged; 

        public static List<Services> uslugi = new List<Services>();


        public static void LoadServicesFromDatabase()
        {
            using MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM Uslugi", con);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    /*
                     * access your record colums by using reader
                            Debug.WriteLine(reader["idUslug"]);
                            Debug.WriteLine(reader["cenaJednostkowa"]);
                            Debug.WriteLine(reader["usluga"]); 
                     */
                    Services.uslugi.Add(new Services
                    {
                        idUslugi = (int)reader["idUslug"],
                        nazwaUslugi = reader["usluga"].ToString(),
                        kwotaJednostkowa = 0.00M
                    });

                }

                reader.Dispose();

                con.Close();
                
                return;
            }
        }

        public static void SaveDataServices()
        {

                List<Services> loadServices = JsonSerialization.JsonDeserializer<Services>(@"C:\Users\pisaq\source\repos\ConstructionERP\Datas\ServicesData\Services.json");
                if (loadServices == null)
                {
                    JsonSerialization.JsonSerializer<Services>(@"C:\Users\pisaq\source\repos\ConstructionERP\Datas\ServicesData\Services.json", uslugi);
                    
                }
                else
                {
                    
                    JsonSerialization.JsonSerializer<Services>(@"C:\Users\pisaq\source\repos\ConstructionERP\Datas\ServicesData\Services.json", uslugi);
                }
  
        }


    }
}

   
