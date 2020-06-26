using DocumentFormat.OpenXml.Drawing.Charts;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Linq;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.IO;
using DataTable = System.Data.DataTable;

namespace ConstructionERP
{
    /// <summary>
    /// Logika interakcji dla klasy Uslugi.xaml
    /// </summary>
    public partial class Uslugi : Window
    {
        public Uslugi()
        {

            InitializeComponent();
            servicesComboboxLoader("SELECT * from uslugi;", "usluga");
            
        }

   

        private void servicesComboboxLoader (string x, string y)
        {

            if(Services.uslugi != null)
            {
                serviceSelectionCombobox.ItemsSource = Services.uslugiDT.DefaultView;
                serviceSelectionCombobox.DisplayMemberPath = "nazwaUslug";
                serviceSelectionCombobox.SelectedValuePath = "idUslug";
            }
            else
            {
                Debug.WriteLine("Services.uslugi DEBUG == null"); 
            }
          
            /*
                using MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                try
                {
                    con.Open();
                } 
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message); 
                }
            if (con.State == ConnectionState.Open)
            {
                using MySqlDataAdapter sda = new MySqlDataAdapter(x, con);
                System.Data.DataTable dt = new System.Data.DataTable();
                sda.Fill(dt);
                serviceSelectionCombobox.ItemsSource = dt.DefaultView;
                serviceSelectionCombobox.DisplayMemberPath = y;
                serviceSelectionCombobox.SelectedValuePath = "idUslug";
                sda.Dispose();
                con.Close();
            } 
            else 
            {
                if (File.Exists(@"C:\Users\pisaq\source\repos\ConstructionERP\Datas\ServicesData\Services.json"))
                {
          
                    DataTable dt = new DataTable(); 
                    dt = ConvertToDatatable(Services.uslugi);
                    serviceSelectionCombobox.ItemsSource = dt.DefaultView;
                    serviceSelectionCombobox.DisplayMemberPath = "nazwaUslug";
                    serviceSelectionCombobox.SelectedValuePath = "idUslug";
                }
            }
            */
           

        }

        private void serviceSelectionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Debug.WriteLine("UWAGA KURWA " + (uslugi.Exists(x => (x.idUslugi == (int) serviceSelectionCombobox.SelectedValue))));

            
            Services usl = Services.uslugi.Find(x => (x.idUslugi == int.Parse(serviceSelectionCombobox.SelectedValue.ToString())));
            if (usl != null)
            {
                NetPrice.Text = usl.kwotaJednostkowa.ToString();
            }
        }

        private void NetPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            Services changeValue = Services.uslugi.Find(x => (x.idUslugi == int.Parse(serviceSelectionCombobox.SelectedValue.ToString())));
            changeValue.kwotaJednostkowa = Convert.ToDecimal(NetPrice.Text);
            Services.isChanged = true;
        }

        
       
    }
}
