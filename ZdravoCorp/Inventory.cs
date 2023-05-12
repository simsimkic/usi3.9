using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ZdravoCorp
{
    public class Inventory
    {
        public Dictionary<int, InventoryItem> items;

        string filePath = "../../../Inventory.csv";
        int curId = 0;
        public Inventory(Dictionary<int, InventoryItem> items)
        {
            this.items = items;
        }
        public Inventory() {
            this.items = new Dictionary<int, InventoryItem>();
            this.ReadInventory();
            FillInventoryWithEmptyItems();
        }
        public void Add(InventoryItem item)
        {
            if(!SimplifyInventory(item))
                items[item.id] = item;
            else
            {
                FillInventoryWithEmptyItems();
            }
        }
        public void AddWithoutSimplifying(InventoryItem item)
        {
            items[item.id] = item;
        }
        public void Remove(InventoryItem item)
        {
            if(items.ContainsKey(item.id))
            {
                items.Remove(item.id);
            }
        }
        public Dictionary<int, InventoryItem> GetItems()
        {
            return items;
        }
        public void ReadInventory()
        {
            StreamReader reader;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                parseLine(ref reader);
            }
            reader.Close();
        }
        private void parseLine(ref StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(",");

            App app = (App)(App.Current);
            
            Add(new InventoryItem(Convert.ToInt32(values[0]), app.EquipmentRepository.Get(Convert.ToInt32(values[1])), 
                app.RoomRepository.Get(Convert.ToInt32(values[2])),
                Convert.ToInt32(values[3]), Convert.ToInt32(values[4])));
            curId = Convert.ToInt32(values[3]);
        }
        public void AddFromRequest(EquipmentRequest request)
        {
            InventoryItem inventoryItem = GenerateInventoryItem(request);
            Add(inventoryItem);
        }
        public void AddFromRequest(EquipmentMovementRequest request)
        {
            InventoryItem inventoryItem = GenerateInventoryItem(request);
            Add(inventoryItem);
        }
        public void RemoveFromRequest(EquipmentMovementRequest request)
        {
            InventoryItem inventoryItem = GenerateRemovalItem(request);
            Add(inventoryItem);
        }
        public InventoryItem GenerateRemovalItem(EquipmentMovementRequest request)
        {
            return new InventoryItem(-request.amount, request.Equipment, request.source.Room, ++curId);
        }
        private InventoryItem GenerateInventoryItem(EquipmentRequest request)
        {
            return new InventoryItem(request.amount, request.Equipment, ((App)Application.Current).RoomRepository.GetDefaultWarehouse(), ++curId);
        }
        private InventoryItem GenerateInventoryItem(EquipmentMovementRequest request)
        {
            return new InventoryItem(request.amount, request.Equipment, request.destination, ++curId);
        }
        public void Dump()
        {
            StreamWriter writer;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            writer = new StreamWriter(filePath);
            foreach (InventoryItem inventoryItem in items.Values)
            {
                if (inventoryItem.amount != 0)
                {
                    writer.WriteLine(inventoryItem.ToString());
                }
            }
            writer.Close();
        }
        public void FillInventoryWithEmptyItems()
        {
            foreach (Equipment equipment in ((App)Application.Current).EquipmentRepository.equipment.Values) {
                foreach (Room room in ((App)Application.Current).RoomRepository.rooms.Values)
                {

                    InventoryItem inventoryItem = new InventoryItem(0, equipment, room, ++curId);
                    inventoryItem.IsEmpty = true;
                    AddWithoutSimplifying(inventoryItem);
                }
                }
        }
        public bool SimplifyInventory(InventoryItem item)
        {
            bool removed = false;
            foreach (int i in items.Keys) {
                if (items[i].Room.Id == item.Room.Id && items[i].equipment.Id == item.equipment.Id) {
                    items[i].amount += item.amount;
                    items[i].amountUnreserved += item.amountUnreserved;
                    removed = true;
                    break;
                }
            }
            RemoveEmptyKeys();
            return removed;
        }
        public void RemoveEmptyKeys()
        {
            foreach (int i in items.Keys)
            {
                if(items[i].amount == 0)
                {
                    items.Remove(i);
                }
            }
        }

        public string EncodeToCSV()
        {
            StringBuilder res = new StringBuilder();
            foreach (InventoryItem item in items.Values) {
                res.Append(item.amount.ToString());
                res.Append(",");
                res.Append(item.equipment.Id.ToString());
                res.Append(",");
                res.Append(item.Room.Id.ToString());
                res.Append("\n");
            }
            res = new StringBuilder(res.ToString().Trim());
            return res.ToString();
        }
    }
}

