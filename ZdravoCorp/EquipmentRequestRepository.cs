using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.Serialization;

namespace ZdravoCorp
{
    public class EquipmentRequestRepository
    {
        string filePath = "../../../EquipmentRequestRepository.csv";

        public Dictionary<DateTime, EquipmentRequest> _requests { get; set; }

        public EquipmentRequestRepository() { 
            _requests = new Dictionary<DateTime, EquipmentRequest>();
            Load();
        }

        public void Add(EquipmentRequest request)
        {
            _requests.Add(request.TimeMade, request);
        }
        public EquipmentRequest Get(DateTime key)
        {
            return _requests[key];
        }
        public void Remove(EquipmentRequest request)
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
        private void lineToObject(string[] values)
        {
            Add(new EquipmentRequest(DateTime.ParseExact(values[3], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                ((App)Application.Current).EquipmentRepository.Get(Convert.ToInt32(values[0])),
                Convert.ToInt32(values[1]), Convert.ToInt32(values[2])));
        }
        public void Dump()
        {
            StreamWriter writer;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            writer = new StreamWriter(filePath);
            foreach (EquipmentRequest equipmentRequest in _requests.Values)
            {
                writer.WriteLine(equipmentRequest.ToString());
            }
            writer.Close();
        }
    }
}