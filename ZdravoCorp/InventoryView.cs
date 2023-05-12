using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Data;
using System.ComponentModel;

namespace ZdravoCorp
{
    public class InventoryView
    {
        Dictionary<string, Predicate<InventoryViewRow>> filters = new Dictionary<string, Predicate<InventoryViewRow>>();
        public ICollectionView Itemlist {get; set;}

        Inventory inventory { get; set; }
        public ObservableCollection<InventoryViewRow> rows { get; set; }
        public CollectionViewSource source { get; set; }

        public InventoryView(Inventory inventory)
        {
            this.inventory = inventory;
            GenerateView();
            source = new CollectionViewSource() { Source = rows };
            Itemlist = source.View;
        }
        public void ApplyTextFilter(string filter)
        {
            filters["text"] = new Predicate<InventoryViewRow>((item => ((InventoryViewRow)item).Name.Contains(filter) ||
            ((InventoryViewRow)item).Type.ToString().Contains(filter) ||
            ((InventoryViewRow)item).Rooms.ToString().Contains(filter)));
        }
        public void ApplyFilters() {
            Itemlist.Filter = AggregatedFilter;
        }
        public bool AggregatedFilter(object row)
        {
            return filters.Values
            .Aggregate(true,
                (prevValue, predicate) => prevValue && predicate((InventoryViewRow)row));
        }
        public void ApplyTypeFilter(Room.Type type)
        {
            if(type == Room.Type.ANY)
            {
                filters["type"] = new Predicate<InventoryViewRow>((item => (true)));
                return;
            }
            filters["type"] =  new Predicate<InventoryViewRow>(item => ((InventoryViewRow)item).Rooms.Contains(type.ToString()));
        }
        public void ApplyAmountFilter(int selectedIndex) {
            Predicate<InventoryViewRow> amountFilter;
            switch (selectedIndex)
            {
                case 0:
                    amountFilter = new Predicate<InventoryViewRow>(item => true);
                    break;
                case 1:
                    amountFilter = new Predicate<InventoryViewRow>(item => ((InventoryViewRow)item).Amount == 0);
                    break;
                case 2:
                    amountFilter = new Predicate<InventoryViewRow>(item => ((InventoryViewRow)item).Amount > 0 && ((InventoryViewRow)item).Amount <= 10);
                    break;
                case 3:
                    amountFilter = new Predicate<InventoryViewRow>(item => ((InventoryViewRow)item).Amount > 10);
                    break;
                default:
                    return;
            }
            filters["amount"] = amountFilter;
        }
        public ObservableCollection<InventoryViewRow> GenerateView(bool showingEquipmentInWareHouse = true)
        {
            ObservableCollection<InventoryViewRow> view = new ObservableCollection<InventoryViewRow>();
            foreach (var pair in inventory.GetItems())
            {
                InventoryItem item = pair.Value;
                if (showingEquipmentInWareHouse && item.Room.type == Room.Type.WAREHOUSE || item.IsEmpty)
                {
                    continue;
                }
                if (view.Count == 0)
                {
                    AddFromInventoryItem(ref view, item);
                    view[0].Rooms = item.Room.type.ToString();
                    continue;
                }
                for (int i = 0; i < view.Count; i++)
                { 
                    if (view[i].Name == item.equipment.name)
                    {
                        view[i].Amount += item.amount;      
                        if (!view[i].Rooms.Contains(item.Room.type.ToString()) && view[i].Amount != 0)
                        {

                            view[i].Rooms += "\n" + item.Room.type.ToString();
                        }
                        break;
                    }
                    if (i == view.Count - 1)
                    {

                        AddFromInventoryItem(ref view, item);
                        view[view.Count - 1].Rooms = item.Room.type.ToString();
                        break;
                    }
                }  
            }
            setSource(view);
            return view;
        }

        private void setSource(ObservableCollection<InventoryViewRow> view) {
            rows = view;
            source = new CollectionViewSource() { Source = rows };
            Itemlist = source.View;
        }
        public bool Filter(InventoryItem item, string filter_string)
        {
            if (filter_string == null || filter_string == "")
            {
                return true;
            }
            if (item.equipment.name.Contains(filter_string)) 
            {
                return true;
            }
            if (item.equipment.type.ToString().Contains(filter_string))
            {
                return true;
            }
            return false;
        }
        public InventoryViewRow GenerateRow(InventoryItem item)
        {
            return new InventoryViewRow(item.equipment.name, item.amount, item.equipment.type.ToString());
        }
        private void AddFromInventoryItem(ref ObservableCollection<InventoryViewRow> view, InventoryItem item) {
            InventoryViewRow newInventoryViewRow = GenerateRow(item);
            view.Add(newInventoryViewRow);
        }
    }
}
