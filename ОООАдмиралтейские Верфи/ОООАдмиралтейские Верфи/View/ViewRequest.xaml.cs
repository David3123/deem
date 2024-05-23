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
using System.Windows.Shapes;

namespace ОООАдмиралтейские_Верфи.View
{
    /// <summary>
    /// Логика взаимодействия для ViewRequest.xaml
    /// </summary>
    public partial class ViewRequest : Window
    {
        public ViewRequest()
        {
            InitializeComponent();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDateAdd.DisplayDateStart = DateTime.Now.Date;
            dpDateEnd.DisplayDateStart = DateTime.Now.Date;
            dpDateStart.DisplayDateStart = DateTime.Now.Date;

            List<Model.Subdivision> divisions = App.DB.Subdivision.ToList();
            cbDivision.DisplayMemberPath = "SubdivisionName"; //видимые значения
            cbDivision.SelectedValuePath = "SubdivisionID"; //получаемые значения
            cbDivision.ItemsSource = divisions;
            cbDivision.SelectedIndex = 0;

            List<Model.Status> statuses = App.DB.Status.ToList();
            cbStatus.DisplayMemberPath = "StatusName"; //видимые значения
            cbStatus.SelectedValuePath = "StatusID"; //получаемые значения
            cbStatus.ItemsSource = statuses;
            cbStatus.SelectedIndex = 0;

            List<Model.Visitor> visitors = App.DB.Visitor.ToList();
            cbVisitor.DisplayMemberPath = "VisitorFullName"; //видимые значения
            cbVisitor.SelectedValuePath = "VisitorID"; //получаемые значения
            cbVisitor.ItemsSource = visitors;
            cbVisitor.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Model.Request request = new Model.Request();
            request.RequestDateAdd = (DateTime)dpDateAdd.SelectedDate;
            request.RequestDateStart = (DateTime)dpDateStart.SelectedDate;
            request.RequestDateEnd = (DateTime)dpDateEnd.SelectedDate;
            request.RequestSubdivision = (int)cbDivision.SelectedValue;
            request.RequestPurpose = tbPurpose.Text;
            request.RequestVisitor = (int)cbVisitor.SelectedValue;
            request.RequestStatus = (int)cbStatus.SelectedValue;

            App.DB.Request.Add(request);

            try
            {
                App.DB.SaveChanges();
                MessageBox.Show("Сохранено");
            }
            catch(Exception) 
            {
                MessageBox.Show("Ошибка");
                throw;
                
            }

        }
    }
}
