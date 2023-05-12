using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ZdravoCorp
{
    public class EquipmentMovementRequestRepository
    {
        public Dictionary<DateTime, EquipmentMovementRequest> _requests { get; set; }
        string filePath = "../../../EquipmentMovementRequestRepository.csv";

        public EquipmentMovementRequestRepository()
        {
            _requests = new Dictionary<DateTime, EquipmentMovementRequest>();
            Load();
        }
        public void Add(EquipmentMovementRequest request)
        {
            _requests.Add(request.TimeMade, request);
        }
        public EquipmentMovementRequest Get(DateTime key)
        {
            return _requests[key];
        }
        public void Remove(EquipmentMovementRequest request)
        {
            _requests.Remove(request.TimeMade);
        }
        public void Load()
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
        private void lineToObject(string[] values) {
            Add(new EquipmentMovementRequest(DateTime.ParseExact(values[5], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
            ((App)Application.Current).EquipmentRepository.Get(Convert.ToInt32(values[0])),
                DateTime.ParseExact(values[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                Convert.ToInt32(values[2]), ((App)Application.Current).inventory.items[Convert.ToInt32(values[3])],
                ((App)Application.Current).RoomRepository.Get(Convert.ToInt32(values[4]))));
        }
        public void Dump()
        {
            StreamWriter writer;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            writer = new StreamWriter(filePath);
            foreach (EquipmentMovementRequest equipmentMovementRequest in _requests.Values)
            {
                writer.WriteLine(equipmentMovementRequest.ToString());
            }
            writer.Close();
        }
    }
}
