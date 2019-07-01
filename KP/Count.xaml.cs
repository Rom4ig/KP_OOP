using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Count.xaml
    /// </summary>
    public partial class Count : Page
    {
        MainWindow mainWindow;
        public Count(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void ChangeC_Click(object sender, RoutedEventArgs e)
        {
            string car = "";
            DataTable balance1 = mainWindow.Select($"Select Name from [dbo].[cars] where Id={User.car_id}");
            foreach (DataRow row in balance1.Rows)
            {
                var Ids = row.ItemArray;
                foreach (string names in Ids)
                    car = names;
            }
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите изменить количество для транспортного средства {car} на {Counts.Text} ?", "Подветрдите действие", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                mainWindow.Select($"Update [dbo].[Cars] set Count = {Counts.Text} Where Id={User.car_id}");
                MessageBox.Show($"Вы установили количество {Counts.Text} для {car}", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Counts.Text = "";
            }
        }

        private void BackK_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.cars);
        }

        private void Counts_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputSymbol = e.Text.ToString();
            if (!Regex.Match(inputSymbol, @"[0-9*]").Success)
            {
                e.Handled = true;
            }
        }
    }
}