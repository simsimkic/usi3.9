using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class InventoryItem
    {
        public int amount { get; set; }

        public int amountUnreserved { get; set; }
        public Equipment equipment { get; set; }
        public Room Room { get; set; }
        public int id { get; }
        public bool IsEmpty { get; set; }   
        public InventoryItem(int amount, Equipment equipment, Room room, int id)
        {
            this.amount = amount;
            this.amountUnreserved = amount;
            this.equipment = equipment;
            this.Room = room;
            this.id = id;
            IsEmpty = false;
        }
        public InventoryItem(int amount, Equipment equipment, Room room, int id, int amountUnreserved)
        {
            this.amount = amount;
            this.amountUnreserved = amountUnreserved;
            this.equipment = equipment;
            this.Room = room;
            this.id = id;
            IsEmpty = false;
        }
        public bool Reserve(int amountToReserve)
        {
            if (equipment.dynamic) {
                return true;
            }
            if(amountToReserve > amountUnreserved)
            {
                return false;
            }
            amountUnreserved -= amountToReserve;
            return true;
        }
        public void Unreserve(int amountToUnreserve)
        {

            if (!equipment.dynamic)
            {
                amountUnreserved += amountToUnreserve;
            }
        }
        public override string ToString()
        {
            return amount.ToString() + "," + equipment.Id.ToString() + "," + Room.Id.ToString() + "," + id.ToString() + "," +
                amountUnreserved.ToString();
        }
    }
}
