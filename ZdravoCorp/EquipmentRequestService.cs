using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using System.Timers;
using System.Windows;

namespace ZdravoCorp
{
    public class EquipmentRequestService
    {
        private static Timer updateTimer;
        ManagerEquipmentWindow parentWindow = null;

        public EquipmentRequestService() { 
            updateTimer = new Timer();
            SetTimer();
        }
        public void SetParentWindow(ref ManagerEquipmentWindow parentWindow)
        {
            this.parentWindow = parentWindow;
        }
        private void SetTimer()
        {
            updateTimer = new System.Timers.Timer(60000);
            updateTimer.Elapsed += this.OnTimedEvent;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            FullFillEquipmentRequests();
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();
            ((App)Application.Current).EquipmentRequestRepository.Dump();
            ((App)Application.Current).inventory.Dump();
        }
        public void RequestEquipment(Equipment equipment, int amount)
        {
            EquipmentRequest equipmentRequest = new EquipmentRequest(equipment, amount);
            ((App)Application.Current).EquipmentRequestRepository.Add(equipmentRequest);
            this.FullFillEquipmentRequests();
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();
            ((App)Application.Current).EquipmentRequestRepository.Dump();
            ((App)Application.Current).inventory.Dump();
        }

        public void FullFillEquipmentRequests()
        {
            foreach (var pair in ((App)Application.Current).EquipmentRequestRepository._requests)
            {
                if (pair.Value.IsExpired())
                    FullfillRequest(pair.Value);
            }
            updateParentWindow();

        }
        private void updateParentWindow()
        {
            ((App)Application.Current).GetRoomEquipmentView().GenerateView();

            if (parentWindow != null)
                ((App)Application.Current).Dispatcher.Invoke(() =>
                {
                    parentWindow.Data_Grid.ItemsSource = ((App)Application.Current).GetRoomEquipmentView().Itemlist;
                    parentWindow.Data_Grid.Items.Refresh();
                });
        }
        public void FullfillRequest(EquipmentRequest request)
        {
            ((App)Application.Current).EquipmentRequestRepository.Remove(request);
            ((App)Application.Current).EquipmentRequestRepository.Dump();
            ((App)Application.Current).inventory.AddFromRequest(request);

        }

    }
}