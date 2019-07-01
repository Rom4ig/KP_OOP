using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для Image.xaml
    /// </summary>
    public partial class Images : Page
    {
        MainWindow mainWindow;
        public Images(MainWindow _mainWindow)
        {
            InitializeComponent();
            //Uri uri = new Uri($@"{Cars.Imm.Replace(@"\", @"/")}", UriKind.RelativeOrAbsolute);
            //BitmapImage bm = new BitmapImage(uri);
            //File.Delete($@"{Cars.Imm}");
            //CarImg.Source = bm;
            //CarImg.Source = BitmapFrame.Create(new Uri($@"{Cars.Imm}"));
            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            //image.CacheOption = BitmapCacheOption.OnLoad;
            //image.UriSource = new Uri($@"{Cars.Imm}");
            //image.EndInit();
            //CarImg.Source = image;
            //image.Freeze();
            //ImageSourceConverter converter = new ImageSourceConverter();
            //ImageSource imageSource = (ImageSource)converter.ConvertFromString($@"{Cars.Imm}");
            //CarImg.Source = imageSource;
            CarImg.Source = LoadImage(Cars.data);
            //File.Delete($@"{Cars.Imm}");
            mainWindow = _mainWindow;
        }
        private void BackP_Click(object sender, RoutedEventArgs e)
        {
            switch (User.garage)
            {
                case 0:
                    mainWindow.OpenPage(MainWindow.pages.garage);
                    break;
                case 1:
                    mainWindow.OpenPage(MainWindow.pages.deals);
                    break;
                default:
                    mainWindow.OpenPage(MainWindow.pages.cars);
                    break;
            }
        }
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
