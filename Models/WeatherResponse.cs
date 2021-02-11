using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class WeatherResponse
    {
       
        public getlist[] weather { get; set; }
        public GetTemperature Main { get; set; }

        public string Name { get; set; }
    }

    public class GetTemperature
    {
        public float Temp { get; set; }

        public float feels_like { get; set; }

        public float temp_min { get; set; }

        public float temp_max { get; set; }

        public float humidity { get; set; }

    }

   
    public class getlist
    {
        public string main { get; set; }
    }

}
