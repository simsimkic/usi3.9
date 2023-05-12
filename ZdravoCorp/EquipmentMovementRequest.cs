using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class EquipmentMovementRequest
    {
        public Equipment Equipment { get; set; }
        public DateTime TimeMade { get; set; }
        public DateTime TimeToFullfill { get; set; }
        public int amount { get; set; }
        public InventoryItem source { get; set; }
        public Room destination { get; set; }

       
        public EquipmentMovementRequest(Equipment equipment, DateTime timeToFullfill, int amount, InventoryItem source, Room destination)
        {
            this.Equipment = equipment;
            this.TimeMade = DateTime.Now;
            this.TimeToFullfill = timeToFullfill;
            this.amount = amount;
            this.source = source;
            this.destination = destination;
        }
        public EquipmentMovementRequest(DateTime timeMade, Equipment equipment, DateTime timeToFullfill, int amount, InventoryItem source, Room destination)
        {
            this.Equipment = equipment;
            this.TimeMade = timeMade;
            this.TimeToFullfill = timeToFullfill;
            this.amount = amount;
            this.source = source;
            this.destination = destination;
        }
        public bool IsExpired()
        {
            return TimeToFullfill <= DateTime.Now || Equipment.dynamic;
        }
        public override string ToString()
        {
            return Equipment.Id.ToString() + "," + TimeToFullfill.ToString("dd-MM-yyyy HH:mm:ss") + "," + amount.ToString() + "," +
                source.id.ToString() + ", "+ destination.Id.ToString() + "," +TimeMade.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}
