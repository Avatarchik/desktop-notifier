/*
    Desktop Notifier
    Copyright (C) 2015 Gowtham (gowthamgowtham@gmail.com)

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

*/
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

        Message msg = new Message( title, text, subText, mNotification.toString(), extras.toString() );

        NotificationTransmitter tx = new NotificationTransmitter();
        try {
            Log.e(MainActivity.TAG, "Sending bluetooth message");
            tx.transmit(address, msg);
        } catch (IOException ioe) {
            Log.e(MainActivity.TAG, "Cannot transmit notification", ioe);
        }
    }

    @Override
    public void onNotificationRemoved(StatusBarNotification sbn) {
        // No implementation needed
    }
}