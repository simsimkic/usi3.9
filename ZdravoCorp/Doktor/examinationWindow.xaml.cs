using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ZdravoCorp.Doktor.changeAppointment;


namespace ZdravoCorp.Doktor
{
   

    public partial class examinationWindow : Window
    {
        //fields
        public Appointment selectedAppointment;
        public Doctor loggedDoc { get; set; }
        public EquipmentRepository equipmentRepo;
        public Inventory inventoryRepo;
        public Dictionary<string[], int> itemsInRoom;
        public DataTable tableOfItems;
        public string anamnesisContent { get; set; }
       
        
        //constructors
        public examinationWindow(Appointment SelectedAppointment, Doctor loggedDoc)
        {
            DataContext = this; 
            this.loggedDoc = loggedDoc;
            selectedAppointment = loggedDoc.appointmentRepo.getAppointment(SelectedAppointment);
            equipmentRepo = new EquipmentRepository();
            inventoryRepo = new Inventory();
            itemsInRoom = loadRoomEquipment();
            tableOfItems = makeDataTable();
            InitializeComponent();
            loadRoomItemsIntoTable(itemsInRoom);
            loadPatientsInfo();
        }

       
        //helper functions
        public Dictionary<string[], int> loadRoomEquipment()
        {
            //inventoryRepo.SimplifyInventory();
            //Dictionary<string, int[]> itemsInRoom = new Dictionary<string, int[]>();    //name equipment, [id, unr. amount]
            //foreach(InventoryItem item in inventoryRepo.items.Values)
            //{
            //    if(item.Room.Id == selectedAppointment.roomID)  //if that is selected room
            //    {
            //        foreach(KeyValuePair<int, Equipment> pair in equipmentRepo.equipment)   //go through dict and same id as
            //        {                                                                       //id of item in the room
            //            if(pair.Value.name == item.equipment.name)
            //            {
            //                int[] equipmentInfo = { item.equipment.Id, item.amountUnreserved };
            //                itemsInRoom[pair.Value.name] = equipmentInfo;  //dict{nameOfItem - [itemID, amount}

            //            }
            //        }
            //    }
            //}
            Dictionary<string[], int> keyValuePairs = new Dictionary<string[], int>();    //[roomidd, equpimenntID], amount
            foreach(InventoryItem item in inventoryRepo.items.Values) 
            {
                if(item.amountUnreserved == 0)
                {
                    continue;
                }
                string[] key = {item.Room.Id.ToString(), item.equipment.name };
                if (keyValuePairs.ContainsKey(key))
                {
                    keyValuePairs[key] += item.amountUnreserved;
                }
                else {
                    keyValuePairs[key] = item.amountUnreserved;
                }
            }
            return keyValuePairs;
        }


        //main functions
        public bool consumeValidation()
        {
            foreach (DataRow row in tableOfItems.Rows)
            {
                int amount = (int)row[2];    
                int spent = (int)row[3];
                if(spent > amount)
                { MessageBox.Show("Wrong amount of spent equipment is entered");  return false; }
            }
            return true;
        }
        public void updateAmounts()
        {
            foreach(InventoryItem item in inventoryRepo.items.Values)  //all items
            {
                if(item.Room.Id == selectedAppointment.roomID)      //take only items in this room from inventory
                {
                    foreach(DataRow row in tableOfItems.Rows)       //through new data in table that i made
                    {
                        if ((int)row[1] == item.equipment.Id)   //if it is the same item then==>update
                        {
                            item.amount = (int)row[2] - (int)(row[3]);
                            break;
                        }
                    }
                }
            }
        }
        public bool updateInventory()
        {
            if (!consumeValidation())
            {
                return false;
            }
            updateAmounts();
            return true;
        }
        public bool isAnamnesisEmpty()
        {
            if(anamnesisContent.Length == 0)
            {
                MessageBox.Show("You must add anamneses before end of examination.");
                return true;
            }
            return false;
        }
        public bool finishExamination()
        {
            if(!updateInventory()) 
            {
                return false;
            }
            if (isAnamnesisEmpty())
            {
                return false;
            }
            selectedAppointment.status = "finished";
            inventoryRepo.Dump();
            Patient patientToBeExamed = loggedDoc.patientRepo.returnPatient(selectedAppointment.patientUser);
            Anamnesis newAnamnesis = new Anamnesis(anamnesisContent, patientToBeExamed.Username, selectedAppointment.timeStart, selectedAppointment.date);
            loggedDoc.anamnesisRepo.AddAnamnesis(newAnamnesis);
            return true;
        }


        //gui functionality
        public DataTable makeDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name of equipment", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Spent", typeof(int));
            dt.Columns[0].ReadOnly = true;
            dt.Columns[1].ReadOnly = true;
            dt.Columns[2].ReadOnly = true;
            return dt;
        }
        public void loadRoomItemsIntoTable(Dictionary<string[], int> items)
        {
            foreach (KeyValuePair<string[], int> pair in items)
            {
                if (int.Parse(pair.Key[0]) == selectedAppointment.roomID)
                {
                    tableOfItems.Rows.Add(pair.Key[1], pair.Value, 0);
                }
            }

            listOfEquipment.ItemsSource = tableOfItems.DefaultView;
        }
        public void loadPatientsInfo()
        {
            Patient patientToBeExamed = loggedDoc.patientRepo.returnPatient(selectedAppointment.patientUser);
            patName.Content = patientToBeExamed.FirstName;
            patLastName.Content = patientToBeExamed.LastName;
        }
        private void openMedCardBtn_Click(object sender, RoutedEventArgs e)
        {
            medicalBackground win = new medicalBackground(loggedDoc, selectedAppointment.patientUser);
            win.Show();
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        public event WindowClosedEventHandler WindowClosed;
        private void OnWindowClosed()
        {
            if (WindowClosed != null)
            {
                WindowClosed(this, EventArgs.Empty);
            }
        }
        private void CloseWindow()
        {
            // Close the window here
            this.Close();
            // Raise the window closed event
            OnWindowClosed();
        }
        private void FinishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (finishExamination())
            {
                CloseWindow();
            }

        }
    }
}

