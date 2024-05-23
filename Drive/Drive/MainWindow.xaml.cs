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

namespace Drive
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //связь с бд
            try
            {
                App.DB = new Model.DBDriveEntities();
                MessageBox.Show("Подключение к бд успешно");
            }
            catch
            {
                MessageBox.Show("Связь не установлена");
                App.Current.Shutdown();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOrders();

            cbSort.SelectedIndex = 0; //изначально выбрано

            List<Model.Subdivision> divisions = App.DB.Subdivision.ToList(); //для фильтра по подразделениям
            Model.Subdivision division = new Model.Subdivision(); // новое подразделение для всех подразделений сразу (как без фильтра)
            division.SubdivisionID = 0;
            division.SubdivisionName = "Все подразделения";
            cbDivision.DisplayMemberPath = "SubdivisionName"; //видимые значения
            cbDivision.SelectedValuePath = "SubdivisionID"; //получаемые значения
            divisions.Insert(0, division);
            cbDivision.ItemsSource = divisions;
            cbDivision.SelectedIndex = 0;
            
            List<Model.Status> statuses = App.DB.Status.ToList();
            Model.Status status = new Model.Status(); // новый статус
            status.StatusID = 0;
            status.StatusName = "Все статусы";
            cbStatus.DisplayMemberPath = "StatusName"; //видимые значения
            cbStatus.SelectedValuePath = "StatusID"; //получаемые значения
            cbStatus.ItemsSource = statuses;
            cbStatus.SelectedIndex = 0;
            statuses.Insert(0, status);
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowOrders();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowOrders();
        }

        public void ShowOrders()
        {
            //получить список всех заявок из БД
            List<Model.Request> requests = App.DB.Request.ToList(); //подключение к ТАБЛИЦЕ бд
            //обработка заявок

            //сортировка
            if (cbSort.SelectedIndex == 1)
            {
                requests = requests.OrderBy(r => r.RequestDateAdd /*по какому полю сортировать заказы*/).ToList();
            }
            else if (cbSort.SelectedIndex == 2)
            {
                requests = requests.OrderByDescending(r => r.RequestDateAdd).ToList();
            }

            //фильтрация по подразделениям
            if (cbDivision.SelectedIndex > 0)
            {
                requests = requests.Where(r => r.RequestSubdivision == (int)cbDivision.SelectedValue).ToList();
            }

            //фильтрация по статусу
            if (cbStatus.SelectedIndex > 0)
            {
                requests = requests.Where(r => r.RequestStatus == (int)cbStatus.SelectedValue).ToList();
            }

            //поиск по фио
            if (tbFullName.Text.Length != 0)
            {
                requests = requests.Where(r => r.Visitor.VisitorFullName.ToLower().Contains(tbFullName.Text.ToLower())).ToList();
            }

            //отобразить список отсортированных заявок в окне
            dgRequest.ItemsSource = requests;
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowOrders();
        }

        private void tbFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowOrders();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            View.AddOrder viewAddOrder = new View.AddOrder();
            this.Hide();
            viewAddOrder.ShowDialog();
            this.ShowDialog();
            ShowOrders();  //обновление списка грида
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(dgRequest.SelectedItem != null)
            {
                View.EditOrder viewEditOrder = new View.EditOrder((Model.Request)dgRequest.SelectedItem);
                this.Hide();
                viewEditOrder.ShowDialog();
                this.ShowDialog();
                ShowOrders();  //обновление списка грида
            }
            else
            {
                MessageBox.Show("Выберите запись");
            }
            
        }

    }
}
