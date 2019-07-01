using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Balance.xaml
    /// </summary>
    public partial class Balance : Page
    {
        MainWindow mainWindow;
        public Balance(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            int id = User.OnlinePerson;
            string money = Money.Text.Replace(',', '.');
            mainWindow.Select($"Update [dbo].[users] set balance = balance+{money} where Id={id}"); //Через .
            MessageBox.Show("Баланс пополнен", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Card.Text = "";
            Money.Text = "";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.garage);
        }

        private void Card_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputSymbol = e.Text.ToString();
            if (!Regex.Match(inputSymbol, @"[0-9*]").Success)
            {
                e.Handled = true;
            }
        }

        private void Money_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputSymbol = e.Text.ToString();
            if (!Regex.Match(inputSymbol, @"[0-9]").Success)
            {
                e.Handled = true;
            }
        }
    }
}
