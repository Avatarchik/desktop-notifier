package gowtham.com.desknote;

import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

import java.io.UnsupportedEncodingException;

/**
 * Created by Gowtham on 28-Mar-15.
 */
public class Message {
    public CharSequence title;
    public CharSequence text;
    public CharSequence details;
    public CharSequence raw;
    public CharSequence extra;

    public Message(CharSequence title, CharSequence text, CharSequence details, CharSequence raw,
                   CharSequence extra) {
        this.title = title;
        this.text = text;
        this.details = details;
        this.raw = raw;
        this.extra = extra;
    }

    public CharSequence toJSON() throws UnsupportedEncodingException {
        JSONObject json = new JSONObject();
        try {
            json.put("title", String.valueOf(title));
            json.put("text", String.valueOf(text));
            json.put("details", String.valueOf(details));
            json.put("raw", String.valueOf(raw));
            json.put("extra", String.valueOf(extra));
        } catch (JSONException je) {
            Log.wtf("DeskNote", je);
        }
        return json.toString();
    }
}
