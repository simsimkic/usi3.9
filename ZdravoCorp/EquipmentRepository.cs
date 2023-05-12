using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class EquipmentRepository
    {

        public Dictionary<int, Equipment> equipment { get; }

        string filePath = "../../../EquipmentRepository.csv";
        public EquipmentRepository() 
        { 
            this.equipment = new Dictionary<int, Equipment>();  
            ReadEquipment();
        }
        public Equipment Get(int id)
        {
            return equipment[id];
        }
        void Add(Equipment equipment)
        {
            this.equipment[equipment.Id] = equipment;
        }
        public void ReadEquipment()
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
        }
        private void parseLine(ref StreamReader reader)
        {
            string line = reader.ReadLine();
            if (line == "" || line == "\n") return;
            string[] values = line.Split(",");
            lineToObject(values);
        }
        private void lineToObject(string[] values)
        {
            Add(new Equipment(values[0], (Equipment.Type)Enum.Parse(typeof(Equipment.Type), values[1]), Convert.ToInt32(values[2]),
                Convert.ToBoolean(values[3])));
        }
        public void Dump() {
            StreamWriter writer;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            writer = new StreamWriter(filePath);
            foreach (Equipment equipment in equipment.Values)
            {
                writer.WriteLine(equipment.ToString());
            }
            writer.Close();
        }

    }
}

