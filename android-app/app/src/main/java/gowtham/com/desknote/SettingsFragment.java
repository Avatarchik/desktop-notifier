package gowtham.com.desknote;

import android.os.Bundle;
import android.preference.PreferenceFragment;

/**
 * Created by Gowtham on 28-Mar-15.
 */
public class SettingsFragment extends PreferenceFragment {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Load the preferences from an XML resource
        addPreferencesFromResource(R.xml.prefs);
    }

}