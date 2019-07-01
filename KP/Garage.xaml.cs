using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Garage.xaml
    /// </summary>
    public partial class Garage : Page
    {
        MainWindow mainWindow;
        public Garage(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.login);
        }
        private void Load_Car()
        {
            var id = User.OnlinePerson;
            DataTable dataTable = mainWindow.Select($"Select Cars.Id[Cd], Garage.Id[Id], Cars.Name[Название тc], Cars.Cost[Стоимость в автосалоне] FROM [dbo].[Cars] Inner join [dbo].[Garage] on Cars.Id = Garage.car_id Where(Garage.user_id = {id})");
            List_Cars.ItemsSource = dataTable.DefaultView;
            DataTable dataTable1 = mainWindow.Select($"SELECT Deals.Id, Cars.Id[Cd], Deals.Car_id[IdCar], Cars.Name[Название тc], Deals.Cost[Цена], Cars.Cost[Стоимость в автосалоне], users.login[От пользователя] FROM Deals INNER JOIN users ON Deals.Seller_id = users.Id Or Deals.Customer_id = users.Id INNER JOIN Garage ON Deals.Car_id = Garage.Id AND users.Id = Garage.user_id INNER JOIN Cars ON Garage.car_id = Cars.Id Where Deals.Customer_id = {User.OnlinePerson} and Deals.Status is NULL");
            if(dataTable1.Rows.Count>0)
            {
                Notification.Badge = dataTable1.Rows.Count;
            }
        }
        private void getUserName()
        {
            string name = "";
            var id = User.OnlinePerson;
            DataTable dataTable = mainWindow.Select($"Select login from [dbo].[users] where Id={id}");
            foreach (DataRow row in dataTable.Rows)
            {
                var Ids = row.ItemArray;
                foreach (string names in Ids)
                    name = names;
            }
            user_name.Content = name;
        }
        private void getBalance()
        {
            string bal = "";
            var id = User.OnlinePerson;
            DataTable dataTable = mainWindow.Select($"Select balance from [dbo].[users] where Id={id}");
            foreach (DataRow row in dataTable.Rows)
            {
                var Ids = row.ItemArray;
                foreach (object names in Ids)
                    bal = names.ToString();
            }
            balance.Content = bal + "&";
        }
        private void Cars_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.cars);
        }
        private void Add_Money_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.balance);
        }
        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            if (List_Cars.SelectedItem != null)
            {
                string main = ((DataRowView)List_Cars.SelectedItems[0]).Row["Стоимость в автосалоне"].ToString().Replace('.', ',');
                decimal Cost_o = Convert.ToDecimal(main) * (decimal)0.9;
                string Cost = Cost_o.ToString().Replace(',', '.');
                string name = ((DataRowView)List_Cars.SelectedItems[0]).Row["Название тc"].ToString();
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите продать транспортное средство {name} за {Cost}& ?\nВсе сделки для этого транспортного средства будут отменены", "Подветрдите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = User.OnlinePerson;
                    string ID = ((DataRowView)List_Cars.SelectedItems[0]).Row["Id"].ToString();
                    DataTable dataTable0 = mainWindow.Select($"Delete from [dbo].[Deals] where car_id={ID}");
                    DataTable dataTable = mainWindow.Select($"Delete from [dbo].[Garage] where Id={ID}");
                    DataTable dataTable1 = mainWindow.Select($"Update [dbo].[users] set balance = balance+{Cost} where Id = {id}");
                    Update();
                }
            }
            else MessageBox.Show("Выберите т/c", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void Update()
        {
            Load_Car();
            getBalance();
            getUserName();
        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Id" || propertyDescriptor.DisplayName == "Cd")
            {
                e.Cancel = true;
            }
        }
        private void ToSell_Click(object sender, RoutedEventArgs e)
        {
            if (List_Cars.SelectedItem != null)
            {
                User.car_id = ((DataRowView)List_Cars.SelectedItems[0]).Row["Id"].ToString();
                mainWindow.OpenPage(MainWindow.pages.sell);
            }
            else MessageBox.Show("Выберите т/c", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ToDeals_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.deals);
        }
        private void List_Cars_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (List_Cars.SelectedItem != null)
            {
                string ID = ((DataRowView)List_Cars.SelectedItems[0]).Row["Cd"].ToString();
                if (ID != "1" && ID != "3" && ID != "4" && ID != "5" && ID != "6")
                {

                    string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KP;Integrated Security=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = $"SELECT * FROM Cars Where Id={ID}";
                        SqlCommand command = new SqlCommand(sql, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            KP.Cars.data = (byte[])reader.GetValue(5);
                        }
                    }
                    User.garage = 0;
                    mainWindow.OpenPage(MainWindow.pages.image);

                }
                else
                {
                    MessageBox.Show("У данного автомобиля недоступна картинка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите т/c", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
