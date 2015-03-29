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
