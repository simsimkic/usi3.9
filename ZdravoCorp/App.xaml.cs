using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Doktor;
using ZdravoCorp.Notifications;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public PatientRepository patientRepository { get; set; }
        public AnamnesisRepository AnamnesisRepository { get; set; }
        InventoryView inventoryView { get; set; }
        RoomEquipmentView roomEquipmentView { get; set; }
        public Inventory inventory{ get; set; }
        public RoomRepository RoomRepository{ get; set; }
        public EquipmentRepository EquipmentRepository{ get; set; }
        public UserRepository UserRepository { get; set; }
        public appointmentRepository AppointmentRepository { get; set; }
        public NotificationRepository NotificationRepository { get; set; }
        public EquipmentRequestRepository EquipmentRequestRepository { get; set; }
        public EquipmentMovementRequestRepository EquipmentMovementRequestRepository { get; set; }
        public User LoggedInUser { get; set; }
        public EquipmentRequestService EquipmentRequestService { get; set; }
        public EquipmentMovementRequestService EquipmentMovementRequestService { get; set; }
        App() {
            InitializeComponent();
            RoomRepository = new RoomRepository();
            EquipmentRepository = new EquipmentRepository();
            inventory = new Inventory();
            UserRepository = new UserRepository();
            //Room room = new Room(Room.Type.OPERATION, "SALA ZA OPERACIJE 1");
            //Room warehouse = new Room(Room.Type.WAREHOUSE, "SKLADISTE 1");
            //Equipment equipment = new Equipment("Skalpel", Equipment.Type.OPERATION);
            //inventory.Add(new InventoryItem(15, equipment, room));
            //inventory.Add(new InventoryItem(10, equipment, room));
            //inventory.Add(new InventoryItem(10, equipment, room));
            //inventory.Add(new InventoryItem(10, equipment, warehouse));
            patientRepository = new PatientRepository();
            EquipmentRequestRepository = new EquipmentRequestRepository();
            EquipmentMovementRequestRepository = new EquipmentMovementRequestRepository();
            inventoryView = new InventoryView(inventory);
            AnamnesisRepository = new AnamnesisRepository();
            AppointmentRepository = new appointmentRepository();
            NotificationRepository = new NotificationRepository();
            roomEquipmentView = new RoomEquipmentView(inventory);
            EquipmentRequestService = new EquipmentRequestService();
            EquipmentMovementRequestService = new EquipmentMovementRequestService();
            EquipmentMovementRequestService.FullFillEquipmentMovementRequests();
        }

        public InventoryView GetInventoryView()
        {
            return inventoryView;
        }
        public RoomEquipmentView GetRoomEquipmentView()
        {
            return roomEquipmentView;
        }


        public bool Login(User user)
        {
            return UserRepository.HasUser(user);
        }
        public void SetLoggedInUser(User user)
        {
            LoggedInUser = user;
        }
        public void OpenCorrespondingWindow() {
            //TODO: Kada bude mergovano sa granama koje implementiraju prozore za druge korisnike, ovde ide switch
            //koji otvara odgovarajuci prozor na osnovu LoggedInUser.Role
        }
    }
}
