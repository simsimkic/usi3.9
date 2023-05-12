using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class Room
    {
        public enum Type
        {
            ANY,
            OPERATION,
            CHECKUP,
            BEDROOM,
            WAITING,
            WAREHOUSE
        }
        public Type type { get; set; }
        public String name { get; set; }
        public int Id { get; set; }
        public Room(Type type, string name, int id)
        {
            this.type = type;
            this.name = name;
            Id = id;
        }
    }
}
