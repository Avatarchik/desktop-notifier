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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.IO;

namespace desktop_notifier
{
    public class Message
    {
        public int Length { get; set; }
        private Dictionary<string, string> extra;

        public string Title
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
        public Image Image
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            set;
        }

        public Message(string json) 
        {
            // How many kbs did we transmit? It is important to keep this number
            // smaller to make the communication quicker
            Length = json.Length / 1024;
            var obj = JObject.Parse(json);
            if(obj.SelectToken("text") != null)
                Text = obj.SelectToken("text").ToString();
            if (obj.SelectToken("title") != null)
                Title = obj.SelectToken("title").ToString();
            if (obj.SelectToken("appname") != null)
                AppName = obj.SelectToken("appname").ToString();
            if (obj.SelectToken("icon") != null)
                Image = DecodeImage(obj.SelectToken("icon").ToString());

            extra = new Dictionary<string, string>();
            if (obj["extra"] != null)
            {
                extra = obj["extra"].ToObject<Dictionary<string, string>>();

                if ("null".Equals(Text))
                {
                    if (extra.ContainsKey("android.text"))
                    {
                        Text = extra["android.text"];
                    }

                }
                if ("null".Equals(Title))
                {
                    if (extra.ContainsKey("android.title"))
                    {
                        Title = extra["android.title"];
                    }
                }
            }
            
            
        }

        private System.Drawing.Image DecodeImage(string base64)
        {
            if (base64 == "null")
                return null;
            byte[] buffer = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }
    }
}
