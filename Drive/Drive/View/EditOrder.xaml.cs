using Drive.Model;
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

namespace Drive.View
{
    /// <summary>
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        private Model.Request workRequest; //создаем объект выбранной заявки с прошлого окна

        public EditOrder(Model.Request editRequest)
        {
            InitializeComponent();

            workRequest = editRequest;

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

            dpDateAdd.SelectedDate = workRequest.RequestDateAdd;
            dpDateStart.SelectedDate = workRequest.RequestDateStart;
            dpDateEnd.SelectedDate = workRequest.RequestDateEnd;
            cbDivision.SelectedValue = workRequest.RequestSubdivision;
            tbPurpose.Text = workRequest.RequestPurpose;
            cbVisitor.SelectedValue = workRequest.RequestVisitor;
            cbStatus.SelectedValue = workRequest.RequestStatus;
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            workRequest.RequestDateAdd = (DateTime)dpDateAdd.SelectedDate;
            workRequest.RequestDateStart = (DateTime)dpDateStart.SelectedDate;
            workRequest.RequestDateEnd = (DateTime)dpDateEnd.SelectedDate;
            workRequest.RequestSubdivision = (int)cbDivision.SelectedValue;
            workRequest.RequestPurpose = tbPurpose.Text;
            workRequest.RequestVisitor = (int)cbVisitor.SelectedValue;
            workRequest.RequestStatus = (int)cbStatus.SelectedValue;

            try
            {
                App.DB.SaveChanges();
                MessageBox.Show("Сохранено");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
                throw;

            }
        }
    }
}
