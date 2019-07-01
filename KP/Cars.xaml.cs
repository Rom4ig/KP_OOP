using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Data.SqlClient;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Cars.xaml
    /// </summary>
    public partial class Cars : Page
    {
        MainWindow mainWindow;
        public static byte[] data = null;
        public Cars(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            if (User.OnlinePerson != 1)
            {
                NewCar.Visibility = Visibility.Hidden;
                deleteButton.Visibility = Visibility.Hidden;
            }
            UpdateDB();
        }
        private void UpdateDB()
        {
            DataTable dataTable = mainWindow.Select("Select Id[Cd], Name[Название тc], Cost[Стоимость], Description[Описание], Count[Наличие] from [dbo].[Cars]");
            List_Cars.ItemsSource = dataTable.DefaultView;
        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Cd")
            {
                e.Cancel = true;
            }
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (List_Cars.SelectedItem != null)
            {
                User.car_id = ((DataRowView)List_Cars.SelectedItems[0]).Row["Cd"].ToString();
                mainWindow.OpenPage(MainWindow.pages.counts);
            }
            else MessageBox.Show("Выберите т/c");
        }

        private void NewCar_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.add);
        }

        private void BuyCar_Click(object sender, RoutedEventArgs e)
        {
            if (List_Cars.SelectedItem != null)
            {
                string count = "0";
                if (((DataRowView)List_Cars.SelectedItems[0]).Row["Наличие"].ToString() != count)
                {
                    string ID = ((DataRowView)List_Cars.SelectedItems[0]).Row["Cd"].ToString();
                    string name = User.OnlinePerson.ToString();
                    string car = ((DataRowView)List_Cars.SelectedItems[0]).Row["Название тc"].ToString();
                    decimal cost = (decimal)((DataRowView)List_Cars.SelectedItems[0]).Row["Стоимость"];
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите приобрести транспортное средство {car} за {cost}& ?", "Подветрдите действие", MessageBoxButton.YesNo);
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
                            mainWindow.Select($"INSERT INTO [dbo].[Garage](user_id,car_id) VALUES ('{name}', '{ID}')");
                            mainWindow.Select($"Update [dbo].[Cars] set Count = Count-1 Where Id={ID}");
                            MessageBox.Show($"Вы приобрели транспортное средство {car}", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            mainWindow.Select($"Update [dbo].[users] set balance = {ost_s} where Id={name}");
                            UpdateDB();
                        }
                        else MessageBox.Show("Недостаточно денег на счету", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else MessageBox.Show("Данного транспортного средства нет в наличии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else MessageBox.Show("Выберите т/с", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Garage_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.garage);
        }

        private void List_Cars_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //string ID = ((DataRowView)List_Cars.SelectedItems[0]).Row["Id"].ToString();
            ////string data = null;
            //byte[] data = null;
            //DataTable balance1 = mainWindow.Select($"Select Img from [dbo].[Cars] where Id={ID}");
            //foreach (DataRow row in balance1.Rows)
            //{
            //    var Ids = row.ItemArray;
            //    foreach (string names in Ids)
            //        data = names;
            //}
            //data = (byte[])bytes;
            //SaveFileDialog dlg = new SaveFileDialog { Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*" };
            //if (dlg.ShowDialog() != true) return;
            //using (FileStream fs = new FileStream(dlg.FileName, FileMode.OpenOrCreate))
            //{
            //    fs.Write(data, 0, data.Length);
            //    MessageBox.Show("+");
            //}
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
                            data = (byte[])reader.GetValue(5);
                        }
                    }
                    User.garage = 2;
                    mainWindow.OpenPage(MainWindow.pages.image);
                }
                else
                {
                    MessageBox.Show("У данного автомобиля недоступна картинка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else MessageBox.Show("Выберите т/с", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
