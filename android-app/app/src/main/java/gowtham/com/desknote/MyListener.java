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
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.ColorMatrix;
import android.graphics.ColorMatrixColorFilter;
import android.graphics.Paint;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.service.notification.NotificationListenerService;
import android.service.notification.StatusBarNotification;
import android.util.Base64;
import android.util.Base64OutputStream;
import android.util.Log;
import android.support.v4.content.LocalBroadcastManager;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

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
        Set<String> emptySet = new HashSet<String>();
        Collection<String> address = pref.getStringSet("desktop_address", emptySet);

        Bundle extras = mNotification.extras;

        String packageName = sbn.getPackageName();
        String title = extras.getString(Notification.EXTRA_TITLE);
        String text = extras.getString(Notification.EXTRA_TEXT);
        String subText = extras.getString(Notification.EXTRA_SUB_TEXT);
        Integer smallIconID = extras.getInt(Notification.EXTRA_SMALL_ICON);
        String icon = "null";
        if(pref.getBoolean("include_images", false)) {
            if (extras.getParcelable(Notification.EXTRA_LARGE_ICON) != null) {
                Bitmap b = Bitmap.class.cast(extras.getParcelable(Notification.EXTRA_LARGE_ICON));
                icon = bitmap2Base64(b);
            } else {
                icon = getIcon(packageName, smallIconID);
            }
        }

        Map<String,String> extrasMap = new HashMap<String,String>();
        for(String key : extras.keySet()) {
            extrasMap.put(key, String.valueOf(extras.get(key)));
        }

        Log.e(MainActivity.TAG, "Got a new notification " + title + " " + mNotification.hashCode());

        Message msg = new Message( title, text, subText, icon, mNotification.toString(), extrasMap, packageName );
        NotificationTransmitter tx = new NotificationTransmitter();

        Log.e(MainActivity.TAG, "Sending bluetooth message");
        tx.transmit(address, msg);
    }

    private String getIcon(String packageName, Integer id) {
        String icon = "null";
        if(id != 0) {
            Context context = null;
            try {
                context = createPackageContext(packageName, CONTEXT_IGNORE_SECURITY);
                Drawable d = context.getResources().getDrawable(id);
                Bitmap b = BitmapDrawable.class.cast(d).getBitmap();
                icon = bitmap2Base64(b);
            } catch (PackageManager.NameNotFoundException e) {
                // Ignore
            }
        }

        return icon;
    }

    private String bitmap2Base64(Bitmap b) {
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        //Bitmap gray = toGrayscale(b);
        // Windows needs 
        Bitmap smaller = b.createScaledBitmap(b, 48, 48, false);
        // PNG is lossless. So, quality setting is unused
        smaller.compress(Bitmap.CompressFormat.PNG, 100, baos);
        byte[] buf = baos.toByteArray();
        return new String(Base64.encode(buf, Base64.NO_WRAP));
    }

    private Bitmap toGrayscale(Bitmap bmpOriginal)
    {
        int width, height;
        height = bmpOriginal.getHeight();
        width = bmpOriginal.getWidth();

        Bitmap bmpGrayscale = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        Canvas c = new Canvas(bmpGrayscale);
        Paint paint = new Paint();
        ColorMatrix cm = new ColorMatrix();
        cm.setSaturation(0);
        ColorMatrixColorFilter f = new ColorMatrixColorFilter(cm);
        paint.setColorFilter(f);
        c.drawBitmap(bmpOriginal, 0, 0, paint);
        return bmpGrayscale;
    }

    @Override
    public void onNotificationRemoved(StatusBarNotification sbn) {
        // No implementation needed
    }
}