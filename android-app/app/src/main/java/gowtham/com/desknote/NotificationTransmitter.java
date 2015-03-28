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

    private BluetoothAdapter adapter = BluetoothAdapter.getDefaultAdapter();
    private BluetoothDevice device;
    private BluetoothSocket socket;

    public void transmit(String address, Message message) throws IOException {
        // If bluetooth was disabled, do nothing
        if( ! adapter.isEnabled() ) {
            return;
        }


        device = adapter.getRemoteDevice(address.toUpperCase());
        if( device == null )
            return;

        // Create a socket
        socket = createRfcommSocket(device);

        socket.connect();
        CharSequence json = message.toJSON();
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
