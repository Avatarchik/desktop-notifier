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

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.preference.PreferenceManager;
import android.util.Log;

import java.io.IOException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.UUID;

/**
 * Created by Gowtham on 28-Mar-15.
 */
public class NotificationTransmitter {

    private static final UUID uuid = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
    private BluetoothAdapter adapter = BluetoothAdapter.getDefaultAdapter();
    private BluetoothDevice device;
    private BluetoothSocket socket;

    public void transmit(String address, Message message) throws IOException {
        // If bluetooth was disabled, do nothing
        if( ! adapter.isEnabled() ) {
            return;
        }


        device = adapter.getRemoteDevice(address.toUpperCase());
        if( device == null ) {
            Log.e(MainActivity.TAG, "Device " + address + " not found");
            return;
        }


        // Create a socket
        // socket = createRfcommSocket(device); // device.createRfcommSocketToServiceRecord(uuid);
        socket = device.createRfcommSocketToServiceRecord(uuid);

        adapter.cancelDiscovery();
        socket.connect();
        CharSequence json = message.toJSON() + ":END_OF_MESSAGE:";
        socket.getOutputStream().write(json.toString().getBytes());
        socket.getOutputStream().flush();
        socket.close();
        Log.e(MainActivity.TAG, "Done");
    }

    // http://habrahabr.ru/post/144547/
    private BluetoothSocket createRfcommSocket(BluetoothDevice device) {
        BluetoothSocket tmp = null;
        try {
            Class class1 = device.getClass();
            Class aclass[] = new Class[1];
            aclass[0] = Integer.TYPE;
            Method method = class1.getMethod("createRfcommSocket", aclass);
            Object aobj[] = new Object[1];
            aobj[0] = Integer.valueOf(1);
            tmp = (BluetoothSocket) method.invoke(device, aobj);
        } catch (NoSuchMethodException e) {
            Log.e(MainActivity.TAG, "createRfcommSocket() failed", e);
        } catch (InvocationTargetException e) {
            Log.e(MainActivity.TAG, "createRfcommSocket() failed", e);
        } catch (IllegalAccessException e) {
            Log.e(MainActivity.TAG, "createRfcommSocket() failed", e);
        }
        return tmp;
    }
}
