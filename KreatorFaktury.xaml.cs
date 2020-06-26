using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConstructionERP
{
    /// <summary>
    /// Logika interakcji dla klasy KreatorFaktury.xaml
    /// </summary>
    public partial class KreatorFaktury : Window
    {
        public KreatorFaktury()
        {
            InitializeComponent();
        }

        private void dodajusluge_Click(object sender, RoutedEventArgs e)
        {
            Uslugi uslugi = new Uslugi();
            uslugi.Show(); 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
