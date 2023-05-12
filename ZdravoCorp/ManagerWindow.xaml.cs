using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>

    public partial class ManagerWindow : Window
    {
        private LoginWindow loginWindow;
        private ManagerEquipmentWindow managerEquipmentWindow;
        public ManagerWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            loadDefaultWindowSetup();

            this.loginWindow = loginWindow;
            this.managerEquipmentWindow = new ManagerEquipmentWindow();
        }
        private void loadDefaultWindowSetup()
        {
            Data_Grid.ColumnWidth = 50;
            Data_Grid.RowHeight = 50;
            Data_Grid.ItemsSource = ((App)App.Current).GetInventoryView().rows;
            Data_Grid.MaxColumnWidth = 300;
            Data_Grid.MinColumnWidth = 100;
            Data_Grid.CanUserAddRows = false;
            Data_Grid.CanUserDeleteRows = false;
            Data_Grid.CanUserReorderColumns = false;
            Data_Grid.CanUserResizeRows = false;
            Data_Grid.CanUserSortColumns = false;
            Data_Grid.IsReadOnly = true;
            Amount_Box.Items.Add("Bilo koja kolicina");
            Amount_Box.Items.Add("Nema Na stanju");
            Amount_Box.Items.Add("0 - 10");
            Amount_Box.Items.Add("10+");
            Amount_Box.SelectedIndex = 0;

            foreach (Room.Type t in Enum.GetValues(typeof(Room.Type)))
            {
                Room_Type_Box.Items.Add(t);
            }
            Room_Type_Box.SelectedIndex = 0;
        }
        private void ApplyFilters() {
            ((App)App.Current).GetInventoryView().GenerateView((bool)Not_Warehouse_Checkbox.IsChecked);
            ((App)App.Current).GetInventoryView().ApplyFilters();
            if (Data_Grid == null) {
                return;
            }
            Data_Grid.ItemsSource = ((App)App.Current).GetInventoryView().Itemlist;
        }
        
        private void ApplyAmountFilter() {
            ((App)App.Current).GetInventoryView().ApplyAmountFilter(Amount_Box.SelectedIndex);
            ApplyFilters();
        }
        private void ApplyTextFilter() {
            ((App)App.Current).GetInventoryView().ApplyTextFilter(Filter_Box.Text.ToUpper());
            ApplyFilters();
        }
        private void ApplyTypeFilter() {
            ((App)App.Current).GetInventoryView().ApplyTypeFilter((Room.Type)Room_Type_Box.SelectedItem);
            ApplyFilters();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyTextFilter();
        }
        private void AmoutBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyAmountFilter();
        }
        private void RoomTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ApplyTypeFilter();
        }

        private void NotWarehouseCheckBox_IsEnabledChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loginWindow.Visibility = Visibility.Visible;
            loginWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            managerEquipmentWindow = new ManagerEquipmentWindow();
            managerEquipmentWindow.Show();
        }
    }
}
