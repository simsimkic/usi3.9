using System;

using System.Linq;
using System.Windows.Threading;
using System.Timers;
using System.Windows;
namespace ZdravoCorp
{
    public class EquipmentMovementRequestService
    {
        int currentAmount = 0;
        InventoryItem currentSource; 
        private static Timer updateTimer;
        ManagerEquipmentWindow parentWindow = null;
        public EquipmentMovementRequestService()
        {
            updateTimer = new Timer();
            SetTimer();
        }

        public void SetParentWindow(ref ManagerEquipmentWindow parentWindow)
        {
            this.parentWindow = parentWindow;
        }
        private void SetTimer()
        {
            updateTimer = new System.Timers.Timer(6000);
            updateTimer.Elapsed += this.OnTimedEvent;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            FullFillEquipmentMovementRequests();
        }
        public bool RequestEquipmentMoved(Room destination, DateTime timeToFullfill)
        {
            if (currentAmount > currentSource.amountUnreserved && !currentSource.equipment.dynamic)
            {
                return false;
            }
            ((App)Application.Current).inventory.items[currentSource.id].Reserve(currentAmount);
            EquipmentMovementRequest EquipmentMovementRequest = new EquipmentMovementRequest(currentSource.equipment, 
                timeToFullfill, currentAmount, currentSource, destination);
            ((App)Application.Current).EquipmentMovementRequestRepository.Add(EquipmentMovementRequest);
            return true;
        }
        public void SetCurrentSource(InventoryItem currentSource)
        {
            this.currentSource = currentSource;
        }
        public void SetCurrentAmount(int currentAmount) {
            this.currentAmount = currentAmount;
        }
        public void FullFillEquipmentMovementRequests()
        {
            foreach (var pair in ((App)Application.Current).EquipmentMovementRequestRepository._requests)
            {
                if (pair.Value.IsExpired())
                    FullfillRequest(pair.Value);
            }
        }
      
        public void FullfillRequest(EquipmentMovementRequest request)
        {
            ((App)Application.Current).EquipmentMovementRequestRepository.Remove(request);
            ((App)Application.Current).inventory.AddFromRequest(request);
            ((App)Application.Current).inventory.RemoveFromRequest(request);
            if (((App)Application.Current).inventory.items.Keys.Contains(request.source.id))
            {
                ((App)Application.Current).inventory.items[request.source.id].Unreserve(request.amount);
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
    }
}

