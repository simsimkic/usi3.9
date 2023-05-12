using System;
using System.Text;

namespace ZdravoCorp.Notifications;

public class Notification
{
   public String username { get; set; }
   public DateTime date { get; set; }
   public String notificationBody { get; set; }

   public Notification() { }

   public Notification(String username, DateTime date, String notificationBody)
   {
      this.username = username;
      this.date = date;
      this.notificationBody = notificationBody;
   }

   public void DecodeFromCSV(string input)
   {
      string[] parts = input.Split(',');
      this.username = parts[0];
      this.date = DateTime.Parse(parts[1]);
      this.notificationBody = parts[2];
   }

   public string EncodeToCSV()
   {
      StringBuilder res = new StringBuilder();
      res.Append(username);
      res.Append(',');
      res.Append(date);
      res.Append(',');
      res.Append(notificationBody);
      return res.ToString();
   }
}