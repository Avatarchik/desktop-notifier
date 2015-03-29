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
import android.os.Bundle;
import android.preference.ListPreference;
import android.preference.PreferenceFragment;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Created by Gowtham on 28-Mar-15.
 */
public class SettingsFragment extends PreferenceFragment {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Load the preferences from an XML resource
        addPreferencesFromResource(R.xml.prefs);

        ListPreference list = ListPreference.class.cast(findPreference("desktop_address"));

        Map<String,String> map = getAllBluetoothDeviceNames();
        list.setEntries(map.values().toArray(new CharSequence[map.size()]));
        list.setEntryValues(map.keySet().toArray(new CharSequence[map.size()]));
    }

    private Map<String,String> getAllBluetoothDeviceNames() {
        BluetoothAdapter adapter = BluetoothAdapter.getDefaultAdapter();
        Map<String,String> macToNameMap = new LinkedHashMap<String, String>();

        if(adapter.isEnabled()) {
            adapter.cancelDiscovery();
            for(BluetoothDevice device : adapter.getBondedDevices()) {
                macToNameMap.put(device.getAddress(), device.getName());
            }
        } else {
            macToNameMap.put("0", "Bluetooth is off");
        }

        return macToNameMap;
    }
}
