package gowtham.com.desknote;

import android.app.Notification;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.util.Log;

import java.io.IOException;

/**
 * Created by Gowtham on 28-Mar-15.
 */
public class MyReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {

        Log.e(MainActivity.TAG, "Received broadcast");
        if (intent != null) {
            Bundle extras = intent.getExtras();
            String notificationTitle =
                    extras.getString(Notification.EXTRA_TITLE);
            Bitmap notificationLargeIcon =
                    ((Bitmap) extras.getParcelable(Notification.EXTRA_LARGE_ICON));
            CharSequence notificationText =
                    extras.getCharSequence(Notification.EXTRA_TEXT);
            CharSequence notificationSubText =
                    extras.getCharSequence(Notification.EXTRA_SUB_TEXT);
            Message msg = new Message(notificationTitle, notificationText, notificationSubText, "", "");
            NotificationTransmitter tx = new NotificationTransmitter();
            try {
                Log.e(MainActivity.TAG, "Sending bluetooth message");
//                tx.transmit(msg);
            } catch (Exception e) {
                Log.e(MainActivity.TAG, "Cannot transmit notification", e);
            }
        }
    }
}
