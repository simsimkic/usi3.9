using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZdravoCorp.Notifications;

public class NotificationRepository
{
    public List<Notification> Notifications { get; set; }
    private string FilePath = "../../../notifications.csv";

    public NotificationRepository()
    {
        Notifications = new List<Notification>();
        ReadNotifications();
    }
    
    public void ReadNotifications()
    {
        if (!File.Exists(FilePath)) throw new FileNotFoundException(FilePath);
        StreamReader reader = new StreamReader(FilePath);

        string line;
        Notification temp;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            System.Console.WriteLine(line);
            temp = new Notification();
            temp.DecodeFromCSV(line);
            Notifications.Add(temp);
        }
        reader.Close();
    }
    
    private void WriteNotifications()
    {
        if (!File.Exists(FilePath)) throw new FileNotFoundException(FilePath);
        StreamWriter writer = File.CreateText(FilePath);
        
        string line;
        Notification temp;
        foreach (Notification notification in Notifications)
        {
            writer.Write(notification.EncodeToCSV());
            writer.Write("\n");
        }
        writer.Close();
    }

    public List<Notification> GetAllNotificationsByUsername(String username)
    {
        List<Notification> res = new List<Notification>();
        foreach (Notification notification in Notifications)
        {
            if (notification.username.Equals(username)) res.Add(notification);
        }
        return res;
    }
    
    public void RemoveAllNotificationsByUsername(String username)
    {
        Notifications.RemoveAll(item => item.username.Equals(username));
        WriteNotifications();
    }

    public void AddNotification(Notification notification)
    {
        Notifications.Append(notification);
        WriteNotifications();
    }
}