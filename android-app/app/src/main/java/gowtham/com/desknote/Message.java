package gowtham.com.desknote;

import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

/**
 * Created by Gowtham on 28-Mar-15.
 */
public class Message {
    public CharSequence title;
    public CharSequence text;
    public CharSequence details;

    public Message(CharSequence title, CharSequence text, CharSequence details) {
        this.title = title;
        this.text = text;
        this.details = details;
    }

    public CharSequence toJSON() {
        JSONObject json = new JSONObject();
        try {
            json.put("title", title);
            json.put("text", text);
            json.put("details", details);
        } catch (JSONException je) {
            Log.wtf("DeskNote", je);
        }
        return json.toString();
    }
}
