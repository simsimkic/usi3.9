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

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for ManagerEquipmentWindow.xaml
    /// </summary>
    public partial class ManagerEquipmentWindow : Window
    {
        public ManagerEquipmentWindow()
        {
            InitializeComponent();
            loadDefaultWindowSetup();
        }
        private void loadDefaultWindowSetup()
        {
            Data_Grid.ColumnWidth = 50;
            Data_Grid.RowHeight = 50;
            ((App)App.Current).GetRoomEquipmentView().GenerateView();
            Data_Grid.ItemsSource = ((App)App.Current).GetRoomEquipmentView().Itemlist;
            Data_Grid.MaxColumnWidth = 300;
            Data_Grid.MinColumnWidth = 100;
            Data_Grid.CanUserAddRows = false;
            Data_Grid.CanUserDeleteRows = false;
            Data_Grid.CanUserReorderColumns = false;
            Data_Grid.CanUserResizeRows = false;
            Data_Grid.CanUserSortColumns = false;
            Data_Grid.IsReadOnly = true;
            Data_Grid.SelectedItem = null;
            Filter_Box.IsChecked = true;

        }
        public void RequestEquipment()
        {
            if (!Validate())
            {
                return;
            }
            int amount = Convert.ToInt32(Number_Box.Text);
            Equipment equipment = ((RoomEquipmentViewRow)Data_Grid.SelectedItem).GetEquipment();
            ((App)Application.Current).EquipmentRequestService.RequestEquipment(equipment, amount);
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();
            Data_Grid.ItemsSource = ((App)Application.Current).GetRoomEquipmentView().Itemlist;
        }
        public void RequestMovement()
        {
            if (!Validate() || !ValidateMovementAmount())
            {
                return;
            }
            ((App)Application.Current).EquipmentMovementRequestService.SetCurrentAmount(Convert.ToInt32(Number_Box.Text));
            ((App)Application.Current).EquipmentMovementRequestService.SetCurrentSource(
                ((RoomEquipmentViewRow)Data_Grid.SelectedItem).GetInventoryItem());
            var self = this;
            ((App)Application.Current).EquipmentMovementRequestService.SetParentWindow(ref self);
            EquipmentMovementWindow window = new EquipmentMovementWindow(this);
            window.Date_Picker.IsEnabled = !((RoomEquipmentViewRow)Data_Grid.SelectedItem).GetEquipment().dynamic;
            window.Time_Box.IsEnabled = window.Date_Picker.IsEnabled;
            window.Show();
        }
        private void Btn_Request_Click(object sender, RoutedEventArgs e)
        {

            RequestEquipment();
        }
        private void Btn_Movement_Request_Click(object sender, RoutedEventArgs e)
        {
            RequestMovement();
        }
        private void EnableAmountFilter()
        {
            ((App)Application.Current).GetRoomEquipmentView().EnableFilter();
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();
            Data_Grid.ItemsSource = ((App)Application.Current).GetRoomEquipmentView().Itemlist;
        }
        private void DisableAmountFilter()
        {
            ((App)Application.Current).GetRoomEquipmentView().DisableFilter();
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();
            Data_Grid.ItemsSource = ((App)Application.Current).GetRoomEquipmentView().Itemlist;
        }

        private void Filter_Box_Checked(object sender, RoutedEventArgs e)
        {
            EnableAmountFilter();
        }

        private void Filter_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            DisableAmountFilter();
        }
        private bool Validate()
        {
            return ValidateData_Grid() && ValidateNumber_Box();
        }
        private bool ValidateData_Grid()
        {
            if (Data_Grid.SelectedItem == null) {
                DisplayValidationErrorMessage("Odaberite opremu");
                return false;
            }
            return true;
        }
        private bool ValidateNumber_Box()
        {
            int number;
            if (!int.TryParse(Number_Box.Text, out number)) {
                DisplayValidationErrorMessage("Kolicina mora biti broj");
                return false;
            }
            return true;
        }
        private bool ValidateMovementAmount()
        {
            int number;
            int.TryParse(Number_Box.Text, out number);
            if (number > ((RoomEquipmentViewRow)Data_Grid.SelectedItem).Amount)
            {
                DisplayValidationErrorMessage("Ne mozete premestiti vecu kolicinu opreme iz te sobe nego sto vec postoji");
                return false;
            }
            return true;
        }
        private void DisplayValidationErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        
    }

}
