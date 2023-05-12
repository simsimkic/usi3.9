using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for EquipmentMovementWindow.xaml
    /// </summary>
    public partial class EquipmentMovementWindow : Window
    {
        private ManagerEquipmentWindow managerEquipmentWindow;
        public EquipmentMovementWindow(ManagerEquipmentWindow managerEquipmentWindow)
        {
            InitializeComponent();
            Data_Grid.ItemsSource = ((App)Application.Current).RoomRepository.GetRooms().Values;
           
            this.managerEquipmentWindow = managerEquipmentWindow;
        }
        public void SetManagerEquipmentWindow(ManagerEquipmentWindow managerEquipmentWindow)
        {
            this.managerEquipmentWindow = (ManagerEquipmentWindow)managerEquipmentWindow;
        }
        public void RequestMovement()
        {
            DateTime dateToBeExecuted;
            if (!Date_Picker.IsEnabled)
            {
                dateToBeExecuted = DateTime.Now;
            }
            else
            {
                dateToBeExecuted = Date_Picker.SelectedDate.Value.Date.Add(TimeSpan.ParseExact(Time_Box.Text.Trim(), "hh\\:mm\\:ss", null, TimeSpanStyles.None));
            }
            if(((App)Application.Current).EquipmentMovementRequestService.RequestEquipmentMoved((Room)Data_Grid.SelectedItem,
                dateToBeExecuted)) {
                return;
            }
            MessageBox.Show("Oprema je rezervisana! Sacekajte da se postojeci zahtev za premestanje ispuni, ili narucite jos.");
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                return;
            }

            RequestMovement();
            ((App)Application.Current).EquipmentMovementRequestService.FullFillEquipmentMovementRequests();
            ((App)Application.Current).EquipmentMovementRequestRepository.Dump();
            ((App)Application.Current).inventory.Dump();
            ((App)Application.Current).EquipmentRepository.Dump();
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();
            managerEquipmentWindow.Data_Grid.ItemsSource = ((App)Application.Current).GetRoomEquipmentView().Itemlist;
            this.Close();
        }

        private bool Validate()
        {
            return ValidateData_Grid() && ValidateDate_Picker() && ValidateText_Box();
        }
        private bool ValidateText_Box()
        {
            if (!Time_Box.IsEnabled)
            {
                return true;
            }
            TimeSpan t;
            if (!TimeSpan.TryParseExact(Time_Box.Text.Trim(), "hh\\:mm\\:ss", null, TimeSpanStyles.None, out t))
            {
                DisplayValidationErrorMessage("Odaberite validno vreme (hh:mm:ss)");
                return false;
            }
            return true;
        }
        private bool ValidateData_Grid()
        {
            if (Data_Grid.SelectedItem == null)
            {
                DisplayValidationErrorMessage("Odaberite prostoriju");
                return false;
            }
            return true;
        }
        private bool ValidateDate_Picker()
        {
            if (!Date_Picker.IsEnabled)
            {
                return true;
            }
            if (Date_Picker.SelectedDate == null)
            {
                DisplayValidationErrorMessage("Odaberite datum");
                return false;
            }
            if (Date_Picker.SelectedDate.Value.Date < DateTime.Now.AddDays(-1))
            {
                DisplayValidationErrorMessage("Odaberite datum u buducnosti, ili danasnji");
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
