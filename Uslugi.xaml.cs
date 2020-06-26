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
using DocumentFormat.OpenXml.Bibliography;
using System.ComponentModel;
using DocumentFormat.OpenXml.Office.CustomUI;

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
            servicesComboboxLoader();
            vatSelectionComboboxLoader();
        }

   

        private void servicesComboboxLoader ()
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

        }

        private void serviceSelectionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Debug.WriteLine("UWAGA KURWA " + (uslugi.Exists(x => (x.idUslugi == (int) serviceSelectionCombobox.SelectedValue))));

            
            Services usl = Services.uslugi.Find(x => (x.idUslugi == int.Parse(serviceSelectionCombobox.SelectedValue.ToString())));
            if (usl != null)
            {
                NetPrice.Text = usl.kwotaJednostkowa.ToString();
            }

            netCalculator(Convert.ToDecimal(NetPrice.Text), 5);
        }

        private void UnityPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            Services changeValue = Services.uslugi.Find(x => (x.idUslugi == int.Parse(serviceSelectionCombobox.SelectedValue.ToString())));
            changeValue.kwotaJednostkowa = Convert.ToDecimal(NetPrice.Text);
            Services.isChanged = true;
            netCalculator(Convert.ToDecimal(NetPrice.Text), 5);
        }

        
        
 
        private void vatSelectionComboboxLoader()
        {
             var items = new[] 
             {
                new { Text = "0 %", Value = 0M },
                new { Text = "10 %", Value = 10M },
                new { Text = "20 %", Value = 20M },
                new { Text = "30 %", Value = 30M },
                new { Text = "40 %", Value = 40M },
             };

            vatSelectionCombobox.DisplayMemberPath = "Text";
            vatSelectionCombobox.SelectedValuePath = "Value";
            vatSelectionCombobox.ItemsSource = items;

        }

        private void vatSelectionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private decimal netCalculator(decimal unityPrice, decimal quantity)
        {
            if ((quantity != null && unityPrice != null))
            {
                NetDisplay.Text = (unityPrice * quantity).ToString();
                return unityPrice * quantity;
            }
            else
            {
                return 0; 
            }
        }
    }
}
