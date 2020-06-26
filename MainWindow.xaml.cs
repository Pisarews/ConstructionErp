using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace ConstructionERP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
       
        public MainWindow()
        {
            InitializeComponent();
            string servicesPath = @"C:\Users\pisaq\source\repos\ConstructionERP\Datas\ServicesData\Services.json"; 
            if (File.Exists(servicesPath))
            {
                Services.uslugi = JsonLibrary.JsonSerialization.JsonDeserializer<Services>(servicesPath);
                Debug.WriteLine("Deserializacja miała miejsce...");
                Debug.WriteLine(Services.uslugi.Count);
                
            }
            else
            {
                Services.LoadServicesFromDatabase();
            }

            Services.uslugiDT = Services.ConvertToDatatable(Services.uslugi); 

        }

        private void btnloaddata_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select NrFaktury, Klient, Kwota, DataFaktury from faktury", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                FakturyGrid.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnaddfacture_Click(object sender, RoutedEventArgs e)
        {
            KreatorFaktury kreator = new KreatorFaktury();
            kreator.Show(); 
        }

        private void btnGenerateFacture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Services.isChanged == true)
            {
                //File.WriteAllText(@"C:\Users\pisaq\source\repos\ConstructionERP\Datas\ServicesData\Services.json", String.Empty);
                Services.SaveDataServices();
                Debug.WriteLine("CHANGES in PROGRESS to JSON FILE... ! ! ! "); 
            }
            else
            {
                Debug.WriteLine("NO CHANGES... Window Closing");
            }
        }
    }
}
