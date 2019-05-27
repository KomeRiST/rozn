using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace rozn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            rozn.DataSet1 dataSet1 = ((rozn.DataSet1)(this.FindResource("dataSet1")));
            // Загрузить данные в таблицу CR_EGAIS_MONITOR3. Можно изменить этот код как требуется.
            rozn.DataSet1TableAdapters.CR_EGAIS_MONITOR3TableAdapter dataSet1CR_EGAIS_MONITOR3TableAdapter = new rozn.DataSet1TableAdapters.CR_EGAIS_MONITOR3TableAdapter();
            dataSet1CR_EGAIS_MONITOR3TableAdapter.Fill(dataSet1.CR_EGAIS_MONITOR3);
            System.Windows.Data.CollectionViewSource cR_EGAIS_MONITOR3ViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cR_EGAIS_MONITOR3ViewSource")));
            cR_EGAIS_MONITOR3ViewSource.View.MoveCurrentToFirst();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(((sender as ListBox).SelectedItem as System.Data.DataRowView).Row.ToString()); //СКЛАД_БУХ = "+Кредос"
            label1.Content = ((sender as ListBox).SelectedItem as System.Data.DataRowView).Row["СКЛАД_БУХ"];
        }
    }
}
