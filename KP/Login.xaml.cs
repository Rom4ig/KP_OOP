using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.Text.RegularExpressions;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private MainWindow mainWindow;
        public Login(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            int user_id=0;        
            if (user_name.Text.Length > 0) // проверяем введён ли логин
            {
                if (user_password.Password.Length > 0) // проверяем введён ли пароль
                {
                    // ищем в базе данных пользователя с такими данными
                    string pass = User.HashPassword(user_password.Password);
                    DataTable dt_user = mainWindow.Select("SELECT Id FROM [dbo].[users] WHERE [login] = '" + user_name.Text +
                        "' AND [password] = '" + pass + "'");  
                    foreach (DataRow row in dt_user.Rows)
                    {
                        var Ids = row.ItemArray;
                        foreach (int id in Ids)
                            user_id = id;
                    }
                    if (dt_user.Rows.Count > 0) // если такая запись существует
                    {                        
                        MessageBox.Show("Пользователь авторизовался", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk); // говорим, что авторизовался
                        User.OnlinePerson = user_id;
                        mainWindow.OpenPage(MainWindow.pages.garage);
                    }
                    else MessageBox.Show("Проверьте корректность введенных данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); // выводим ошибку                   
                }
                else // выводим ошибку
                {
                    user_password.BorderBrush = Brushes.Red;
                    MessageBox.Show("Введите Пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); // выводим ошибку
                   
                }
            }

            else
            {
                user_name.BorderBrush = Brushes.Red;
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); // выводим ошибку
               
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.register);
        }
        private void user_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            user_name.BorderBrush = Brushes.Gray;
        }
        private void user_password_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            user_password.BorderBrush = Brushes.Gray;
        }
        private void user_name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputSymbol = e.Text.ToString();
            if (!Regex.Match(inputSymbol, @"[a-zA-zа-яА-Я0-9_,.@$#!*]").Success)
            {
                e.Handled = true;
            }
        }
    }
}
