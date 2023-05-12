using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{

    public class Equipment
    {
        public enum Type
        {
            CHECKUP,
            OPERATION,
            HALLWAY,
            FURNITURE
        }
        public string name { get; set; }
        public Type type { get; set; }
        public int Id { get; set; }
        public bool dynamic { get; set; } = false;
        public Equipment(string name, Type type, int id, bool dynamic)
        {
            this.name = name;
            this.type = type;
            this.Id = id;
            this.dynamic = dynamic;
        }
        public override string ToString()
        {
            return this.name + "," + this.type.ToString() + "," + this.Id.ToString() + "," + this.dynamic.ToString();
        }
    }
}
