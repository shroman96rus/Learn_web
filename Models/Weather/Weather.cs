using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace Learn_web.Models
{
    public static class Weather
    {
        public static WeatherResponse GetWeather()
        {
            string url = "https://api.openweathermap.org/data/2.5/weather?q=Yekaterinburg&units=metric&appid=e8719b3f5140ad54cf08f0b99b974ea3";

            HttpWebRequest HttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)HttpWebRequest.GetResponse();

            string respone;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                respone = streamReader.ReadToEnd();

            }
            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(respone);
            weatherResponse.Main.feels_like = Convert.ToInt32(weatherResponse.Main.feels_like);
            return weatherResponse;
        }
    }
}
