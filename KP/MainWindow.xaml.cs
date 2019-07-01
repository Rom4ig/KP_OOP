using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using MahApps.Metro.Controls;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(pages.login);
            CommandBinding myBind = new CommandBinding(RCommands.myCommand);
            myBind.Executed += CommandBinding_Executed;//вызвано
            CommandBindings.Add(myBind);//добавление в список команд
        }
        public enum pages
        {
            login,
            register,
            cars,
            add,
            garage,
            balance,
            sell,
            deals,
            image,
            counts
        }
        public void OpenPage(pages pages)
        {
            switch (pages)
            {
                case pages.login:
                    frame.Navigate(new Login(this));
                    break;
                case pages.register:
                    frame.Navigate(new Register(this));
                    break;
                case pages.cars:
                    frame.Navigate(new Cars(this));
                    break;
                case pages.add:
                    frame.Navigate(new Add(this));
                    break;
                case pages.garage:
                    frame.Navigate(new Garage(this));
                    break;
                case pages.balance:
                    frame.Navigate(new Balance(this));
                    break;
                case pages.sell:
                    frame.Navigate(new Sell(this));
                    break;
                case pages.deals:
                    frame.Navigate(new Deals(this));
                    break;
                case pages.image:
                    frame.Navigate(new Images(this));
                    break;
                case pages.counts:
                    frame.Navigate(new Count(this));
                    break;
                default:
                    break;
            }
        }
        public DataTable Select(string selectSQL)
        {  
            DataTable dataTable = new DataTable("dataBase");
            SqlConnection sqlConnection = new SqlConnection(@"server=localhost\SQLExpress;Trusted_Connection=Yes;DataBase=KP;");
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Это приложение разработано на WPF с ипользованием Ado.Net. Разработчик: RN corp.", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
