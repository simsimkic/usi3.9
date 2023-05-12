using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ZdravoCorp.Notifications;

public partial class NotificationsWindow : Window
{
    private Window parent;
    public NotificationsWindow(Window parent, List<Notification> notifications)
    {
        this.parent = parent;
        InitializeComponent();
        foreach (var notification in notifications)
        {
            List.Items.Add(notification);
        }
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (parent != null)
        {
            parent.Visibility = Visibility.Visible;
            parent.Show();
        }
        Close();
    }

    private void NotificationsWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        if (parent != null)
        {
            parent.Visibility = Visibility.Visible;
            parent.Show();
        }
    }
}