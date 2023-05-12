using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class RoomRepository
    {
        public Dictionary<int, Room> rooms { get; }
        private int defaultWarehouseID = 2;
        string filePath = "../../../RoomRepository.csv";
        public RoomRepository()
        {
            this.rooms = new Dictionary<int, Room>();
            ReadRooms();
        }
        public Room GetDefaultWarehouse()
        {
            return rooms[defaultWarehouseID];
        }
        public Dictionary<int, Room> GetRooms()
        {
            return rooms;
        }
        public Room Get(int id)
        {
            return rooms[id];
        }
        void Add(Room room)
        {
            this.rooms[room.Id] = room;
        }

        public void ReadRooms()
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
            string[] values = line.Split(",");
            lineToObject(values);
        }
        private void lineToObject(string[] values)
        {
            Add(new Room((Room.Type)Enum.Parse(typeof(Room.Type), values[0]), values[1], Convert.ToInt32(values[2])));
        }
    }
}
