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

namespace ОООАдмиралтейские_Верфи
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
                App.DB = new Model.DBShipyard();
                MessageBox.Show("Подключение к бд успешно");
            }
            catch
            {
                MessageBox.Show("Связь не установлена");
                App.Current.Shutdown();
            }
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ShowRequest()
        {
            //получить список всех заявок из БД
            List<Model.Request> requests = App.DB.Request.ToList();
            //обработка заявок

            //сортировка
            if(cbSort.SelectedIndex == 1)
            {
                requests = requests.OrderBy(r => r.RequestDateAdd).ToList();
            }
            else
            {
                requests = requests.OrderByDescending(r => r.RequestDateAdd).ToList();
            }

            //фильтрация по подразделениям
            if(cbDivision.SelectedIndex > 0) 
            { 
                requests = requests.Where(r=>r.RequestSubdivision == (int)cbDivision.SelectedValue).ToList();
            }

            //фильтрация по статусу
            if (cbStatus.SelectedIndex > 0)
            {
                requests = requests.Where(r => r.RequestStatus == (int)cbStatus.SelectedValue).ToList();
            }

            //поиск по фио
            if(tbFullName.Text.Length != 0)
            {
                requests = requests.Where(r => r.Visitor.VisitorFullName.ToLower().Contains(tbFullName.Text.ToLower())).ToList();
            }

            //отобразить список заявок в окне
            dgRequest.ItemsSource = requests;

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ShowRequest();

            cbSort.SelectedIndex = 0;

            List<Model.Subdivision> divisions = App.DB.Subdivision.ToList();
            Model.Subdivision division = new Model.Subdivision(); // новое подразделение
            division.SubdivisionID = 0;
            division.SubdivisionName = "Все подразделения";
            cbDivision.DisplayMemberPath = "SubdivisionName"; //видимые значения
            cbDivision.SelectedValuePath = "SubdivisionID"; //получаемые значения
            cbDivision.ItemsSource = divisions;
            cbDivision.SelectedIndex = 0;
            divisions.Insert(0, division);



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

        /// <summary>
        /// Сортировка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowRequest();
        }

        /// <summary>
        /// фильтрация по подразделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowRequest();
        }

        /// <summary>
        /// фильтрация по статусу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowRequest();
        }

        /// <summary>
        /// фильтрация по имени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowRequest();
        }

        /// <summary>
        /// Регистрация заявки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            View.ViewRequest viewRequest = new View.ViewRequest();
            this.Hide();
            viewRequest.ShowDialog();
            this.ShowDialog();
        }
    }
}
