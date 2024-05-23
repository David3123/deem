using System;
using System.Collections.Generic;
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

namespace ttttt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                App.DB = new DB_modle.test123Entities();
                MessageBox.Show("Подключение к бд успешно");
            }
            catch
            {
                MessageBox.Show("Связь не установлена");
                App.Current.Shutdown();
            }

            ShowTable();
        }

        public void ShowTable()
        {
            var spisok = App.DB.User.ToList();

            dgFIO.ItemsSource = spisok;
        }
    }
}
