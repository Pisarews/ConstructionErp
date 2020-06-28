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


        public struct VerificationBool
        {
            public bool isVerified { get; set; }
            public string message { get; set; }
        }
     
        public static VerificationBool serviceSelectionVerif = new VerificationBool();
        public static VerificationBool vatSelectionVerif = new VerificationBool();
        public static VerificationBool quantityVerification = new VerificationBool();
        
        public Uslugi()
        {
            InitializeComponent();
            servicesComboboxLoader();
            vatSelectionComboboxLoader();
            VerificationGenerator(); 
        }

        #region Display
        /* 
         * 
         * Tutaj znajdują się generatory wyświetlanych danych oraz obsługa zdarzeń przez użytkownika
         * Wyświetlanie paneli, ładowanie danych oraz aktualizacja powiązań danych
         * 
         */

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

            dataUpdate();
            serviceSelectionVerif.isVerified = true; 

        }

        private void UnityPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            Services changeValue = Services.uslugi.Find(x => (x.idUslugi == int.Parse(serviceSelectionCombobox.SelectedValue.ToString())));
            changeValue.kwotaJednostkowa = Convert.ToDecimal(NetPrice.Text);
            Services.isChanged = true;
            dataUpdate(); 
        }

        #region VAT selector + items generator
        private void vatSelectionComboboxLoader()
        {
             var items = new[] 
             {
                new { Text = "0 %", Value = 0M },
                new { Text = "6 %", Value = 6M },
                new { Text = "12 %", Value = 12M },
                new { Text = "21 %", Value = 21M },
                new { Text = "0 % (AL)*", Value = 0M },
             };

            vatSelectionCombobox.DisplayMemberPath = "Text";
            vatSelectionCombobox.SelectedValuePath = "Value";
            vatSelectionCombobox.ItemsSource = items;
            
        }

        #endregion

        private void vatSelectionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataUpdate();
            vatSelectionVerif.isVerified = true;
        }

        private void quantitySelector_ValueChanged(object sender, ControlLib.ValueChangedEventArgs e)
        {
            dataUpdate();
            quantityVerification.isVerified = true; 
        }

        private void rabatSelector_ValueChanged(object sender, ControlLib.ValueChangedEventArgs e)
        {
            dataUpdate();
        }

        #endregion

        #region Calculators

        /*Aktualizacja kwot w Formularzu */
                
        private decimal netCalculator(decimal unityPrice)
        {
            decimal quantity = (decimal) quantitySelector.Value; 
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

        private decimal netCalculatorRabat(decimal netPrice, decimal rabat)
        {
            if (netPrice != null && rabat != null)
            {
                rabat = rabat / 100;
                netDisplayAfterRabat.Text = (netPrice - netPrice * rabat).ToString();
                rabatCost.Text = (Convert.ToDecimal(NetDisplay.Text) - (netPrice - (netPrice * rabat))).ToString();
                return (netPrice - (netPrice * rabat));
                
            }
            else { return 0; }
        }

        private void vatCalculator()
        {
            decimal taxe;
            taxe = (Convert.ToDecimal(netDisplayAfterRabat.Text) / 100) * Convert.ToDecimal(vatSelectionCombobox.SelectedValue);
            vat.Text = taxe.ToString();
            return; 
        }

        private void brutCalculator()
        {
            decimal brutto = Convert.ToDecimal(vat.Text) + Convert.ToDecimal(netDisplayAfterRabat.Text);
            brut.Text = brutto.ToString(); 
        }

        private void dataUpdate()
        {
            netCalculator(Convert.ToDecimal(NetPrice.Text));
            netCalculatorRabat(Convert.ToDecimal(NetDisplay.Text), Convert.ToDecimal(rabatSelector.Value));
            vatCalculator();
            brutCalculator(); 
        }

        #endregion

        #region Submit and Verification
        /*  
         *  Weryfikacja oraz zatwierdzanie danych przez użytkownika\
         *  Obsługa zdarzeń oraz wyświetlenie Popup...
         *  
         */
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Verify(); 
        }

        #endregion

        #region VerificationMethods 

        
        public static void VerificationGenerator()
        {
            /*Obsługę języków...*/

            serviceSelectionVerif.isVerified = false;
            serviceSelectionVerif.message = "Proszę wybrać usługę.";

            vatSelectionVerif.isVerified = false;
            vatSelectionVerif.message = "Proszę wybrać stawkę VAT.";

            quantityVerification.isVerified = false;
            quantityVerification.message = "Ilość nie może być zerowa.";
        }

        public static void Verify()
        {
            var items = new[]
            {
                new { item = serviceSelectionVerif },
                new { item = quantityVerification },
                new { item = vatSelectionVerif }
            };

            foreach(var x in items)
            {
                if(x.item.isVerified == false)
                {
                    Debug.WriteLine(x.item.message);
                }
            }
        }

        #endregion

    }
}
