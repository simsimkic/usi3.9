using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class EquipmentRequest
    {
        public Equipment Equipment { get; set; }
        public DateTime TimeMade { get; set; }
        int DaysToFullfill { get; set; }
        public int amount { get; set; }

        public EquipmentRequest(DateTime timeMade, Equipment equipment, int daysToFullFill, int amount)
        {
            this.Equipment = equipment;
            this.TimeMade = timeMade;
            this.DaysToFullfill = daysToFullFill;
            this.amount = amount;
        }
        public EquipmentRequest(Equipment equipment, int daysToFullFill, int amount)
        {
            this.Equipment = equipment;
            this.TimeMade = DateTime.Now;
            this.DaysToFullfill = daysToFullFill;
            this.amount = amount;
        }
        public EquipmentRequest(Equipment equipment, int amount)
        {
            this.Equipment = equipment;
            this.TimeMade = DateTime.Now;
            this.DaysToFullfill = 5;
            this.amount = amount;
        }

        
        public bool IsExpired()
        {
            return TimeMade.AddDays(DaysToFullfill) <= DateTime.Now;
        }
        public override string ToString()
        {
            return Equipment.Id.ToString() + "," + DaysToFullfill.ToString() + "," + amount.ToString() + "," + 
                TimeMade.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
   
}