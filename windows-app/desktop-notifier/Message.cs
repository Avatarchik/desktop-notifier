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

        public Message(string json) 
        {
            var obj = JObject.Parse(json);
            if(obj.SelectToken("text") != null)
                Text = obj.SelectToken("text").ToString();
            if (obj.SelectToken("title") != null)
                Title = obj.SelectToken("title").ToString();
            if (obj.SelectToken("icon") != null)
                Image = DecodeImage(obj.SelectToken("icon").ToString());
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
