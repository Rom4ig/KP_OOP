using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        MainWindow mainWindow;
        public Add(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.cars);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //    byte[] imageData;
            //    var dlg = new OpenFileDialog { Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*" };
            //    if (dlg.ShowDialog() != true) return;
            //    Cars.Imm = dlg.FileName;
            //    MessageBox.Show(dlg.FileName);
            //    using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open))
            //    {
            //        imageData = new byte[fs.Length];
            //        fs.Read(imageData, 0, imageData.Length);
            //    }
            //    string kek = Convert.ToString(imageData);
            //    MessageBox.Show(kek);
            //    mainWindow.Select($"INSERT INTO [dbo].[Cars](Name,Cost,Description,Img,Count) VALUES ('{Car_Name.Text}', '{Car_Price.Text}', '{Car_Description.Text}', {kek}, {Car_Count.Text})");
            //mainWindow.Select($"INSERT INTO [dbo].[Cars](Name,Cost,Description,Img) VALUES ('{Car_Name.Text}', '{Car_Price.Text}', '{Car_Description.Text}', '{Cars.Imm}')");
            //mainWindow.Select($"INSERT INTO [dbo].[Cars](Name,Cost,Description,Count) VALUES ('{Car_Name.Text}', '{Car_Price.Text}', '{Car_Description.Text}', {Car_Count.Text})");
            //MessageBox.Show("Транспортное средство добавлено", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);



            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KP;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Cars VALUES (@Name, @Cost, @Description, @Count, @Img)";
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 40);
                command.Parameters.Add("@Cost", SqlDbType.Money);
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 10000);
                command.Parameters.Add("@Count", SqlDbType.Int);
                command.Parameters.Add("@Img", SqlDbType.Image, 1000000);
                // путь к файлу для загрузки
                // заголовок файла
                // получаем короткое имя файла для сохранения в бд
                var dlg = new OpenFileDialog { Filter = "All files (*.*)|*.*" };
                if (dlg.ShowDialog() != true) return;

                // массив для хранения бинарных данных файла
                byte[] imageData;
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                // передаем данные в команду через параметры
                command.Parameters["@Name"].Value = Car_Name.Text;
                command.Parameters["@Cost"].Value = Car_Price.Text;
                command.Parameters["@Description"].Value = Car_Description.Text;
                command.Parameters["@Count"].Value = Car_Count.Text;
                command.Parameters["@Img"].Value = imageData;
                command.ExecuteNonQuery();
            }
            MessageBox.Show("Авто добавлено", "Информация", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Car_Name.Text = "";
            Car_Price.Text = "";
            Car_Description.Text = "";
            Car_Count.Text = "";
        }

    }

}
