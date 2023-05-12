using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class InventoryViewRow
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public string Rooms { get; set; }

        public InventoryViewRow(string name, int amount, string type, string rooms = "")
        {
            this.Name = name;
            this.Amount = amount;
            this.Type = type;
            this.Rooms = rooms;
        }
    }
}
