using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marquee_Editor.Models
{
    public class Mqtt
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Text { get; set; }
    }
}