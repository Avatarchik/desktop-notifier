package gowtham.com.desknote;
import android.app.Notification;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.service.notification.NotificationListenerService;
import android.service.notification.StatusBarNotification;
import android.util.Log;
import android.support.v4.content.LocalBroadcastManager;

import java.io.IOException;

public class MyListener extends NotificationListenerService {

    @Override
    public void onNotificationPosted(StatusBarNotification sbn) {
        SharedPreferences pref = PreferenceManager.getDefaultSharedPreferences(this);

        // If user has disabled notifications, then skip
        if( ! pref.getBoolean("send_notifications", false))
            return;

        Notification mNotification=sbn.getNotification();

        // Can't do much if we get a null!
        if (mNotification==null)
            return;

        // Look for our device
        String address = pref.getString("desktop_address", "");

        Bundle extras = mNotification.extras;

        String title = extras.getString(Notification.EXTRA_TITLE);
        String text = extras.getString(Notification.EXTRA_TEXT);
        String subText = extras.getString(Notification.EXTRA_SUB_TEXT);

        Log.e(MainActivity.TAG, "Got a new notification " + title + " " + mNotification.hashCode());

        Message msg = new Message( title, text, subText );

        NotificationTransmitter tx = new NotificationTransmitter();
        try {
            Log.e(MainActivity.TAG, "Sending bluetooth message");
            tx.transmit(address, msg);
        } catch (IOException ioe) {
            Log.e(MainActivity.TAG, "Cannot transmit notification", e);
        }
    }

    @Override
    public void onNotificationRemoved(StatusBarNotification sbn) {
        // No implementation needed
    }
}