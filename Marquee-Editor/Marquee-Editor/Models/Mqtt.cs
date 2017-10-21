using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marquee_Editor.Models
{
    public class Mqtt
    {
        public string Topic { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public DateTime Date { get; set; }
    }
}