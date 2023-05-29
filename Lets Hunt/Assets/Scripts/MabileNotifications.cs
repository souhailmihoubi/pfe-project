using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class MabileNotifications : MonoBehaviour
{
    private void Start()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        //Create Android Notification Channel to send messages through!
        var reminderChannel = new AndroidNotificationChannel()
        {
            Id = "reminderNotif",
            Name = "LetsHunt Notification Channel",
            Importance = Importance.Default,
            Description = "Reminder notification",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(reminderChannel);

        //Create the Notification
        var notification = new AndroidNotification();
        notification.Title = "";
        notification.Text = "Enemies are waiting!";
        notification.FireTime = System.DateTime.Now.AddHours(48);

        //Send it
        var idReminder = AndroidNotificationCenter.SendNotification(notification, "reminderNotif");


        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(idReminder) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "reminderNotif");
        }

    }
}
 