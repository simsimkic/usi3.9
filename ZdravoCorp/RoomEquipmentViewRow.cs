using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class RoomEquipmentViewRow : INotifyPropertyChanged
    {
        public string RoomName { get; set; }
        public string EquipmentName { get; set; }
        public int Amount { get; set; }
        public bool Dynamic { get; set; }
        public string RowColor { get; set; } 
        private InventoryItem inventoryItem;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RoomEquipmentViewRow(InventoryItem inventoryItem, string roomName, string equipmentName, int amount, bool dynamic)
        {
            this.RoomName = roomName;
            this.Amount = amount;
            this.EquipmentName = equipmentName;
            this.Dynamic = dynamic;
            this.inventoryItem = inventoryItem;
            SetRowColor();
        }
        public Equipment GetEquipment() {  return inventoryItem.equipment;  }
        public InventoryItem GetInventoryItem() {  return inventoryItem;  }
        public void SetRowColor()
        {
            if (Amount > 0)
            {
                RowColor = "White";
            }
            else { RowColor = "Red"; }
            OnPropertyChanged("RowColor");
        }
        public bool Belongs(InventoryItem item)
        {
            return this.RoomName == item.Room.name && this.EquipmentName == item.equipment.name;
        }
    }
}
