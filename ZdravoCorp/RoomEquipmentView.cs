using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data;


namespace ZdravoCorp
{
    
    public class RoomEquipmentView
    {
        private Inventory inventory;
        public ICollectionView Itemlist { get; set; }
        private int filterAmount = 5;
        public RoomEquipmentView(Inventory inventory)
        {
            this.inventory = inventory;
            GenerateView();
        }
        public void GenerateView()
        {
            ObservableCollection<RoomEquipmentViewRow> view = new ObservableCollection<RoomEquipmentViewRow>();
            foreach (var pair in inventory.GetItems())
            {
                InventoryItem item = pair.Value;
                
                if (view.Count == 0)
                {
                    RoomEquipmentViewRow newInventoryViewRow = generateRow(item);
                    view.Add(newInventoryViewRow);

                    continue;
                }
                for (int i = 0; i < view.Count; i++)
                {

                    if (view[i].Belongs(item))
                    {
                        view[i].Amount += item.amount;
                        view[i].SetRowColor();
                        break;
                    }
                    if (i == view.Count - 1)
                    {

                        RoomEquipmentViewRow newInventoryViewRow = generateRow(item);
                        view.Add(newInventoryViewRow);
                        break;
                    }
                }

            }
            Itemlist = new CollectionViewSource() { Source = ApplyFilter(view) }.View;
        }
        private RoomEquipmentViewRow generateRow(InventoryItem item)
        {
            return new RoomEquipmentViewRow(item, item.Room.name,
                        item.equipment.name, item.amount, item.equipment.dynamic);
        }
        public void EnableFilter() {
            filterAmount = 5;
        }
        public void DisableFilter()
        {
            filterAmount = int.MaxValue;
        }
        public ObservableCollection<RoomEquipmentViewRow> ApplyFilter(ObservableCollection<RoomEquipmentViewRow> view)
        {

            for (int i = 0; i < view.Count; i++)
            {
                if (view[i].Amount > filterAmount)
                {
                    view.Remove(view[i]);
                    i--;
                }
            }
            return view;
        }
    }
}
