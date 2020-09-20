﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Iot_HUB.Models
{
    class TemperatureApiModel
    {
        public class Rootobject
        {
            public Current current { get; set; }
        }

        public class Current
        {
            public double temp { get; set; }
            public int humidity { get; set; }
        }
    }
}
