using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Deals.xaml
    /// </summary>
    public partial class Deals : Page
    {
        MainWindow mainWindow;
        public Deals(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            Refresh();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Id" || propertyDescriptor.DisplayName == "IdCar" || propertyDescriptor.DisplayName=="Cd")
            {
                e.Cancel = true;
            }
        }
        private void Refresh()
        {
            DataTable dataTable = mainWindow.Select($"SELECT Deals.Id, Cars.Id[Cd], Deals.Car_id[IdCar], Cars.Name[Название тc], Deals.Cost[Цена], Cars.Cost[Стоимость в автосалоне], users.login[От пользователя] FROM Deals INNER JOIN users ON Deals.Seller_id = users.Id Or Deals.Customer_id = users.Id INNER JOIN Garage ON Deals.Car_id = Garage.Id AND users.Id = Garage.user_id INNER JOIN Cars ON Garage.car_id = Cars.Id Where Deals.Customer_id = {User.OnlinePerson} and Deals.Status is NULL");
            List_Deals.ItemsSource = dataTable.DefaultView;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.garage);
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (List_Deals.SelectedItem != null)
            {
                string ID = ((DataRowView)List_Deals.SelectedItems[0]).Row["Id"].ToString();
                string name = User.OnlinePerson.ToString();
                string car = ((DataRowView)List_Deals.SelectedItems[0]).Row["Название тc"].ToString();
                string seller = ((DataRowView)List_Deals.SelectedItems[0]).Row["От пользователя"].ToString();
                decimal cost = (decimal)((DataRowView)List_Deals.SelectedItems[0]).Row["Цена"];
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите приобрести транспортное средство {car} за {cost}& у пользователя {seller} ?", "Подветрдите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    decimal balance = 0;
                    DataTable balance1 = mainWindow.Select($"Select balance from [dbo].[users] where Id={name}");
                    foreach (DataRow row in balance1.Rows)
                    {
                        var Ids = row.ItemArray;
                        foreach (object names in Ids)
                            balance = (decimal)(names);
                    }
                    decimal ost = balance - cost;
                    string ost_s = ost.ToString().Replace(',', '.');
                    if (ost > 0)
                    {
                        string IdCar = ((DataRowView)List_Deals.SelectedItems[0]).Row["IdCar"].ToString();
                        mainWindow.Select($"Update [dbo].[Deals] set Status = 1 where Id={ID}");
                        mainWindow.Select($"Update [dbo].[Garage] set user_id = {User.OnlinePerson} where Id={IdCar}");
                        mainWindow.Select($"Update [dbo].[users] set balance = {ost_s} where Id={name}");
                        mainWindow.Select($"Update [dbo].[users] set balance = balance+{cost.ToString().Replace(',', '.')} where login='{seller}'");
                        MessageBox.Show($"Вы приобрели транспортное средство {car}", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        Refresh();
                    }
                    else MessageBox.Show("Недостаточно денег на счету", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else MessageBox.Show("Выберите т/c", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Cencel_Click(object sender, RoutedEventArgs e)
        {
            if (List_Deals.SelectedItem != null)
            {
                string ID = ((DataRowView)List_Deals.SelectedItems[0]).Row["Id"].ToString();
                string name = User.OnlinePerson.ToString();
                string car = ((DataRowView)List_Deals.SelectedItems[0]).Row["Название тc"].ToString();
                string seller = ((DataRowView)List_Deals.SelectedItems[0]).Row["От пользователя"].ToString();
                decimal cost = (decimal)((DataRowView)List_Deals.SelectedItems[0]).Row["Цена"];
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите отказаться от приобретения транспортного средства {car} за {cost}& у пользователя {seller} ?", "Подветрдите действие", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    mainWindow.Select($"Update [dbo].[Deals] set Status = 0 where Id={ID}");
                    MessageBox.Show("Вы отказались от сделки", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Refresh();
                }
            }
            else MessageBox.Show("Выберите т/с", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void List_Deals_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (List_Deals.SelectedItem != null)
            {
                string ID = ((DataRowView)List_Deals.SelectedItems[0]).Row["Cd"].ToString();
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
                    User.garage = 1;
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
