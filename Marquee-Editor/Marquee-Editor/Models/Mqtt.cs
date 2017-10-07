using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marquee_Editor.Models
{
    public class Mqtt
    {
        public string Topic { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string PreFunc { get; set; }

        public string PostFunc { get; set; }
    }
}