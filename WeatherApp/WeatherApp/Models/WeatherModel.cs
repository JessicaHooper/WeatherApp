using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherModel// temps long lat and humid wx desc
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Location { get; set; }
        public float tempC { get; set; }
        public float tempF { get; set; }
        public float humidityPCT { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public WeatherModel(float Lat, float Lon, string Location, float tempC, float tempF, float humidityPCT, string Description, string icon)
        {
            this.Lat = Lat;
            this.Lon = Lon;
            this.Location = Location;
            this.tempC = tempC;
            this.tempF = tempF;
            this.humidityPCT = humidityPCT;
            this.Description = Description;
            this.Icon = icon;
        }
    }

}
